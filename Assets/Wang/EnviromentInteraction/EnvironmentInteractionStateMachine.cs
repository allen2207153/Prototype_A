using UnityEditor.VersionControl;
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
    [SerializeField] private CharacterController _characterController;

    void Awake()
    {
           ValidDateConstraints();
        _context = new EnvironmentInteractionContext(_leftIkConstraint,_rightIkConstraint,_leftMultiRotationConstraint,
        _rightMultiRotationConstraint,_characterController, transform.root);
        InitializeStates();
        ConstructEnvironmentDetctionCollider();
    }
    private void ValidDateConstraints()
    {
        Assert.IsNotNull(_leftIkConstraint, "Left IK constraint is not assigned");
        Assert.IsNotNull(_rightIkConstraint, "Right IK constraint is not assigned");
        Assert.IsNotNull(_leftMultiRotationConstraint, "Left multi-rotation constraint is not assigned");
        Assert.IsNotNull(_rightMultiRotationConstraint, "Right multi-rotation constraint is not assigned");
        Assert.IsNotNull(_characterController, "Character controller constraint is not assigned");

    }

    private void InitializeStates()
    {
        States.Add(EEnvironmentInteractionState.Reset, new ResetState(_context,EEnvironmentInteractionState.Reset));//Reset
        States.Add(EEnvironmentInteractionState.Search, new SearchState(_context, EEnvironmentInteractionState.Search));//Search
        States.Add(EEnvironmentInteractionState.Approach, new ApproachState(_context, EEnvironmentInteractionState.Approach));//Approach
        States.Add(EEnvironmentInteractionState.Rise, new RiseState(_context, EEnvironmentInteractionState.Rise));//Rise
        States.Add(EEnvironmentInteractionState.Touch, new TouchState(_context, EEnvironmentInteractionState.Touch));//Touch
        currentState = States[EEnvironmentInteractionState.Reset];
    }

    private void ConstructEnvironmentDetctionCollider()
    {
        float wingspan = _characterController.height;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);   
        boxCollider.center = new Vector3(_characterController.center.x, _characterController.center.y + (0.3f*wingspan), _characterController.center.z + (0.1f * wingspan));
        boxCollider.isTrigger = true;
    }
}
