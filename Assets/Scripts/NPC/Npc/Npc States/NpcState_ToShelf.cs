using UnityEngine;

public class NpcState_ToShelf : IState
{
    readonly NpcController _npc;
    public NpcState_ToShelf(NpcController npc) { _npc = npc; }

    public void Enter()
    {
        _npc.Agent.isStopped = false;
        _npc.Agent.SetDestination(_npc.TargetShelfSlot.position);
        _npc.Anim.Play("Walking");
    }

    public void Tick()
    {
        if (_npc.Agent.pathPending) return;

        bool arrived = _npc.Agent.remainingDistance <= 0.1f && _npc.Agent.velocity.sqrMagnitude < 0.01f;

        if (!arrived) return;

        _npc.Agent.updateRotation = false;              // 에이전트 자동 회전 끔

        Vector3 lookdir = -_npc.TargetShelfSlot.forward;
        lookdir.y = 0f;                                      // 위아래 방향 제거

        if (lookdir != Vector3.zero)
        {
            _npc.transform.rotation = Quaternion.LookRotation(lookdir); 
        }

        _npc.Agent.isStopped = true;      // ← 도착 후 즉시 정지
        _npc.SM.SetState(new NpcState_PickWait(_npc));
    }

    public void Exit() { }
}