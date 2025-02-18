using UnityEngine;

namespace RobloxTest
{
    public class PlayerStateIdle : PlayerStateBase
    {
        public PlayerStateIdle(PlayerFSM playerFSM, PlayerController controller) : base(playerFSM, controller)
        {
        }

        public override void Enter()
        {
            _player.Animator.SetFloat("Velocity", 0);
            _player.AppliedVelocity = new Vector3(0, _player.GravityGrounded, 0);
        }
    }
}
