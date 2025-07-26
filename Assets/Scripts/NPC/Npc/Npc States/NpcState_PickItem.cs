using UnityEngine;

public class NpcState_PickItem : IState
{
    readonly NpcController _npc;
    public NpcState_PickItem(NpcController npc){ _npc = npc; }

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
        // 애니 끝났다고 가정하고 바로 나가기
        if (!_npc.HasItemInHand) return; // 애니메이션 이벤트로 true 될 때까지 대기
        _npc.SM.SetState(new NpcState_Leave(_npc));
    }
    public void Exit() { }
}