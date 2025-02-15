using UnityEngine;

namespace RobloxTest
{
    public class StateIdle : StateBase, IUpdateable
    {
        private readonly PlayerController _playerController;
        private readonly Animator _animator;

        public StateIdle(FSM fsm, Animator animator, PlayerController playerController) : base(fsm)
        {
            _animator = animator;
            _playerController = playerController;
        }

        public override void Enter()
        {
            _animator.SetFloat("Velocity", 0);
        }

        public void Update()
        {
            if (!_playerController.IsGrounded)
            {
                _fsm.SetState<StateFall>();
            }

            if (_playerController.InputDirection != Vector3.zero)
            {
                _fsm.SetState<StateMove>();
            }
        }
    }
}
