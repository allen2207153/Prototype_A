using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{

    public IdleState(PlayerStateContext context, PlayerStateMachine.PlayerStates estate) : base(context, estate)
    {
        PlayerStateContext Context = context;
    }
    public override void EnterState() { Debug.Log("IdleState開始"); }
    public override void ExitState() { }
    public override void UpdateState() { Debug.Log("IdleState中"); }
    public override PlayerStateMachine.PlayerStates GetNextState()
    {
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
