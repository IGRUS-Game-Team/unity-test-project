using System.Collections;
using UnityEngine;

public class NpcState_PickWait : IState
{
    readonly NpcController _npc;
    float _waitEnd;

    public NpcState_PickWait(NpcController npc){ _npc = npc; }

    public void Enter()
    {
        _waitEnd = Time.time + Random.Range(1f, 3f);
        _npc.Anim.Play("Standing");
        _npc.Agent.isStopped = true;
    }
    public void Tick()
    {
        if (Time.time < _waitEnd) return;
        _npc.SM.SetState(new NpcState_PickItem(_npc));
    }
    public void Exit() { }
}