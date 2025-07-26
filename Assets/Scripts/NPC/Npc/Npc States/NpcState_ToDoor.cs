using UnityEngine;

public class NpcState_ToDoor : IState
{
    readonly NpcController _npc;
    readonly Transform     _door;

    public NpcState_ToDoor(NpcController npc, Transform door){ _npc = npc; _door = door; }

    public void Enter()
    {
        _npc.Agent.isStopped = false;
        _npc.Agent.SetDestination(_door.position);
        _npc.Anim.Play("Walking");
    }
    public void Tick() { /* 문 앞 Trigger가 처리하므로 여기선 아무것도 안함 */ }
    public void Exit() { }
}
