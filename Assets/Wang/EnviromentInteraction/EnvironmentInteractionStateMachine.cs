using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;

// 環境とのインタラクションに関する状態管理を行うクラス
public class EnvironmentInteractionStateMachine : StateManager<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{
    // インタラクションの状態を列挙
    public enum EEnvironmentInteractionState
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset
    }

    // コンテキストと必要なコンポーネントの参照
    private EnvironmentInteractionContext _context;

    // シリアライズ可能なフィールド：IKや回転コンストレイント、CharacterController
    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    [SerializeField] private CharacterController _characterController;

    // OnDrawGizmosSelected：Gizmosで近接点を表示する
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_context != null && _context.ClosestPointOnColliderFromShoulder != null)
        {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, .03f);
        }
    }

    // Awake：各コンポーネントの初期化処理
    void Awake()
    {
        ValidateConstraints(); // コンポーネントの検証

        // コンテキストの初期化
        _context = new EnvironmentInteractionContext(_leftIkConstraint, _rightIkConstraint, _leftMultiRotationConstraint,
        _rightMultiRotationConstraint, _characterController, transform.root);

        ConstructEnvironmentDetectionCollider(); // 環境検出用のコライダー構築
        InitializeStates(); // 状態の初期化
    }

    // ValidateConstraints：コンストレイントが正しくアサインされているか検証
    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftIkConstraint, "Left IK constraint is not assigned.");
        Assert.IsNotNull(_rightIkConstraint, "Right IK constraint is not assigned.");
        Assert.IsNotNull(_leftMultiRotationConstraint, "Left multi-rotation constraint is not assigned.");
        Assert.IsNotNull(_rightMultiRotationConstraint, "Right multi-rotation constraint is not assigned.");
        Assert.IsNotNull(_characterController, "CharacterController used to control character is not assigned.");
    }

    // InitializeStates：各インタラクション状態を追加し、初期状態を設定
    private void InitializeStates()
    {
        // StateManagerのStates辞書に状態を追加
        States.Add(EEnvironmentInteractionState.Reset, new ResetState(_context, EEnvironmentInteractionState.Reset));
        States.Add(EEnvironmentInteractionState.Search, new SearchState(_context, EEnvironmentInteractionState.Search));
        States.Add(EEnvironmentInteractionState.Approach, new ApproachState(_context, EEnvironmentInteractionState.Approach));
        States.Add(EEnvironmentInteractionState.Rise, new RiseState(_context, EEnvironmentInteractionState.Rise));
        States.Add(EEnvironmentInteractionState.Touch, new TouchState(_context, EEnvironmentInteractionState.Touch));

        // 初期状態をリセット状態に設定
        currentState = States[EEnvironmentInteractionState.Reset];
    }

    // ConstructEnvironmentDetectionCollider：環境検出用のBoxColliderを構築し、キャラクターの周囲のインタラクション範囲を定義
    private void ConstructEnvironmentDetectionCollider()
    {
        // キャラクターの身長をウィングスパンとして使用
        float wingspan = _characterController.height;

        // BoxColliderを追加し、キャラクターの周囲に配置
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_characterController.center.x, _characterController.center.y + (.25f * wingspan), _characterController.center.z + (.5f * wingspan));
        boxCollider.isTrigger = true; // トリガーとして設定
    }
}