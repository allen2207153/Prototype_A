using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;
using UnityEngine.EventSystems;

public class PlayerMovement : BChara
{
    //追加時間：20240723＿チョウハク
    bool _isPushPressed = false;
    bool _pushState = false;
    Transform _interactPoint;
    MovableObject _movableObject;
    PlayerSensor _playerSensor;
    //追加時間：20240820＿チョウハク
    public bool _attackFlag = false;
    private bool _isAttacking = false;


    //追加時間：20240829＿ワンユールン
    [SerializeField] private bool canHoldHand;
    Animator animator;
    private Vector3 moveDirection;
    private Transform platformTransform;
    private Vector3 previousPlatformPosition;
    private bool isOnPlatform;


    //重力の大きさを設定します
    [SerializeField] private float _gravity = -9.8f;

    //移動速度を設定します
    [SerializeField] private float _walkSpeedMax = 10f;
    //移動の加速------------------------------------
    private float _walkSpeedMin = 0f; //移動開始速度
    [SerializeField] private float _walkAddSpeed = 0.4f; //移動加速

    //ジャンプ力を設定します
    [SerializeField] private float _jumpForce = 20.0f;

    //仮想カメラの参照を設定します
    [Header("CinemachineVirtualCamera")]
    public CinemachineVirtualCamera _vCam;

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

    //しゃがみのフラグ
    private bool _crouchFlag = false;  //更新_追加時間：20240726＿八子遥輝

    public bool _grabHandFlag = false; //更新_追加時間：20240807＿ワンユールン

    private PlayerClimbing _playerClimbing;

    [Header("Raycastにより登るの判定位置----デバッグ観測用-----")]
    [SerializeField] private Vector3 _climbVec3;
    private Vector3 _perClimbVec3;
    private bool _isClimbingUp = false;

    [Header("「ジャンプしてぶら下がる」無効の高さ")]
    [SerializeField] private float _invalidClimbHeight = 2f;

    [Header("ぶら下がる位置のＹ軸のオフセット量（プレイヤの身長による調節してください）")]
    [SerializeField] private float _playerHangingOffset_Y = 0.8f;

    [Header("プレイヤ登る時の移動量（登った後固まったら調節してください）")]
    [SerializeField] private float _playerClimbOffset = 0.1f;

    [Header("プレイヤ登る時のスピード")]
    [SerializeField] private float _playerClimbSpeed = 2f;


    //PlayerTriggerAction
    [Header("----デバッグ観測用-----")]
    [SerializeField] private bool _jumpTrigger = false;
    [SerializeField] private bool _hangTrigger = false;
    [SerializeField] private bool _crouchTrigger = false;

    private void OnEnable()
    {
        //イベントを登録
        PlayerEvent.CheckHanging += HandleCheckHanging; //ぶら下がるイベント
        PlayerEvent.CheckCollider += SetTriggerActions;
    }
    private void OnDisable()
    {
        //イベントを解除
        PlayerEvent.CheckHanging -= HandleCheckHanging;
        PlayerEvent.CheckCollider -= SetTriggerActions;
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

        //追加時間：20240723＿チョウハク
        _playerSensor = GetComponent<PlayerSensor>();

        //追加時間：20240814＿ワンユールン
        canHoldHand = GameObject.Find("imouto").GetComponent<FollowPlayer>().canHold;
    }
    private void FixedUpdate()
    {
        _moveCnt++;//増加する
        //_playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
    }

    void Update()
    {
        //追加時間：20240822_チョウハク
        _isAttacking = GameObject.Find("Oniisan").GetComponent<MeleeAttack>()._isAttacking;
        //Debug.Log(_velocity);
        //_moveCntの値を観測するだけ
        _checkMoveCnt = _moveCnt;
        canHoldHand = GameObject.Find("imouto").GetComponent<FollowPlayer>().canHold;

        if (isOnPlatform && platformTransform != null)//更新_追加時間：20240904＿ワンユールン
        {
            // プラットフォームの移動量を計算
            Vector3 platformMovement = platformTransform.position - previousPlatformPosition;

            // プラットフォームの移動量をキャラクターに適用
            _cCtrl.Move(platformMovement);

            // プラットフォームの前回位置を更新
            previousPlatformPosition = platformTransform.position;

            // キャラクターがまだプラットフォーム上にいるかどうかを確認
            if (!_cCtrl.isGrounded)
            {
                isOnPlatform = false;
            }
        }


        Think();
        Move();
        animator.SetFloat("Speed", _movementInput.magnitude * _walkSpeedMax, 0.1f, Time.deltaTime); //追加時間：20240812＿ワンユールン

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
                    //登るの位置は0でなく、登る可能の高さ以上になると登る
                    if (_climbVec3 != Vector3.zero &&
                        _climbVec3.y - _cCtrl.transform.position.y > _invalidClimbHeight &&
                        _hangTrigger == true)
                    {
                        nm = Motion.JumpToHangingTakeOff;
                    }

                    else
                    {
                        nm = Motion.TakeOff;
                    }
                }

                if (!CheckFoot()) { nm = Motion.Fall; }

                //追加時間：20240822_チョウハク
                if (_isAttacking)
                {
                    nm = Motion.Attack;
                }

                //更新_追加時間：20240826＿八子遥輝
                if (_crouchFlag && CheckFoot()) { nm = Motion.Crouching_Enter; } //更新時間：20240807＿ワンユールン
                animator.SetBool("Walk_Bool", false);
                animator.SetBool("Crouch_Bool", false);
                animator.SetFloat("Time", 0.0f); //しゃがみ中にカウントした時間をリセット 

                break;
            case Motion.Walk: //更新_追加時間：20240826＿八子遥輝
                if (_movementInput.x == 0 && _movementInput.y == 0) { nm = Motion.Stand; }
                
                if (_jumpFlag && CheckFoot())
                {
                    //登るの位置は0でなく、登る可能の高さ以上になると登る
                    if (_climbVec3 != Vector3.zero &&
                        _climbVec3.y - _cCtrl.transform.position.y > _invalidClimbHeight &&
                        _hangTrigger == true)
                    {
                        nm = Motion.JumpToHangingTakeOff;
                    }

                    else
                    {
                        nm = Motion.TakeOff;
                    }
                }

                if (!CheckFoot()) { nm = Motion.Fall; }

                //追加時間：20240822_チョウハク
                if (_isAttacking)
                {
                    nm = Motion.Attack;
                }

                animator.SetBool("Walk_Bool", true);

                break;
            case Motion.Jump: //更新_追加時間：20240824＿八子遥輝
                if (_velocity.y < 0) { nm = Motion.Fall; } 
                if (CheckHead()) { _velocity.y = -0.01f; }
                animator.SetBool("Walk_Bool", false);//更新_追加時間：20240829＿ワンユールン
                animator.SetBool("Jump_Bool", true);

                break;
            case Motion.Fall: //更新_追加時間：20240827＿八子遥輝
                if (CheckFoot()) { nm = Motion.Landing; }                                                   
                if (_checkHanging && _hangingFlag == true) { nm = Motion.Hanging_ByCollider; }

                animator.SetBool("Fall_Bool", true);

                break;
            case Motion.Landing: //更新_追加時間：20240827＿八子遥輝
                if (CheckFoot()) { nm = Motion.Stand; }

                animator.SetBool("Jump_Bool", false);
                animator.SetBool("JumpToHanging_Bool", false);
                animator.SetBool("Fall_Bool", false);

                break;
            case Motion.TakeOff:
                if (_moveCnt >= 0) { nm = Motion.Jump; }

                break;
            case Motion.JumpToHangingTakeOff:
                if (_moveCnt >= 0) { nm = Motion.JumpToHanging; }

                break;
            case Motion.JumpToHanging: //更新_追加時間：20240827＿八子遥輝
                if ((_climbVec3 - _cCtrl.transform.position).magnitude < 0.12f) { nm = Motion.Hanging_ByJump; }

                animator.SetBool("JumpToHanging_Bool", true);

                break;
            case Motion.Hanging_ByJump: //更新_追加時間：20240827＿八子遥輝
                if (_jumpFlag) { nm = Motion.ClimbingUp; }
                if (_moveCnt >= 10 && _movementInput.y > 0.2f) { nm = Motion.ClimbingUp; }
                if (_moveCnt >= 10 && _movementInput.y < -0.2f) { nm = Motion.Fall; }

                animator.SetTrigger("Hanging_ByJump_Trigger");

                break;
            case Motion.Hanging_ByCollider:
                //if (CheckHanging() == false) { nm = Motion.Fall; }
                if (_checkHanging == false) { nm = Motion.Fall; }
                if (_moveCnt >= 10 && _movementInput.y < -0.2f) { nm = Motion.Fall; }

                break;
            case Motion.ClimbingUp: //更新_追加時間：20240827＿八子遥輝
                if (!_isClimbingUp) { nm = Motion.Fall; }

                animator.SetTrigger("ClimbingUp_Trigger");

                break;

            case Motion.Crouching_Enter:
                if (_crouchFlag == true && CheckFoot() && _moveCnt >= 150) { nm = Motion.Crouching_Idle; }
                if (!CheckFoot()) { nm = Motion.Fall; }

                animator.SetBool("Crouch_Bool", true);
                animator.SetFloat("Time", _moveCnt);

                break;
            case Motion.Crouching_Idle:
                if (_movementInput.x != 0 || _movementInput.y != 0) { nm = Motion.Crouching_Walk; }
                if (!CheckFoot()) { nm = Motion.Fall; }

                animator.SetTrigger("Crouching_Move_Trigger");
                animator.SetFloat("Time", 0.0f); //Crouching_Enter中にカウントした時間をリセット

                break;
            case Motion.Crouching_Walk: //更新_追加時間：20240807＿ワンユールン -> 更新_追加時間：20240824＿八子遥輝
                if (_movementInput.x == 0 && _movementInput.y == 0) { nm = Motion.Crouching_Idle; }
                if (_crouchTrigger == false) { nm = Motion.Crouching_Exit; }
                if (!CheckFoot()) { nm = Motion.Fall; }

                animator.SetTrigger("Crouching_Move_Trigger");

                break;
            case Motion.Crouching_Exit:
                if (_moveCnt >= 120) { nm = Motion.Stand; }
                if (!CheckFoot()) { nm = Motion.Fall; }

                animator.SetTrigger("Crouching_Exit_Trigger");
                animator.SetFloat("Time", _moveCnt);

                break;
            case Motion.Attack: //追加時間：20240824_チョウハク
                if (!_isAttacking) { nm = Motion.Stand; }

                break;
        }

        UpdataMotion(nm);
    }
    private void Move()
    {
        //_playerClimbing.ClimbDetect(_cCtrl.transform, _movementInput, out _climbVec3);

        //各行動の処理---------------------------------
        switch (_motion)
        {
            case Motion.Stand:
                _playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
                break;
            case Motion.Walk:
                _playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
                HandleWalking();
                //SmoothRotation();
                _perClimbVec3 = _climbVec3;
                break;
            case Motion.Jump:
                if (_moveCnt == 0) { HandleJumping(); }

                HandleWalking();
                break;
            case Motion.Fall:
                HandleGravity();
                break;
            case Motion.Landing:
                _jumpFlag = false; //ジャンプを一回だけに制限する（Landing後ジャンプ可能にする）
                _hangingFlag = true; //ぶら下がるを一回だけに制限する
                break;
            case Motion.TakeOff:

                break;
            case Motion.JumpToHangingTakeOff:
                _jumpFlag = false; //ジャンプ可能にする
                _isClimbingUp = true; //「登る」を可能にする
                _climbVec3.y -= _playerHangingOffset_Y; //キャラのぶら下がるの位置を計算
                break;
            case Motion.JumpToHanging:
                //if (_moveCnt == 0) { _climbVec3.y -= 0.4f; }
                PlayerMoveToTarget(_climbVec3, 8f);
                break;
            case Motion.Hanging_ByJump:
                PlayerMoveToTarget(_climbVec3, 8f);
                break;
            case Motion.Hanging_ByCollider:
                _hangingFlag = false;
                break;
            case Motion.ClimbingUp:
                HandleClimbingUp();
                break;
            case Motion.Crouching_Enter:
                _playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
                break;
            case Motion.Crouching_Idle:
                _playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
                break;
            case Motion.Crouching_Walk: //更新_追加時間：20240827＿八子遥輝
                _playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
           
                // しゃがみ歩き用の一時的な変数を作成
                float crouchWalkSpeedMax = _walkSpeedMax;
                float crouchWalkAddSpeed = _walkAddSpeed;

                // 速度を25%に減少
                _walkSpeedMax *= 0.25f;
                _walkAddSpeed *= 0.25f;

                HandleWalking();

                // Animatorの「Speed」パラメータも25%に減少
                animator.SetFloat("Speed", _movementInput.magnitude * _walkSpeedMax * 0.25f, 0.1f, Time.deltaTime);

                // 元の速度に戻す
                _walkSpeedMax = crouchWalkSpeedMax;
                _walkAddSpeed = crouchWalkAddSpeed;

                //SmoothRotation();
                _perClimbVec3 = _climbVec3;
                break;
            case Motion.Crouching_Exit:
                _crouchFlag = false;
                _playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
                break;
            case Motion.Attack: //追加時間：20240824_チョウハク
                break;
        }

        //重力操作--------------------------------
        switch (_motion)
        {
            //重力を使用されたくないcaseをここに追加
            case Motion.JumpToHangingTakeOff:
                break;
            case Motion.JumpToHanging:
                break;
            case Motion.Hanging_ByJump:
                break;
            case Motion.Hanging_ByCollider:
                break;
            case Motion.ClimbingUp:
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
        //追加＿修正時間：20240825＿ワンユールン
        Vector2 input = _ctx.ReadValue<Vector2>();
        Vector3 moveInput = new Vector3(input.x, 0, input.y);

        //カメラの回転によってキャラクタの向きを調整します
        moveDirection = _vCam.transform.forward * moveInput.z + _vCam.transform.right * moveInput.x;
        moveDirection.y = 0f;
        if (moveDirection.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }


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
        if (_jumpTrigger == true ||
                _hangTrigger == true)
        {
            if (_ctx.phase == InputActionPhase.Started)
            {
                _jumpFlag = true;
            }
        }
    }

    /// <summary>
    /// 移動処理を実行します
    /// </summary>
    private void HandleWalking()
    {
        //20240723＿チョウハク
        Push();
        //移動入力に基づいて方向ベクトルを計算し、正規化します
        Vector3 direction = new Vector3(_movementInput.x, 0f, _movementInput.y).normalized;

        //方向ベクトルの大きさが0.1以上の場合に移動を実行します
        if (direction.magnitude >= 0.1f)
        {

            //前方向と右方向を基に移動方向を計算します
            //修正時間：20240829＿ワンユールン
            Vector3 _moveDirection = moveDirection;

            //追加時間：20240709＿ワンユールン— ->修正_20240711_チンキントウ ->修正20240723_チョウハク
            if (!_pushState)
            {
                targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            }

            //20240723＿チョウハク
            Vector3 playerDeltaMovement = _moveDirection *
           ((_walkSpeedMin += _walkAddSpeed) < _walkSpeedMax ? _walkSpeedMin : _walkSpeedMax) *
           _movementInput.magnitude *
           Time.deltaTime;




            //20240723＿チョウハク
            if (_pushState)
            {
                //animator.ApplyBuiltinRootMotion();
                //animator.MatchTarget(_interactPoint.position, _interactPoint.rotation, AvatarTarget.Root,
                //new MatchTargetWeightMask(Vector3.one, 1f), 0.2f, 0.5f);

                //20240723＿チョウハク
                if (_movableObject)
                {
                    playerDeltaMovement = _cCtrl.transform.InverseTransformDirection(playerDeltaMovement);
                    playerDeltaMovement.x = 0;
                    playerDeltaMovement = _cCtrl.transform.TransformDirection(playerDeltaMovement);
                    playerDeltaMovement.y = 0f;
                    _movableObject.transform.Translate(playerDeltaMovement);
                }
            }

            //移動入力の大きさを基に速度を調整し、プレイヤーを移動させます
            _cCtrl.Move(playerDeltaMovement); //20240801_チョウハク　呼び出し所ここに移動して押す時左右移動を防ぎました

        }
    }
    /// <summary>
    /// ジャンプ処理を実行します
    /// </summary>
    private void HandleJumping()
    {
        _velocity.y = _jumpForce; //ジャンプ実行

        //_velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
    }

    /// <summary>
    /// 重力計算
    /// </summary>
    private void HandleGravity()
    {
        
        switch (_motion)
        {
            case Motion.JumpToHangingTakeOff:
            case Motion.JumpToHanging:
            case Motion.Hanging_ByJump:
            case Motion.Hanging_ByCollider:
            case Motion.ClimbingUp:
                return; // 重力を適用します
        }

        
        if (_cCtrl.isGrounded && (_motion == Motion.Landing || _motion == Motion.Walk))
        {
            _velocity.y = -2f; 
        }
        else
        {
            
            _velocity.y += _gravity * Time.deltaTime;
        }

        
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
    private void SmoothRotation()
    {
        //追加時間：20240709＿ワンユールン
        var rotationSpeed = _rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

    }
    /// <summary>
    /// キャラが指定する位置に移動
    /// </summary>
    /// <param name="targetPos">目標位置</param>
    /// <param name="moveSpeed">移動のスピード</param>
    private void PlayerMoveToTarget(Vector3 targetPos, float moveSpeed)
    {
        // キャラの位置を取得
        Vector3 currentPosition = _cCtrl.transform.position;

        // 移動のベクトルを計算
        Vector3 moveVector = Vector3.MoveTowards(currentPosition, targetPos, moveSpeed * Time.deltaTime);

        // キャラを移動させる
        _cCtrl.Move(moveVector - currentPosition);
    }

    private void HandleClimbingUp()
    {
        Vector3 targetPos = _perClimbVec3 + new Vector3(0f, _playerClimbOffset, 0f);
        PlayerMoveToTarget(targetPos, _playerClimbSpeed);
        if (_cCtrl.transform.position == targetPos) //登るの位置に到達したら
        {
            _isClimbingUp = false; //フラグを変更し、Fallモーションに切り替えを
        }
    }

    //playerTriggerAction-------------------------------------------------------------------------------
    private void SetTriggerActions(Collider colliderEventHandler, bool checkEnterOrExitHandler)
    {
        IPlayerTriggerAction _pta = colliderEventHandler.GetComponent<IPlayerTriggerAction>();
        if (_pta != null)
        {
            if (checkEnterOrExitHandler == true)
            {
                _pta.TriggerAction(this);
            }
            else
            {
                _pta.EndAction(this);
            }
        }
        else
        {
            Debug.Log("Unknown trigger");
        }
    }
    public void SetJumpTrigger(bool jumpTrigger)
    {
        _jumpTrigger = jumpTrigger;
    }
    public void SetHangTrigger(bool hangTrigger)
    {
        _hangTrigger = hangTrigger;
    }
    public void SetCrouchTrigger(bool crouchTrigger)
    {
        _crouchTrigger = crouchTrigger;
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

    //追加時間：20240723＿チョウハク
    public void OnPush(InputAction.CallbackContext _ctx)
    {
        //_isPushPressed = _ctx.ReadValueAsButton();

        if (_ctx.phase == InputActionPhase.Started)
        {
            _isPushPressed = true;
        }
        else if (_ctx.phase == InputActionPhase.Canceled)
        {
            _isPushPressed = false;
        }

    }
    /// <summary>
    /// プレイヤが押す機能の処理
    /// </summary>
    void Push()
    {
        if (_isPushPressed)
        {
            _movableObject = _playerSensor.MovableObjectCheck(_cCtrl.transform);
            if (_movableObject)
            {
                _interactPoint = _movableObject.GetInteractPoint(_cCtrl.transform);
                _pushState = true;
            }
        }
        else if (!_isPushPressed)
        {
            _pushState = false;
        }
        else if (_pushState)
        {
            if (Vector3.Distance(_interactPoint.position, _cCtrl.transform.position) > 1f)
            {
                _pushState = false;
            }
        }
    }

    //更新_追加時間：20240824＿八子遥輝
    public void Crouch(InputAction.CallbackContext _ctx)
    {
        if (_crouchTrigger == true)
        {
            if (_movementInput.x == 0 && _movementInput.y == 0 &&_ctx.phase == InputActionPhase.Started) //キャラクタが動いているときは受け付けない
            {
                _crouchFlag = true;
            }
        }
    }

    //更新_追加時間：20240807＿ワンユールン
    public void GrabHand(InputAction.CallbackContext _ctx)
    {
        if (_ctx.phase == InputActionPhase.Started && canHoldHand == true)
        {
            _grabHandFlag = true;
            Debug.Log("Grab hand success");
        }
        else if (_ctx.phase == InputActionPhase.Canceled)
        {
            _grabHandFlag = false;
        }
    }

    //追加時間：20240820＿チョウハク
    public void Attack(InputAction.CallbackContext _ctx)
    {
        //InputActionPhase.Started;      <-これはGetKeyDown
        //InputActionPhase.Performed;    <-これはGetKey
        //InputActionPhase.Canceled;     <-これはGetKeyUp
        if (_ctx.phase == InputActionPhase.Started)
        {
            Debug.Log("Attack");
            _attackFlag = true;
        }
        if (_ctx.phase == InputActionPhase.Canceled)
        {
            Debug.Log("Stop Attack");
            _attackFlag = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)//更新_追加時間：20240904＿ワンユールン
    {
        // キャラクターが「Platform」タグを持つオブジェクト上にいるかどうかを確認
        if (hit.gameObject.CompareTag("Platform"))
        {
            // プラットフォームのTransformを記録
            if (!isOnPlatform)
            {
                platformTransform = hit.transform;
                previousPlatformPosition = platformTransform.position;
                isOnPlatform = true;
            }
        }
    }
    void OnTriggerExit(Collider other)//更新_追加時間：20240904＿ワンユールン
    {
        // キャラクターがプラットフォームから離れたときに状態をリセット
        if (other.CompareTag("Platform"))
        {
            isOnPlatform = false;
            platformTransform = null;
        }
    }
}


