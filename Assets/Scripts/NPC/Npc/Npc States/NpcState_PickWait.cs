using UnityEngine;

public class NpcState_PickWait : IState
{
    readonly NpcController _npc;
    float _waitEnd;

    public NpcState_PickWait(NpcController npc){ _npc = npc; }

    public void Enter()
    {
        _waitEnd = Time.time + Random.Range(1f, 3f);
        _npc.Agent.isStopped = true;
        _npc.Anim.Play("Standing");
        _npc.Agent.updateRotation = false;
    }
    public void Tick()
    {
    /* 매 프레임 선반 방향 고정 */
        KeepFacingShelf();

        if (Time.time < _waitEnd) return;
        _npc.SM.SetState(new NpcState_PickItem(_npc));
    }

    void KeepFacingShelf()
    {
        Transform shelf = _npc.TargetShelfSlot.parent ?? _npc.TargetShelfSlot;

        Vector3 forward = shelf.forward;
        forward.y = 0f;
        if (forward == Vector3.zero) return;

        _npc.transform.rotation = Quaternion.LookRotation(forward);
    }

    public void Exit() { _npc.Agent.updateRotation = true; }
}