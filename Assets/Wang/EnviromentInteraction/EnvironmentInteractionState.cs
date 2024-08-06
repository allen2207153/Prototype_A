using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentInteractionState : BaseState<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
   protected EnvironmentInteractionContext Context;
    private float _movingAwayOffset = .05f;
    bool _shouldReset;

    public EnvironmentInteractionState(EnvironmentInteractionContext context,EnvironmentInteractionStateMachine.EEnvironmentInteractionState
         stateKey):base(stateKey)
    {
        Context = context;
    }

    private Vector3 GetClosestPointOnCollider(Collider intersectingCollider, Vector3 positionToCheck)
    {
        return intersectingCollider.ClosestPoint(positionToCheck);
    }

    protected void StartIkTargetPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider.gameObject.layer == LayerMask.NameToLayer("Interactable") && Context.CurrentIntersectingCollider == null)
        {
            Context.CurrentIntersectingCollider = intersectingCollider;
            Vector3 closestPointFromRoot = GetClosestPointOnCollider(intersectingCollider, Context.RootTransform.position);
            Context.SetCurrentSide(closestPointFromRoot);

            SetIkTargetPosition();
        }
    }

    protected void UpdateIKTargetPosition(Collider intersectingCollider)
    {
        if (intersectingCollider == Context.CurrentIntersectingCollider)
        {
            SetIkTargetPosition();
        }
    }

    protected void ResetIkTargetPositionTracking(Collider intersectingCollider)
    {
        if (intersectingCollider == Context.CurrentIntersectingCollider)
        {
            Context.CurrentIntersectingCollider = null;
            Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        }
    }

    private void SetIkTargetPosition()
    {
        Context.ClosestPointOnColliderFromShoulder = GetClosestPointOnCollider(Context.CurrentIntersectingCollider,
        new Vector3(Context.CurrentShoulderTransform.position.x, Context.CharacterShoulderHeight, Context.CurrentShoulderTransform.position.z));

        Vector3 rayDirection = Context.CurrentShoulderTransform.position - Context.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedRayDirection = rayDirection.normalized;
        float offsetDistance = .05f;
        Vector3 offset = normalizedRayDirection * offsetDistance;

        Vector3 offsetPosition = Context.ClosestPointOnColliderFromShoulder + offset;
        Context.CurrentIkTargetTransform.position = offsetPosition;
    }
}
