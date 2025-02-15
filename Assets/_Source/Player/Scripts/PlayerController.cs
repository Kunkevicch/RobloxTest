using UnityEngine;

namespace RobloxTest
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _fallingSpeed;

        [Space(10)]
        [SerializeField] private Transform _cameraTransform;

        [SerializeField] private Transform _checkGround;
        [SerializeField] private LayerMask _groundLayer;

        private Rigidbody _rb;
        private Animator _animator;
        private FSM _fsm;
        private bool _isGrounded;
        private CapsuleCollider _collider;
        private Vector2 _inputDirection;

        private const float CHECK_GROUND_RADIUS = 0.1f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            InitialzeFSM();
        }

        public Vector3 InputDirection => new Vector3(_inputDirection.x, 0, _inputDirection.y);
        public float MoveSpeed => _moveSpeed;
        public float JumpHeight => _jumpHeight;
        public float JumpSpeed => _jumpSpeed;
        public float FallingSpeed => _fallingSpeed;
        public bool IsGrounded => CheckGround();

        private void Update()
        {
            _inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _fsm.SetState<StateJump>();
            }

            _fsm.Update();
        }

        private void FixedUpdate()
        {
            _isGrounded = CheckGround();
            _fsm.FixedUpdate();
        }

        private void InitialzeFSM()
        {
            _fsm = new();

            StateIdle stateIdle = new(_fsm, _animator, this);
            StateMove stateMove = new(_fsm, _animator, _rb, this);
            StateJump stateJump = new(_fsm, _animator, _rb, this);
            StateFall stateFall = new(_fsm, _animator, _rb, this);

            _fsm.AddState(stateIdle);
            _fsm.AddState(stateMove);
            _fsm.AddState(stateJump);
            _fsm.AddState(stateFall);
            _fsm.SetState<StateIdle>();
        }

        private bool CheckGround() => Physics.CheckSphere(_checkGround.position, CHECK_GROUND_RADIUS, _groundLayer);

        public Vector3 GetMoveDirection()
        {
            Vector3 cameraForward = _cameraTransform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 moveDirection = cameraForward * InputDirection.z + _cameraTransform.right * InputDirection.x;
            moveDirection.Normalize();

            return moveDirection;
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_checkGround.position, CHECK_GROUND_RADIUS);
        }
    }
}
