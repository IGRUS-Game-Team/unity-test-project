using UnityEngine;

public class NpcState_PickItem : IState
{
    readonly NpcController _npc;
    QueueManager _queue;
    public NpcState_PickItem(NpcController npc)
    {
        _npc = npc;
        _queue = Object.FindObjectOfType<QueueManager>();
    }

    public void Enter()
    {
        // 에이전트 완전 정지
        _npc.Agent.isStopped = true;
        _npc.Agent.ResetPath();

        var itemY = _npc.TargetShelfSlot.position.y;
        string clipName = itemY > 1.4f ? "ReachHigh" : itemY < 0.6f ? "ReachLow" : "ReachMid";

        _npc.Anim.Play(clipName);
    }

    public void Tick()
    {
        if (!_npc.HasItemInHand) return;             // 아이템 집기 완료 여부

        // QueueManager 캐싱(한 번만 찾음)
        if (_queue == null)
        {
            _queue = Object.FindObjectOfType<QueueManager>();
        }
        
        // (1) 큐 매니저가 있으면 줄 서러 가기
        if (_queue != null)
        {
            _npc.SM.SetState(new NpcState_ToQueue(_npc, _queue));   // ← 여기!
        }
        // (2) 없으면 기존 로직대로 바로 퇴장
        else
        {
            _npc.SM.SetState(new NpcState_Leave(_npc));
        }
    }

    public void Exit() { }
}