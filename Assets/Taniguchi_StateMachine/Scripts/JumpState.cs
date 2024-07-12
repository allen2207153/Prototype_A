using UnityEngine;
namespace Platformer
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController00 player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(JumpHash, crossFadeDuration);
        }
        public override void FixedUpdate()
        {
        }
    }
}