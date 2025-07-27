public class StateMachine
{
    IState _current;
    public void SetState(IState next)
    {
        _current?.Exit();
        _current = next;
        _current.Enter();
    }
    public void Tick() => _current?.Tick();
}