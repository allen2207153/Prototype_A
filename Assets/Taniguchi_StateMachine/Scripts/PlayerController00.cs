using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Platformer
{
    public class PlayerController00 : MonoBehaviour
    {
        StateMachine stateMachine;
        Animator animator;
        void Awake()
        {
            stateMachine = new StateMachine();
            var locomotionState = new LocomotionState(this, animator);
            var jumpState = new JumpState(this, animator);

            stateMachine.SetState(locomotionState);
        }
        void At(MyState from, MyState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(MyState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
        void OnEnable()
        {
            
        }
        void OnDisable()
        {
            
        }
        void Update()
        {
            stateMachine.Update();
        }
        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }
    }
}

