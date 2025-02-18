namespace RobloxTest
{
    public class PlayerStateJump : PlayerStateBase, IUpdateable
    {
        public PlayerStateJump(PlayerFSM playerFSM, PlayerController controller) : base(playerFSM, controller)
        {
        }

        public override void Enter()
        {
            HandleJump();
        }

        public void Update()
        {
            HandleGrounded();
        }

        private void HandleJump()
        {
            _player.Animator.SetTrigger("Jump");
            _player.CurrentJumpVelocity = _player.InitialJumpVelocity * .5f;
            _player.AppliedJumpVelocity = _player.InitialJumpVelocity * .5f;
        }

        private void HandleGrounded()
        {
            if (_player.CharacterController.isGrounded)
            {
                SwitchState<PlayerStateGrounded>();
            }
        }
    }
}