using UnityEngine;

public class ResetState : EnvironmentInteractionState
{
    public ResetState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        EnvironmentInteractionContext Context = context;
    }

    public override void EnterState() { }
    public override void ExitState() { }
    public override void UpdateState() { }
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Search;
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}






























