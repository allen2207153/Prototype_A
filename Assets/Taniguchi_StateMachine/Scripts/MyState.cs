using UnityEngine;

namespace Platformer
{
    public interface MyState
    {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }
    
}
