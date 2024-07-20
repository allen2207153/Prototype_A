using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentInteractionState : BaseState<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
   protected EnvironmentInteractionContext Context;

    public EnvironmentInteractionState(EnvironmentInteractionContext context,EnvironmentInteractionStateMachine.EEnvironmentInteractionState
         stateKey):base(stateKey)
    {
        Context = context;
    }

    private Vector3 GetClosePointOnCollider(Collider intersectingCollider, Vector3 positionToCheck)
    {
        return intersectingCollider.ClosestPoint(positionToCheck);
    }
    protected void StartIkTargetPositionTracking(Collider intersectingCollider)
    {
        
        if(intersectingCollider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
   
            Vector3 closePointFromRoot = GetClosePointOnCollider(intersectingCollider, Context.RootTransform.position);
            Context.SetCurrentSide(closePointFromRoot);
        }
      
    }

    protected void UpdateIkTargetPosition(Collider intersectingCollider)
    {

    }

    protected void ResetIkTargetPositionTracking(Collider intersectingCollider)
    {

    }
}
