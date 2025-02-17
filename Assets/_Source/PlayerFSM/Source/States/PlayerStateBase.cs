namespace RobloxTest
{
    public abstract class PlayerStateBase
    {
        protected readonly PlayerFSM _fsm;
        protected readonly PlayerController _player;
        public PlayerStateBase(PlayerFSM playerFSM, PlayerController controller)
        {
            _fsm = playerFSM;
            _player = controller;
        }

        public abstract void Enter();

        protected void SwitchState<T>() where T : PlayerStateBase
        => _fsm.SetState<T>();
    }
}
