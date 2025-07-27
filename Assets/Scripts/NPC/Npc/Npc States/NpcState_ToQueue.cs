using UnityEngine;

public class NpcState_ToQueue : IState
{
    readonly NpcController _npc;
    readonly QueueManager  _queue;

    Transform _myNode;

    public NpcState_ToQueue(NpcController npc, QueueManager queue)
    { _npc = npc; _queue = queue; }

    public void Enter()
    {
        // 줄 시도
        if (!_queue.TryEnqueue(_npc, out Transform myNode))
        {
            // 줄 가득 참 → Wander 로 전환
            var points = _queue.WanderPoints;

            if (points == null || points.Length == 0)
            {
                // 배회 포인트 없으면 그냥 Leave
                _npc.SM.SetState(new NpcState_Leave(_npc));
            }
            else
            {
                _npc.SM.SetState(new NpcState_Wander(_npc, points, 20f));
            }
            return;
        }

        // 자리 확보 성공
        _npc.SetQueueTarget(myNode);
        _myNode = myNode;
        _npc.Anim.Play("Walking");
    }

    public void Tick()
    {
        // 이동 중엔 몸을 앞사람(또는 Counter) 쪽으로 살짝 돌려 줘도 자연스러움
        _npc.FaceLookTarget(540f);                 // ← 추가 (수치는 원하는 회전 속도)

        if (_npc.Agent.pathPending) return;

        bool arrived =
            _npc.Agent.remainingDistance < 0.05f &&
            _npc.Agent.velocity.sqrMagnitude < 0.01f;

        if (!arrived) return;

        // 노드 도착 → 대기 상태로
        _npc.Agent.isStopped = true;
        _npc.SM.SetState(new NpcState_QueueWait(_npc, _queue, _myNode));
    }

    public void Exit() { }
}