using UnityEngine;
namespace Platformer
{
    public abstract class BaseState : MyState
    {
        protected readonly PlayerController00 player;
        protected readonly Animator animator;

        protected static readonly int LocomotionHash = Animator.StringToHash(name: "Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash(name: "Jump");

        protected const float crossFadeDuration = 0.1f;

        protected BaseState(PlayerController00 player,Animator animator)
        {
            this.player = player;
            this.animator = animator;
        }
        public virtual void OnEnter()
        {

        }
        public virtual void Update()
        {

        }
        public virtual void FixedUpdate()
        {

        }
        public virtual void OnExit()
        {

        }
    }
}

