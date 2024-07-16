using UnityEngine;

public abstract class PlayerState : BaseState<PlayerStateMachine.PlayerStates>
{
    protected PlayerStateContext Context;

    public PlayerState(PlayerStateContext context,PlayerStateMachine.PlayerStates statekey):base(statekey)
    {
        Context = context;
    }
}