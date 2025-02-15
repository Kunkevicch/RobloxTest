using UnityEngine;

namespace RobloxTest
{
    public class StateJump : StateBase, IFixedUpdateable, IExitable
    {
        private readonly Animator _animator;
        private readonly Rigidbody _rb;
        private readonly PlayerController _playerController;
        private bool _isJumping;
        private float _startY;
        private float _jumpHeight;
        private float _verticalVelocity;

        public StateJump(
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
            _verticalVelocity = _playerController.JumpHeight;
            _startY = _rb.position.y;
            _jumpHeight = _playerController.JumpHeight;
            _animator.SetTrigger("Jump");
            _animator.ResetTrigger("Jump");
            _animator.SetBool("IsFalling", true);
            _isJumping = true;
        }

        public void FixedUpdate()
        {
            if (_isJumping)
            {
                Vector3 moveDirection = _playerController.GetMoveDirection();
                _verticalVelocity += Physics.gravity.y * Time.fixedDeltaTime;
                if (_rb.position.y >= _startY + _jumpHeight)
                {
                    _fsm.SetState<StateFall>();
                }
                else
                {
                    Vector3 newPosition = _rb.position
                        + new Vector3(moveDirection.x * _playerController.MoveSpeed
                        , _verticalVelocity * _playerController.JumpSpeed
                        , moveDirection.z * _playerController.MoveSpeed) * Time.fixedDeltaTime;
                    _rb.MovePosition(newPosition);
                    if (moveDirection != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                        _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, targetRotation, 360 * Time.fixedDeltaTime));
                    }
                }
            }
        }

        public void Exit()
        {
            _isJumping = false;
        }
    }
}
