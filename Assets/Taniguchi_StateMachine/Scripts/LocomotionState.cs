using UnityEngine;
namespace Platformer
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController00 player, Animator animator) : base(player, animator) { }
        public override void OnEnter()
        {
            animator.CrossFade(LocomotionHash, crossFadeDuration);
        }
        public override void FixedUpdate()
        {

        }
    }
}