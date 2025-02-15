using UnityEngine;

namespace RobloxTest
{
    public class StateFall : StateBase, IFixedUpdateable, IExitable
    {
        private readonly Animator _animator;
        private readonly Rigidbody _rb;
        private readonly PlayerController _playerController;
        private float _verticalVelocity;

        public StateFall(
            FSM fsm,
            Animator animator,
            Rigidbody rb,
            PlayerController playerController
            ) : base(fsm)
        {
            _animator = animator;
            _rb = rb;
            _playerController = playerController;
        }

        public override void Enter()
        {
            _verticalVelocity = 0;
            _animator.SetBool("IsFalling", true);
        }

        public void FixedUpdate()
        {
            if (!_playerController.IsGrounded)
            {
                Vector3 moveDirection = _playerController.GetMoveDirection();
                _verticalVelocity += Physics.gravity.y * Time.fixedDeltaTime;

                Vector3 newPosition = _rb.position
                    + new Vector3(
                        moveDirection.x
                    , _verticalVelocity
                    , moveDirection.z)
                    * Time.fixedDeltaTime * _playerController.FallingSpeed;

                _rb.MovePosition(newPosition);

                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, targetRotation, 360 * Time.deltaTime));
                }
            }
            else
            {
                _fsm.SetState<StateIdle>();
            }
        }

        public void Exit() => _animator.SetBool("IsFalling", false);
    }
}
