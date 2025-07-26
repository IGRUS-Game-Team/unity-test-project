public class NpcState_Leave : IState
{
    readonly NpcController _npc;
    readonly ShelfSlot _slot;                       // 슬롯 기억용

    public NpcState_Leave (NpcController npc)
    {
        _npc = npc;
        _slot = _npc.TargetShelfSlot?.GetComponent<ShelfSlot>();
    }

    public void Enter()
    {
        _slot?.Release();
        _npc.IsLeaving = true;

        _npc.Agent.ResetPath();
        _npc.Agent.isStopped = false;
        _npc.Agent.SetDestination(_npc.ExitPoint.position);
        _npc.Anim.Play("Walking");
    }
    
    public void Tick() { }
    public void Exit() { }
}