using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer{
    public interface IState
    {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }
    public abstract class BaseState : IState
    {
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
