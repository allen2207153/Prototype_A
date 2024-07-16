using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateManager<PlayerStateMachine.PlayerStates>
{
    public enum PlayerStates
    {
        Idle,
        Walk,
        Run,
        Jump,
        Dead
    }
    private PlayerStateContext _context;

    void Awake()
    {
        _context = new PlayerStateContext();
        InitializeStates();
    }
    private void InitializeStates()
    {
        States.Add(PlayerStates.Idle, new IdleState(_context, PlayerStates.Idle));
        currentState = States[PlayerStates.Idle];
    }
}
