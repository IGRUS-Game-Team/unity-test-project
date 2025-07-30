using UnityEngine;

// NPC가 줄에 서서 대기하는 동안 처리하는 상태
public class NpcState_QueueWait : IState
{
    private readonly NpcController npcController;  // 이 상태의 대상 NPC
    private readonly QueueManager queueManager;    // 줄 관리 매니저
    private readonly Transform myNode;             // NPC가 대기 중인 자리

    private const string StandingAnim = "Standing"; // 대기 애니메이션 이름

    // 생성자: 필요한 참조를 받아 필드에 할당
    public NpcState_QueueWait(
        NpcController npcController,
        QueueManager  queueManager,
        Transform     node)
    {
        this.npcController = npcController;  // 필드 초기화
        this.queueManager  = queueManager;
        this.myNode        = node;
    }

    // 상태 시작 시 호출: 이동·회전 멈추고 애니메이션 재생
    public void Enter()
    {
        npcController.agent.isStopped      = true;                   // 이동 중지
        npcController.agent.updateRotation = false;                  // 자동 회전 중단
        npcController.transform.LookAt(queueManager.CounterTransform); // 카운터 쪽 바라보기
        npcController.animator.Play(StandingAnim);                   // 대기 애니메이션 실행
    }

    // 매 프레임 호출: 자리 이동·결제 전환 로직
    public void Tick()
    {
        // 1) 줄이 앞으로 당겨져서 내 자리가 바뀌었는지 확인
        if (npcController.QueueTarget != this.myNode)
        {
            npcController.agent.isStopped = false;                   // 이동 재개
            npcController.agent.SetDestination(npcController.QueueTarget.position); // 새 자리로 이동
            return;                                                  // 이후 로직 실행 안 함
        }

        // 2) 내가 줄 맨 앞이고, 아이템을 이미 들고 있으면 결제 상태로 전환
        if (queueManager.IsFront(npcController) && npcController.hasItemInHand)
        {
            npcController.stateMachine.SetState(
                new NpcState_OfferPayment(
                    npcController,                                 // NPC 컨트롤러
                    queueManager,                                  // 줄 관리 매니저
                    npcController.CashPrefab,                      // 현금 프리팹
                    npcController.CardPrefab,                      // 카드 프리팹
                    queueManager.CounterTransform));                // 계산대 위치
        }
    }

    // 상태 종료 시 호출: 자동 회전 복원
    public void Exit()
    {
        npcController.agent.updateRotation = true;                   // 자동 회전 재개
    }
}