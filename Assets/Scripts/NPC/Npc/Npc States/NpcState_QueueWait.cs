using UnityEngine;

public class NpcState_QueueWait : IState
{
    readonly NpcController _npc;
    readonly QueueManager  _queue;
    readonly Transform     _myNode;

    public NpcState_QueueWait(NpcController npc, QueueManager queue, Transform node)
    { _npc = npc; _queue = queue; _myNode = node; }

    public void Enter()
    {
        _npc.Agent.isStopped = true;
        _npc.Agent.updateRotation = false;          // 고정 시선
        _npc.Anim.Play("Standing");
        
        // 바로 한 번 돌려 놓고 시작
        _npc.FaceLookTarget(999f);                  // 999 → 즉시 스냅
    }

    public void Tick()
    {
        /* 1) 몸을 LookTarget 쪽으로 계속 보정 */
        _npc.FaceLookTarget(540f);                  // 부드러운 회전 속도

        /* 2) 줄이 한 칸 전진했을 때 새 노드 지정 */
        if (_npc.QueueTarget != _myNode)
        {
            _npc.Agent.isStopped = false;
            _npc.Agent.SetDestination(_npc.QueueTarget.position);
            return;
        }

        /* 3) 맨 앞 & 아이템 들고 있으면 결제 상태 */
        if (_queue.IsFront(_npc) && _npc.HasItemInHand)
        {
            _npc.SM.SetState(
                new NpcState_OfferPayment(
                _npc, _queue,
                _npc.CashPrefab, _npc.CardPrefab,
                _queue.Counter));
        }
    }

    public void Exit()
    {
        _npc.Agent.updateRotation = true;   // 회전 다시 허용
    }
}
