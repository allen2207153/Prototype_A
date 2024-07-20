using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ResetState : EnvironmentInteractionState
{
    public ResetState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)

    {
        EnvironmentInteractionContext Context = context;
    }
    public override void EnterState()
    {
        Debug.Log("Enter reset state");
    }
    public override void ExitState()
    {
    }
    public override void UpdateState()
    {
        Debug.Log("Updating reset state");

    }
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Search;
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other)
    {
      
    }
    public override void OnTriggerStay(Collider other)
    {

    }
    public override void OnTriggerExit(Collider other)
    {

    }
}
