using UnityEngine;

namespace RobloxTest
{
    public class PlayerStateMove : PlayerStateBase, IUpdateable
    {
        public PlayerStateMove(PlayerFSM playerFSM, PlayerController controller) : base(playerFSM, controller)
        {
        }

        public override void Enter()
        {
            _player.Animator.SetFloat("Velocity", 1);
        }

        public void Update()
        {
            Vector3 moveInput = _player.GetMoveInput();
            _player.AppliedVelocity = new Vector3(moveInput.x, _player.AppliedVelocity.y, moveInput.z);
            if (moveInput != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveInput);
                _player.transform.rotation = Quaternion.Slerp(
                    _player.transform.rotation,
                    targetRotation,
                    _player.RotationSpeed * Time.deltaTime);
            }
        }
    }
}
