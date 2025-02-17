using UnityEngine;

namespace RobloxTest
{
    public class PlayerStateFall : PlayerStateBase, IUpdateable, IExitable
    {
        public PlayerStateFall(PlayerFSM playerFSM, PlayerController controller) : base(playerFSM, controller)
        {
        }

        public override void Enter()
        {
            _player.Animator.SetBool("IsFalling", true);
        }

        public void Update()
        {
            if (_player.CharacterController.isGrounded)
            {
                SwitchState<PlayerStateGrounded>();
            }
            else
            {
                float previouslyYVelocity = _player.CurrentJumpVelocity;
                _player.CurrentJumpVelocity = _player.CurrentJumpVelocity + (_player.Gravity * 2f * Time.deltaTime);
                _player.AppliedJumpVelocity = (previouslyYVelocity + _player.CurrentJumpVelocity) * .5f;
            }
        }

        public void Exit()
        {
            _player.Animator.SetBool("IsFalling", false);
        }

    }
}
