using UnityEngine;

namespace RobloxTest
{
    public class PlayerController : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private Transform _cameraTransform;
        public float MoveSpeed;
        public float RotationSpeed;

        [Space(10)]
        [SerializeField] private float _maxJumpHeight;
        [SerializeField] private float _maxJumpTime;
        [SerializeField] private float _gravityGrounded;

        private Vector3 _currentVelocity;
        private Vector3 _appliedVelocity;

        private bool _isJumpPressed;

        private float _gravity;
        private float _initialJumpVelocity;

        private CharacterController _characterController;
        private Animator _animator;
        private PlayerFSM _fsm;
        private Vector2 _inputDirection;
        private PlayerInput _input;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
            _input = new();
            InitializeJumpVariables();
            InitializeFSM();
        }

        public CharacterController CharacterController => _characterController;
        public Animator Animator => _animator;
        public Vector3 CurrentVelocity { get => _currentVelocity; set => _currentVelocity = value; }
        public Vector3 AppliedVelocity { get => _appliedVelocity; set => _appliedVelocity = value; }
        public float MaxJumpHeight => _maxJumpHeight;
        public float MaxJumpTime => _maxJumpTime;
        public float GravityGrounded => _gravityGrounded;
        public float CurrentJumpVelocity { get => _currentVelocity.y; set => _currentVelocity.y = value; }
        public float AppliedJumpVelocity { get => _appliedVelocity.y; set => _appliedVelocity.y = value; }
        public bool IsJumpPressed { get => _isJumpPressed; set => _isJumpPressed = value; }
        public float Gravity => _gravity;
        public float InitialJumpVelocity => _initialJumpVelocity;

        private void OnEnable()
        {
            _input.Enable();

            _input.Player.Move.started += OnMoveInput;
            _input.Player.Move.performed += OnMoveInput;
            _input.Player.Move.canceled += OnMoveInput;

            _input.Player.Jump.started += OnJumpInput;
            _input.Player.Jump.canceled += OnJumpInput;
        }

        private void OnDisable()
        {
            _input.Disable();

            _input.Player.Move.started -= OnMoveInput;
            _input.Player.Move.performed -= OnMoveInput;
            _input.Player.Move.canceled -= OnMoveInput;

            _input.Player.Jump.started -= OnJumpInput;
            _input.Player.Jump.canceled -= OnJumpInput;
        }

        private void Update()
        {
            _fsm.Update();
            _characterController.Move(MoveSpeed * Time.deltaTime * _appliedVelocity);
            HandleGravity();
        }

        private void FixedUpdate()
        {
            _fsm.FixedUpdate();
        }

        private void InitializeJumpVariables()
        {
            float timeToApex = MaxJumpTime / 2;
            _gravity = (-2 * MaxJumpHeight) / Mathf.Pow(timeToApex, 2);
            _initialJumpVelocity = (2 * MaxJumpHeight) / timeToApex;
        }

        private void InitializeFSM()
        {
            _fsm = new();

            PlayerStateIdle playerStateIdle = new(_fsm, this);
            PlayerStateMove playerStateMove = new(_fsm, this);
            PlayerStateJump playerStateJump = new(_fsm, this);

            PlayerStateGrounded playerStateGrounded = new(_fsm, this, playerStateIdle, playerStateMove);

            _fsm.AddState(playerStateGrounded);
            _fsm.AddState(playerStateJump);

            _fsm.SetState<PlayerStateGrounded>();
        }

        private void HandleGravity()
        {
            bool isFalling = _currentVelocity.y <= 0 || !_isJumpPressed;
            float fallMultiplier = 2f;

            if (_characterController.isGrounded)
            {
                _currentVelocity.y = GravityGrounded;
                _appliedVelocity.y = GravityGrounded;
            }
            else if (isFalling)
            {
                float previouslyYVelocity = _currentVelocity.y;
                _currentVelocity.y += (Gravity * fallMultiplier * Time.deltaTime);
                _appliedVelocity.y = (previouslyYVelocity + _currentVelocity.y) * .5f;
            }
            else
            {
                float previousYVelocity = _currentVelocity.y;
                _currentVelocity.y += (Gravity * fallMultiplier * Time.deltaTime);
                _appliedVelocity.y = (previousYVelocity + _currentVelocity.y) * .5f;
            }
        }

        public Vector3 GetMoveInput()
        {
            Vector3 cameraForward = _cameraTransform.forward;
            Vector3 cameraRight = _cameraTransform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;

            return (cameraForward * _inputDirection.y + cameraRight * _inputDirection.x).normalized;
        }

        private void OnMoveInput(UnityEngine.InputSystem.InputAction.CallbackContext context) =>
            _inputDirection = context.ReadValue<Vector2>();


        private void OnJumpInput(UnityEngine.InputSystem.InputAction.CallbackContext context) =>
            _isJumpPressed = context.ReadValueAsButton();
    }
}
