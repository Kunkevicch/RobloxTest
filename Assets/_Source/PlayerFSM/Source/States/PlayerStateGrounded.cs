using UnityEngine;

namespace RobloxTest
{
    public class PlayerStateGrounded : PlayerStateSuperBase
    {
        public PlayerStateGrounded(
            PlayerFSM playerFSM
            , PlayerController controller
            , PlayerStateIdle idleState
            , PlayerStateMove moveState
            ) : base(playerFSM, controller)
        {
            AddState(idleState);
            AddState(moveState);
        }

        public override void Enter()
        {
            _player.CurrentJumpVelocity = _player.GravityGrounded;
            _player.AppliedJumpVelocity = _player.GravityGrounded;
            if (_player.GetMoveInput() == Vector3.zero)
            {
                SetSubState<PlayerStateIdle>();
            }
            else
            {
                SetSubState<PlayerStateIdle>();
            }
        }

        public override void Update()
        {
            base.Update();
            HandleStateSwitch();
        }

        private void HandleStateSwitch()
        {
            if (!_player.CharacterController.isGrounded)
            {
                SwitchState<PlayerStateFall>();
                return;
            }

            if (_player.IsJumpPressed == true)
            {
                SwitchState<PlayerStateJump>();
            }
            else if (_player.GetMoveInput() == Vector3.zero)
            {
                SetSubState<PlayerStateIdle>();
            }
            else
            {
                SetSubState<PlayerStateMove>();
            }
        }
    }
}
