using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : BChara
{
    // 重力の大きさを設定します
    [SerializeField] private float _gravity = -9.8f;
    // 移動速度を設定します
    [SerializeField] private float _moveSpeed = 10f;
    // ジャンプ力を設定します
    [SerializeField] private float _jumpForce = 10.0f;

    // 仮想カメラの参照を設定します
    [Header("CinemachineVirtualCamera")]
    [SerializeField] private CinemachineVirtualCamera _vCam;

    // 移動入力を保存する変数
    private Vector2 _movementInput = Vector2.zero;
    // 重力やジャンプの速度を保存する変数
    private Vector3 _velocity = Vector3.zero;

    // キャラクターコントローラーの参照
    private CharacterController _cCtrl;

    void Start()
    {
        // キャラクターコントローラーを取得します
        _cCtrl = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 移動処理を実行します
        HandleMovement();
        // 重力処理を実行します
        HandleGravity();
        if (CheckFoot())
        {
            Debug.Log("checkfoot");
        }
    }

    public void Move(InputAction.CallbackContext _ctx)
    {
        // 入力のフェーズがPerformedの場合、移動入力を読み取ります
        if (_ctx.phase == InputActionPhase.Performed)
        {
            _movementInput = _ctx.ReadValue<Vector2>();
        }
        // 入力のフェーズがCanceledの場合、移動入力をリセットします
        else if (_ctx.phase == InputActionPhase.Canceled)
        {
            _movementInput = Vector2.zero;
        }
    }

    public void Jump(InputAction.CallbackContext _ctx)
    {
        // 入力のフェーズがStartedであり、キャラクターが地面に接している場合、ジャンプ力を適用します
        if (_ctx.phase == InputActionPhase.Started && CheckFoot())
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
        }
    }

    private void HandleMovement()
    {
        // 移動入力に基づいて方向ベクトルを計算し、正規化します
        Vector3 direction = new Vector3(_movementInput.x, 0f, _movementInput.y).normalized;

        // 方向ベクトルの大きさが0.1以上の場合に移動を実行します
        if (direction.magnitude >= 0.1f)
        {
            // Cinemachine仮想カメラのTransformを取得します
            Transform _camTransform = _vCam.transform;
            // カメラの前方向を取得します
            Vector3 _forward = Vector3.Scale(_camTransform.forward, new Vector3(1, 0, 1)).normalized;
            // カメラの右方向を取得します
            Vector3 _right = Vector3.Scale(_camTransform.right, new Vector3(1, 0, 1)).normalized;

            // 前方向と右方向を基に移動方向を計算します
            Vector3 _moveDirection = _forward * direction.z + _right * direction.x;
            // 移動入力の大きさを基に速度を調整し、プレイヤーを移動させます
            _cCtrl.Move(_moveDirection * _moveSpeed * _movementInput.magnitude * Time.deltaTime);
        }
    }

    private void HandleGravity()
    {
        // キャラクターが地面に接している場合
        if (CheckFoot())
        {
            // キャラクターが下降中であれば、Y軸速度をリセットします
            if (_velocity.y < 0)
            {
                _velocity.y = -2f; // プレイヤーを地面に保つ
            }
        }
        // キャラクターが空中にいる場合
        else
        {
            // 重力を適用します
            _velocity.y += _gravity * Time.deltaTime;
        }

        // キャラクターを重力に基づいて移動させます
        _cCtrl.Move(_velocity * Time.deltaTime);
    }
}