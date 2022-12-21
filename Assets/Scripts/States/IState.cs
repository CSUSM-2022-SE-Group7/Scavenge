namespace LocalV2
{
    public interface IState
    {
        public void EnterState();
        public void Execute();
        public void ExitState();
        string Name { get; }
    }
}