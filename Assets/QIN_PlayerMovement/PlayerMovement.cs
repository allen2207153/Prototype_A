using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : BChara
{
    //追加時間：20240709＿ワンユールン

    Animator animator;


    //重力の大きさを設定します
    [SerializeField] private float _gravity = -9.8f;

    //移動速度を設定します
    [SerializeField] private float _walkSpeedMax = 10f;
    //移動の加速------------------------------------
    private float _walkSpeedMin = 0f;//移動開始速度
    [SerializeField] private float _walkAddSpeed = 0.4f;//移動加速

    //ジャンプ力を設定します
    [SerializeField] private float _jumpForce = 20.0f;

    //仮想カメラの参照を設定します
    [Header("CinemachineVirtualCamera")]
    [SerializeField] private CinemachineVirtualCamera _vCam;

    [Header("キャラの回転スピード")]
    [SerializeField] private float _rotationSpeed = 600f;

    //回転の角度を保存する変数
    private Quaternion targetRotation;

    //ぶら下がり
    private bool _checkHanging = false;
    //ぶら下がるを一回だけに制限する用
    private bool _hangingFlag = true;

    [Header("_moveCntの値を観測用----------------------")]
    [SerializeField] private int _checkMoveCnt;

    //移動入力を保存する変数
    private Vector2 _movementInput = Vector2.zero;
    //重力やジャンプの速度を保存する変数
    private Vector3 _velocity = Vector3.zero;

    //キャラクターコントローラーの参照
    private CharacterController _cCtrl;

    //ジャンプのフラグ
    private bool _jumpFlag = false;

    private PlayerClimbing _playerClimbing;

    [Header("登るの位置----デバッグ観測用-----")]
    [SerializeField] private Vector3 _climbVec3;

    [SerializeField] private float _playerJumpHight = 2f;

    private void OnEnable()
    {
        //イベントを登録
        PlayerEvent.CheckHanging += HandleCheckHanging;//ぶら下がるイベント
    }
    private void OnDisable()
    {
        //イベントを解除
        PlayerEvent.CheckHanging -= HandleCheckHanging;
    }

    void Awake()
    {
        //追加時間：20240709＿ワンユールン
        TryGetComponent(out animator);
        targetRotation = transform.rotation;
    }

    void Start()
    {
        //キャラクターコントローラーを取得します
        _cCtrl = GetComponent<CharacterController>();
        _playerClimbing = new PlayerClimbing();
        _climbVec3 = Vector3.zero;
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


#if DEBUG
        //追加時間：20240713＿八子遥輝
        if (CheckHead())
        {
            Debug.Log("CheckHead");
        }
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

                if (_jumpFlag && CheckFoot())
                {
                    if (_playerClimbing.ClimbDetect(_cCtrl.transform, _movementInput, out _climbVec3))
                    {
                        nm = Motion.JumpToHangingTakeOff;
                    }
                    else
                    {
                        nm = Motion.TakeOff;
                    }
                }

                if (!CheckFoot()) { nm = Motion.Fall; }
                break;
            case Motion.Walk:
                if (_movementInput.x == 0 && _movementInput.y == 0) { nm = Motion.Stand; }

                if (_jumpFlag && CheckFoot())
                {
                    if (_playerClimbing.ClimbDetect(_cCtrl.transform, _movementInput, out _climbVec3))
                    {
                        nm = Motion.JumpToHangingTakeOff;
                    }
                    else
                    {
                        nm = Motion.TakeOff;
                    }
                }        

                if (!CheckFoot()) { nm = Motion.Fall; }
                break;
            case Motion.Jump:
                if (_velocity.y < 0|| CheckHead()) { nm = Motion.Fall; }// 更新_追加時間：20240713＿八子遥輝
                break;
            case Motion.Fall:
                if (CheckFoot()) { nm = Motion.Landing; }
                //if (CheckHanging() && _hangingFlag == true) { nm = Motion.Hanging; }
                if (_checkHanging && _hangingFlag == true) { nm = Motion.Hanging; }
                break;
            case Motion.Landing:
                if (CheckFoot()) { nm = Motion.Stand; }
                break;
            case Motion.TakeOff:
                if (_moveCnt >= 0) { nm = Motion.Jump; }
                break;
            case Motion.JumpToHangingTakeOff:
                if (_moveCnt >= 0) { nm = Motion.JumpToHanging; }
                break;
            case Motion.JumpToHanging:
                if (_moveCnt > 5) { nm=Motion.Fall; }
                break;
            case Motion.Hanging:
                //if (CheckHanging() == false) { nm = Motion.Fall; }
                if (_checkHanging == false) { nm = Motion.Fall; }
                if (_moveCnt >= 10 && _movementInput.y < -0.2f) { nm = Motion.Fall; }
                break;
            case Motion.ClimbingUp:
                break;
        }

        UpdataMotion(nm);
    }
    private void Move()
    {
        Vector3 climbVec3Handeler = default(Vector3);
        //各行動の処理---------------------------------
        switch (_motion)
        {
            case Motion.Stand:
                _playerClimbing.ClimbDetect(_cCtrl.transform, _movementInput, out _climbVec3);
                break;
            case Motion.Walk:
                _playerClimbing.ClimbDetect(_cCtrl.transform, _movementInput, out _climbVec3);
                HandleWalking();
                SmoothRotation();
                climbVec3Handeler = _climbVec3;
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
                _hangingFlag = true;//ぶら下がるを一回だけに制限する
                break;
            case Motion.TakeOff:

                break;
            case Motion.JumpToHangingTakeOff:
                break;
            case Motion.JumpToHanging:
                //Vector3 moveVector = climbVec3Handeler - _cCtrl.transform.position;
                //_cCtrl.Move(moveVector);
                break;
            case Motion.Hanging:
                _hangingFlag = false;
                break;
            case Motion.ClimbingUp:
                break;
        }

        //重力操作--------------------------------
        switch (_motion)
        {
            //重力を使用されたくないcaseをここに追加
            case Motion.Hanging:
                break;
            default:
                //重力処理を実行します
                HandleGravity();
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

            //移動開始速度をリセット
            _walkSpeedMin = 0f;
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


            //追加時間：20240709＿ワンユールン— ->修正_20240711_チンキントウ
            targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _walkSpeedMax = Input.GetKey(KeyCode.LeftShift) ? 10 : 2;
            //移動入力の大きさを基に速度を調整し、プレイヤーを移動させます
            _cCtrl.Move(
                _moveDirection *
                ((_walkSpeedMin += _walkAddSpeed) < _walkSpeedMax ? _walkSpeedMin : _walkSpeedMax) *
                _movementInput.magnitude *
                Time.deltaTime
                );
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

            case Motion.Fall://追加時間:20240713_八子遥輝
                _velocity.y -=1;
                break;

            default:
                //重力を適用します
                _velocity.y += _gravity * Time.deltaTime;
                break;
        }

        //キャラクターを重力に基づいて移動させます
        _cCtrl.Move(_velocity * Time.deltaTime);
    }

    /// <summary>
    /// ぶら下がる判定
    /// </summary>
    /// <returns></returns>
    private void HandleCheckHanging(bool checkHangingEventHandler)
    {
        if (_checkHanging == checkHangingEventHandler) { return; }
        else
        {
            _checkHanging = checkHangingEventHandler;
        }
    }
    /// <summary>
    /// 回転をスムーズにする
    /// </summary>
    void SmoothRotation()
    {
        //追加時間：20240709＿ワンユールン
        var rotationSpeed = _rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
        animator.SetFloat("Speed", _movementInput.magnitude * _walkSpeedMax, 0.1f, Time.deltaTime);
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