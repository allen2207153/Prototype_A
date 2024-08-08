
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;
public class EnvironmentInteractionStateMachine : StateManager<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
   public enum EEnvironmentInteractionState
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset
    }
    private EnvironmentInteractionContext _context;

    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    [SerializeField]private CharacterController _characterController;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_context != null && _context.ClosestPointOnColliderFromShoulder != null)
        {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, .03f);
        }
    }

    void Awake()
    {
        ValidateConstraints();

        _context = new EnvironmentInteractionContext(_leftIkConstraint, _rightIkConstraint, _leftMultiRotationConstraint,
        _rightMultiRotationConstraint,_characterController, transform.root);

        ConstructEnvironmentDetectionCollider();
        InitializeStates();
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftIkConstraint, "Left IK constraint is not assigned.");
        Assert.IsNotNull(_rightIkConstraint, "Right IK constraint is not assigned.");
        Assert.IsNotNull(_leftMultiRotationConstraint, "Left multi-rotation constraint is not assigned.");
        Assert.IsNotNull(_rightMultiRotationConstraint, "Right multi-rotation constraint is not assigned.");
        Assert.IsNotNull(_characterController, "CharacterController used to control character is not assigned.");
      
    }

    private void InitializeStates()
    {
        // Add States to inherited StateManager "States" dictionary and Set Initial State
        States.Add(EEnvironmentInteractionState.Reset, new ResetState(_context, EEnvironmentInteractionState.Reset));
        States.Add(EEnvironmentInteractionState.Search, new SearchState(_context, EEnvironmentInteractionState.Search));
        States.Add(EEnvironmentInteractionState.Approach, new ApproachState(_context, EEnvironmentInteractionState.Approach));
        States.Add(EEnvironmentInteractionState.Rise, new RiseState(_context, EEnvironmentInteractionState.Rise));
        States.Add(EEnvironmentInteractionState.Touch, new TouchState(_context, EEnvironmentInteractionState.Touch));
        currentState = States[EEnvironmentInteractionState.Reset];
    }

    private void ConstructEnvironmentDetectionCollider()
    {
        float wingspan = _characterController.height;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_characterController.center.x, _characterController.center.y + (.25f * wingspan), _characterController.center.z + (.5f * wingspan));
        boxCollider.isTrigger = true;
    }
}
