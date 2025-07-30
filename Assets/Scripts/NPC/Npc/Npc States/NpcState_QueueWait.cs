using UnityEngine;

// NPC가 줄에 서서 대기 중, 자리 이동 시 걷기 애니메이션, 도착 후 대기 애니메이션 재생
public class NpcState_QueueWait : IState
{
    private readonly NpcController npcController;  // 이 상태를 수행할 NPC
    private readonly QueueManager queueManager;    // 줄 관리 매니저
    private Transform myNode;                      // NPC가 현재 서 있는 자리

    private const string StandingAnim   = "Standing";  // 대기 애니메이션 이름
    private const string WalkingAnim    = "Walking";   // 걷기 애니메이션 이름
    private const float  ArrivalDistance = 0.1f;       // 도착 허용 오차 거리

    public NpcState_QueueWait(
        NpcController npcController,
        QueueManager  queueManager,
        Transform     node)
    {
        this.npcController = npcController;  // NPC 참조 저장
        this.queueManager  = queueManager;   // 매니저 참조 저장
        this.myNode        = node;           // 초기 자리 저장
    }

    public void Enter()
    {
        npcController.agent.ResetPath();                             // 이전 이동 경로 완전 초기화
        npcController.agent.isStopped      = true;                   // 이동 정지
        npcController.agent.updateRotation = false;                  // 자동 회전 중단
        npcController.transform.LookAt(queueManager.CounterTransform); // 카운터 방향으로 회전
        npcController.animator.Play(StandingAnim);                   // 대기 애니메이션 실행
    }

    public void Tick()
    {
        // 1) 자리가 변경된 경우 → 걷기 애니메이션, 새 자리로 이동
        if (npcController.QueueTarget != this.myNode)
        {
            npcController.agent.ResetPath();                             // 경로 재설정
            npcController.agent.isStopped      = false;                  // 이동 재개
            npcController.agent.SetDestination(npcController.QueueTarget.position); // 새 자리 목적지 설정
            npcController.animator.Play(WalkingAnim);                    // 걷기 애니메이션 실행
            this.myNode = npcController.QueueTarget;                     // 현재 자리 업데이트
            return;                                                      // 이동만 처리하고 종료
        }

        // 2) 도착 여부 판단: 경로 계산 완료 + 거리 허용 범위 이내
        bool arrived = npcController.agent.pathPending == false &&
                       npcController.agent.remainingDistance <= ArrivalDistance;

        // 3) 도착했으면 대기 애니메이션으로 전환
        if (arrived)
        {
            npcController.agent.isStopped = true;        // 완전 정지
            npcController.animator.Play(StandingAnim);   // 대기 애니메이션 재생
        }

        // 4) 맨 앞이고 물건을 들고 있으면 결제 상태로 전환
        if (arrived && queueManager.IsFront(npcController) && npcController.hasItemInHand)
        {
            npcController.stateMachine.SetState(
                new NpcState_OfferPayment(
                    npcController,
                    queueManager,
                    npcController.CashPrefab,
                    npcController.CardPrefab,
                    queueManager.CounterTransform));           // 결제 상태로 이동
        }
    }

    public void Exit()
    {
        npcController.agent.updateRotation = true;         // 자동 회전 복원
    }
}