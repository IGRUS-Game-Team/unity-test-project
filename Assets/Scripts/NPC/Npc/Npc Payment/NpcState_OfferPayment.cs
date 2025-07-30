using UnityEngine;    // Quaternion, Vector3

// 계산대 앞에서 결제 수단(현금·카드)을 내밀고 플레이어 클릭을 기다리는 상태
public class NpcState_OfferPayment : IState
{
    /* ---------- 의존성 필드 ---------- */

    private readonly NpcController npcController;        // 상태를 실행할 NPC
    private readonly QueueManager queueManager;          // 줄 관리 매니저
    private readonly GameObject cashPrefab;              // 현금 프리팹
    private readonly GameObject cardPrefab;              // 카드 프리팹
    private readonly Transform counterTransform;         // 계산대 위치

    /* ---------- 상수 ---------- */

    private const float RotationSpeedDeg = 720f;         // NPC가 몸을 돌릴 속도(도/초)
    private const float AngleEpsilon    = 1f;            // 목표 회전과 허용 오차(도)

    /* ---------- 내부 상태 ---------- */

    private bool itemSpawned;                            // 소지품을 손에 생성했는가?
    private bool rotationComplete;                       // 목표 방향으로 정확히 선 뒤 true
    private PaymentContext paymentContext;               // 결제 정보

    /* ---------- 생성자 ---------- */

    // 외부에서 필요한 요소를 모두 주입받아 상태를 만든다.
    public NpcState_OfferPayment(
        NpcController npcController,
        QueueManager   queueManager,
        GameObject     cashPrefab,
        GameObject     cardPrefab,
        Transform      counter)
    {
        this.npcController   = npcController;            // 필드와 매개변수 이름 충돌 → this. 사용
        this.queueManager    = queueManager;
        this.cashPrefab      = cashPrefab;
        this.cardPrefab      = cardPrefab;
        this.counterTransform = counter;
    }

    /* ---------- IState 구현 ---------- */

    // 상태 시작: 결제 데이터를 준비하고 이동·회전을 잠시 멈춘다
    public void Enter()
    {
        // (1) 결제 정보 구조체 생성
        paymentContext = new PaymentContext
        {
            totalPrice = 0f,                             // TODO: 실제 가격 로직
            method     = Random.value < 0.5f ? PaymentType.Cash : PaymentType.Card,
            payer      = npcController                   // 본인을 payer로 지정
        };

        // (2) NavMeshAgent 이동·회전 중단
        npcController.agent.isStopped      = true;       // 이동 정지
        npcController.agent.updateRotation = false;      // 자동 회전 끔
    }

    // 매 프레임: 몸을 계산대 쪽으로 돌리고, 다 돌면 결제 물건을 생성
    public void Tick()
    {
        // 이미 정확히 돌았으면 이동·회전 로직 건너뜀
        if (rotationComplete)
        {
            return;
        }

        // (1) NPC → 계산대 방향 벡터(수평) 계산
        Vector3 direction = counterTransform.position - npcController.transform.position;
        direction.y = 0f;                                // 수평 회전만 고려

        // (2) 원하는 최종 회전 값
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // (3) 현재 회전을 목표 회전에 근접하게 보간
        npcController.transform.rotation = Quaternion.RotateTowards(
            npcController.transform.rotation,
            targetRotation,
            RotationSpeedDeg * Time.deltaTime);

        // (4) 목표 각도에 도달했는지 검사
        float angle = Quaternion.Angle(npcController.transform.rotation, targetRotation);
        if (angle <= AngleEpsilon)
        {
            // 4-1) 잔여 오차를 0으로 스냅
            npcController.transform.rotation = targetRotation;

            // 4-2) 앞으로 Tick에서 더 돌리지 않음
            rotationComplete = true;
        }

        // (5) 정확히 맞춘 그 프레임에만 결제 물건을 손에 생성
        if (!itemSpawned && rotationComplete)
        {
            SpawnPaymentItem();
            itemSpawned = true;
        }
    }

    // 상태 종료: NavMeshAgent 회전·이동 제어를 원래대로 복원
    public void Exit()
    {
        npcController.agent.updateRotation = true;       // 자동 회전 복구
        npcController.agent.isStopped      = false;      // 이동 재개
    }

    /* ---------- 내부 보조 메서드 ---------- */

    // 손 소켓에 현금 또는 카드를 생성하고 클릭 이벤트를 연결
    private void SpawnPaymentItem()
    {
        Transform hand = npcController.HandTransform;    // 손 위치
        GameObject prefab = paymentContext.method == PaymentType.Cash ? cashPrefab : cardPrefab;

        // 손·프리팹이 없으면 바로 퇴장 상태로 전환
        if (hand == null || prefab == null)
        {
            npcController.stateMachine.SetState(new NpcState_Leave(npcController));
            return;
        }

        // (1) 손에 프리팹 인스턴스 생성
        GameObject item = Object.Instantiate(prefab, hand);

        // (2) 약간 앞으로 이동해 손 모델과 겹침 방지
        item.transform.localPosition = Vector3.forward * 0.05f;
        item.transform.localRotation = Quaternion.identity;

        // (3) 클릭 시 호출될 콜백 등록
        item.GetComponent<PaymentItem>().Init(paymentContext, OnPaid);
    }

    // 플레이어가 아이템을 클릭해서 결제를 완료했을 때 호출
    private void OnPaid()
    {
        // (1) 줄 관리 매니저에 맨 앞 결제 완료 알림
        queueManager.DequeueFront();

        // (2) NPC를 퇴장 상태로 전환
        npcController.stateMachine.SetState(new NpcState_Leave(npcController));
    }
}