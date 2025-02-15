using UnityEngine;

namespace RobloxTest
{
    public class StateMove : StateBase, IUpdateable, IFixedUpdateable
    {
        private readonly Animator _animator;
        private readonly Rigidbody _rb;
        private readonly PlayerController _playerController;
        private float _animationVelocity;
        private float _animationTimer;

        public StateMove(FSM fsm, Animator animator, Rigidbody rb, PlayerController playerController) : base(fsm)
        {
            _animator = animator;
            _rb = rb;
            _playerController = playerController;
        }

        public override void Enter()
        {
            _animationVelocity = 0;
            _animationTimer = 0;
        }

        public void Update()
        {
            if (!_playerController.IsGrounded)
            {
                _fsm.SetState<StateFall>();
            }

            if (_playerController.InputDirection != Vector3.zero)
            {
                if (_animationTimer <= 1)
                {
                    AnimateMove(1);
                    _animationTimer += Time.deltaTime;
                }
            }
            else
            {
                _fsm.SetState<StateIdle>();
            }
        }

        public void FixedUpdate()
        {

            Vector3 moveDirection = _playerController.GetMoveDirection();

            Vector3 targetPosition = _rb.position + moveDirection * _playerController.MoveSpeed * Time.deltaTime;

            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, targetRotation, 360 * Time.deltaTime));
            }

            _rb.MovePosition(targetPosition);

        }

        private void AnimateMove(float targetValue)
        {
            _animationVelocity = Mathf.Lerp(_animationVelocity, targetValue, _animationTimer);
            _animator.SetFloat("Velocity", _animationVelocity);
        }
    }
}
