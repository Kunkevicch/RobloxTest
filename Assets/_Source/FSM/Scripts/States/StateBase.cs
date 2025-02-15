namespace RobloxTest
{
    public abstract class StateBase
    {
        protected readonly FSM _fsm;

        public virtual void Enter() { }

        public StateBase(FSM fsm)
        {
            _fsm = fsm;
        }
    }
}
