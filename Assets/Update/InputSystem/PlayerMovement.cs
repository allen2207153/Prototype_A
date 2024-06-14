using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : BChara
{
    //重力の大きさを設定します
    [SerializeField] private float _gravity = -9.8f;
    //移動速度を設定します
    [SerializeField] private float _walkSpeed = 10f;
    //ジャンプ力を設定します
    [SerializeField] private float _jumpForce = 20.0f;

    [Header("_moveCntの値を観測するだけ")]
    [SerializeField] private int _checkMoveCnt;

    //仮想カメラの参照を設定します
    [Header("CinemachineVirtualCamera")]
    [SerializeField] private CinemachineVirtualCamera _vCam;

    //移動入力を保存する変数
    private Vector2 _movementInput = Vector2.zero;
    //重力やジャンプの速度を保存する変数
    private Vector3 _velocity = Vector3.zero;

    //キャラクターコントローラーの参照
    private CharacterController _cCtrl;

    //ジャンプのフラグ
    private bool _jumpFlag = false;


    void Start()
    {
        //キャラクターコントローラーを取得します
        _cCtrl = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        _moveCnt++;//増加する
    }
    void Update()
    {
        //_moveCntの値を観測するだけ
        _checkMoveCnt = _moveCnt;

        Think();
        Move();

        //重力処理を実行します
        HandleGravity();

#if DEBUG
        if (CheckFoot())
        {
            //Debug.Log("checkfoot");
        }
#endif
    }
    private void Think()
    {
        Motion nm = _motion;

        switch (_motion)
        {
            case Motion.Stand:
                if (_movementInput.x != 0 || _movementInput.y != 0) { nm = Motion.Walk; }
                if (_jumpFlag && CheckFoot()) { nm = Motion.TakeOff; }
                if (!CheckFoot()) { nm = Motion.Fall; }
                break;
            case Motion.Walk:
                if (_movementInput.x == 0 && _movementInput.y == 0) { nm = Motion.Stand; }
                if (_jumpFlag && CheckFoot()) { nm = Motion.TakeOff; }
                if (!CheckFoot()) { nm = Motion.Fall; }
                break;
            case Motion.Jump:
                if (_velocity.y < 0) { nm = Motion.Fall; }
                break;
            case Motion.Fall:
                if (CheckFoot()) { nm = Motion.Landing; }
                break;
            case Motion.Landing:
                if (CheckFoot()) { nm = Motion.Stand; }
                break;
            case Motion.TakeOff:
                if (_moveCnt >= 0) { nm = Motion.Jump; }
                break;
        }

        UpdataMotion(nm);
    }
    private void Move()
    {
        switch (_motion)
        {
            case Motion.Stand:

                break;
            case Motion.Walk:
                HandleWalking();
                break;
            case Motion.Jump:
                if (_moveCnt == 0) { HandleJumping(); }
                HandleWalking();
                break;
            case Motion.Fall:
                HandleWalking();
                break;
            case Motion.Landing:
                _jumpFlag = false;//ジャンプを一回だけに制限する
                break;
            case Motion.TakeOff:

                break;
        }
    }
    /// <summary>
    /// InputSystemのWalk_Action
    /// </summary>
    /// <param name="_ctx"></param>
    public void Walk(InputAction.CallbackContext _ctx)
    {
        //入力のフェーズがPerformedの場合、移動入力を読み取ります
        if (_ctx.phase == InputActionPhase.Performed)
        {
            _movementInput = _ctx.ReadValue<Vector2>();
        }
        //入力のフェーズがCanceledの場合、移動入力をリセットします
        else if (_ctx.phase == InputActionPhase.Canceled)
        {
            _movementInput = Vector2.zero;
        }
    }
    /// <summary>
    /// InputSystemのJump_Action
    /// </summary>
    /// <param name="_ctx">InputSystemの変数</param>
    public void Jump(InputAction.CallbackContext _ctx)
    {
        if (_ctx.phase == InputActionPhase.Started)
        {
            _jumpFlag = true;
        }
    }

    /// <summary>
    /// 移動処理を実行します
    /// </summary>
    private void HandleWalking()
    {
        //移動入力に基づいて方向ベクトルを計算し、正規化します
        Vector3 direction = new Vector3(_movementInput.x, 0f, _movementInput.y).normalized;

        //方向ベクトルの大きさが0.1以上の場合に移動を実行します
        if (direction.magnitude >= 0.1f)
        {
            //Cinemachine仮想カメラのTransformを取得します
            Transform _camTransform = _vCam.transform;
            //カメラの前方向を取得します
            Vector3 _forward = Vector3.Scale(_camTransform.forward, new Vector3(1, 0, 1)).normalized;
            //カメラの右方向を取得します
            Vector3 _right = Vector3.Scale(_camTransform.right, new Vector3(1, 0, 1)).normalized;

            //前方向と右方向を基に移動方向を計算します
            Vector3 _moveDirection = _forward * direction.z + _right * direction.x;
            //移動入力の大きさを基に速度を調整し、プレイヤーを移動させます
            _cCtrl.Move(_moveDirection * _walkSpeed * _movementInput.magnitude * Time.deltaTime);
        }
    }
    /// <summary>
    /// ジャンプ処理を実行します
    /// </summary>
    private void HandleJumping()
    {
        _velocity.y = _jumpForce;//ジャンプ実行

        //_velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
    }

    /// <summary>
    /// 重力計算
    /// </summary>
    private void HandleGravity()
    {
        switch (_motion)
        {
            case Motion.Landing:
                if (_velocity.y < 0)
                {
                    _velocity.y = -2f; //プレイヤーを地面に保つ
                }
                break;
            case Motion.Walk:
                if (_velocity.y < 0)
                {
                    _velocity.y = -2f; //プレイヤーを地面に保つ
                }
                break;
            //重力を適用しないモーションを追加

            default:
                //重力を適用します
                _velocity.y += _gravity * Time.deltaTime;
                break;
        }

        //キャラクターを重力に基づいて移動させます
        _cCtrl.Move(_velocity * Time.deltaTime);
    }

    public void Fire(InputAction.CallbackContext _ctx)
    {
        //InputActionPhase.Started;      <-これはGetKeyDown
        //InputActionPhase.Performed;    <-これはGetKey
        //InputActionPhase.Canceled;     <-これはGetKeyUp
        if (_ctx.phase == InputActionPhase.Started)
        {
            Debug.Log("Fire!");
        }
    }
}