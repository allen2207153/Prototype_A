using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

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

    //追加時間：20240917＿チンキントウ_pauseMenuのフラグ追加
    [NonSerialized] public bool IsPlayerPaused = false;

    //重力の大きさを設定します
    [SerializeField] private float _gravity = -0.01f; // 最大重力加速度 --------------TODO:調整
    [SerializeField] private float _maxGravity = -10f;//追加時間：20241020＿ワンユールン
    //ジャンプ変数
    float initialJumpVelocity;
    [SerializeField] float maxJumpHeight = 1.0f;
    [SerializeField] float maxJumpTime = 0.5f;

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

    //回転を制限するフラグ
    private bool _canRotate = true;

    //ぶら下がり中に回転したかどうかを記録
    private bool _hasRotatedWhileHanging = false;

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
    [SerializeField] private bool _interactionTrigger = false;

    public InputAction pushPullAction; // 前後方向の入力アクション
    public float interactionDistance = 1.5f; // 交互できる最大距離

    private void OnEnable()
    {
        //イベントを登録
        PlayerEvent.CheckHanging += HandleCheckHanging; //ぶら下がるイベント
        PlayerEvent.CheckCollider += SetTriggerActions;
        pushPullAction.Enable();
    }
    private void OnDisable()
    {
        //イベントを解除
        PlayerEvent.CheckHanging -= HandleCheckHanging;
        PlayerEvent.CheckCollider -= SetTriggerActions;
        pushPullAction.Disable();
    }

    void Awake()
    {
        //追加時間：20240709＿ワンユールン
        TryGetComponent(out animator);
        targetRotation = transform.rotation;
        setJumpVariables();
    }

    void Start()
    {
        //キャラクターコントローラーを取得します
        _cCtrl = GetComponent<CharacterController>();
        _playerClimbing = new PlayerClimbing();
        _climbVec3 = Vector3.zero;

        //Collider enemyCollider = GameObject.FindWithTag("Enemy").GetComponent<CapsuleCollider>(); // 敵のCollider

        // CharacterControllerと敵のColliderの衝突を無視
        //Physics.IgnoreCollision(_cCtrl, enemyCollider);

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
        Gravity();
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
        //  Debug.Log(_velocity.y);
        //Debug.Log(_crouchFlag);
        Think();
        Move();
        //Debug.Log(CheckFoot());
        animator.SetFloat("Speed", _movementInput.magnitude * _walkSpeedMax, 0.1f, Time.deltaTime); //追加時間：20240812＿ワンユールン
        if (animator.GetFloat("Speed") < 0.05)
        {
            animator.SetFloat("Speed", 0);
        }


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

                if (_crouchFlag && CheckFoot()) { nm = Motion.Crouching_Enter; } //更新時間：20240807＿ワンユールン
                animator.SetBool("Walk_Bool", false);
                animator.SetBool("Crouch_Bool", false);
                animator.SetBool("ClimbingUp_Bool", false); //更新_追加時間：20240915＿八子遥輝

                animator.SetFloat("Time", 0.0f); //しゃがみ中や登り中にカウントした時間をリセット 

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
                animator.SetBool("Walk_Bool", false); //更新_追加時間：20240829＿ワンユールン
                animator.SetBool("JumpToHanging_Bool", true);

                break;
            case Motion.Fall: //更新_追加時間：20240915＿八子遥輝
                if (CheckFoot()) { nm = Motion.Landing; }
                if (_checkHanging && _hangingFlag == true) { nm = Motion.Hanging_ByCollider; }

                animator.SetBool("Fall_Bool", true);//TODO:Fallアニメはバッグがあります(重要)----------------------------
                animator.SetBool("Landing_Bool", false);
                animator.SetBool("JumpToHanging_Bool", false);
                animator.SetBool("Hanging_ByJump_Bool", false);

                break;
            case Motion.Landing: //更新_追加時間：20240915＿八子遥輝
                nm = Motion.Stand;

                animator.SetBool("Landing_Bool", true);
                animator.SetBool("Fall_Bool", false);
                animator.SetBool("Jump_Bool", false);
                animator.SetBool("JumpToHanging_Bool", false);

                break;
            case Motion.TakeOff: //更新_追加時間：20240915＿八子遥輝
                if (_moveCnt >= 0) { nm = Motion.Jump; }

                animator.SetBool("Landing_Bool", false);

                break;
            case Motion.JumpToHangingTakeOff: //更新_追加時間：20240915＿八子遥輝
                if (_moveCnt >= 0) { nm = Motion.JumpToHanging; }

                animator.SetBool("Landing_Bool", false);

                break;
            case Motion.JumpToHanging: //更新_追加時間：20241021_ワンユールン

                nm = Motion.Hanging_ByJump;
                _canRotate = true; // JumpToHangingが終わったら回転を有効化 
                                   //_velocity.y = -1; // 垂直方向の速度をリセット


                animator.SetBool("JumpToHanging_Bool", true);

                break;
            case Motion.Hanging_ByJump: //更新_追加時間：20241021_ワンユールン
                //if (_jumpFlag) { nm = Motion.ClimbingUp; }
                if (_moveCnt >= 20) { nm = Motion.ClimbingUp; } // 硬直を追加しバグを起こりにくくする
                //if (_movementInput.y < -0.2f && _hasRotatedWhileHanging)
                //{
                //    nm = Motion.Fall;
                //    _canRotate = true; // Hanging_ByJumpが終わったら回転を有効化
                //}
                animator.SetBool("ClimbingUp_Bool", true);

                break;
            case Motion.Hanging_ByCollider://更新_追加時間：20241021_ワンユールン
                ////if (CheckHanging() == false) { nm = Motion.Fall; }
                //if (_checkHanging == false) { nm = Motion.Fall; }
                //if (_moveCnt >= 10 && _movementInput.y < -0.2f) { nm = Motion.Fall; }

                break;
            case Motion.ClimbingUp: //更新_追加時間：20241021_ワンユールン
                //_cCtrl.enabled = false;
                if (_moveCnt >= 25)
                {
                    Debug.Log(_moveCnt);
                    _cCtrl.enabled = true;
                    nm = Motion.Landing;
                    _canRotate = true; // ClimbingUpが終わったら回転を有効化
                }
                animator.SetBool("JumpToHanging_Bool", false);
                animator.SetBool("ClimbingUp_Bool", false);

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
                _canRotate = true;
                _perClimbVec3 = _climbVec3;
                break;
            case Motion.Walk:
                _playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
                HandleWalking();
                //SmoothRotation();
                _perClimbVec3 = _climbVec3;
                break;
            case Motion.Jump:
                _playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
                if (_moveCnt == 0) { HandleJumping(); }
                HandleWalking();
                break;
            case Motion.Fall:
                HandleWalking();
                break;
            case Motion.Landing:
                _jumpFlag = false; //ジャンプを一回だけに制限する（Landing後ジャンプ可能にする）
                _hangingFlag = true; //ぶら下がるを一回だけに制限する
                break;
            case Motion.TakeOff:

                break;
            case Motion.JumpToHangingTakeOff:
                _jumpFlag = false; //ジャンプ可能にする
                _canRotate = false;
                _climbVec3.y -= _playerHangingOffset_Y; //キャラのぶら下がるの位置を計算
                break;
            case Motion.JumpToHanging: //更新_追加時間：20241021_ワンユールン
                //if (_moveCnt == 0) { _climbVec3.y -= 0.4f; }
                _canRotate = false; // 回転を無効化
                //_hasRotatedWhileHanging = false; // リセット
                _movementInput = Vector2.zero; // JumpToHanging中は移動入力を無視
                                                FaceTowardsWall(_climbVec3);
                PlayerMoveToTarget(_climbVec3, 8f);
                break;
            case Motion.Hanging_ByJump: //更新_追加時間：20241021_ワンユールン
                _canRotate = false;
                if (_movementInput.y < -0.2f && !_hasRotatedWhileHanging)
                {
                  // 後ろに入力されたら回転を一時的に有効化
                    transform.rotation = Quaternion.LookRotation(-transform.forward); // 即座に180度回転
                    _hasRotatedWhileHanging = true; // 回転したことを記録
                }
             
                // FaceTowardsWall(_climbVec3);
                PlayerMoveToTarget(_climbVec3, 8f);
                break;
            case Motion.Hanging_ByCollider:
                _hangingFlag = false;
                break;
            case Motion.ClimbingUp: //追加時間：20240915＿八子遥輝
                _canRotate = false; // 回転を無効化
                _movementInput = Vector2.zero; // ClimbingUp中は移動入力を無視
                HandleClimbingUp();
                break;
            case Motion.Crouching_Enter://更新_追加時間：20241021_ワンユールン
                //_playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
                _cCtrl.height = 0.5f; // しゃがみ時のキャラクター高さ
                _cCtrl.center = new Vector3(0, 0.57f, 0); // 中心を低く設定

                break;
            case Motion.Crouching_Idle:
               // _playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
                break;
            case Motion.Crouching_Walk: //更新_追加時間：20240827＿八子遥輝
                //_playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);

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
               // _perClimbVec3 = _climbVec3;
                break;
            case Motion.Crouching_Exit://更新_追加時間：20241021_ワンユールン
                _crouchFlag = false;
               // _playerClimbing.ClimbDetect(_cCtrl.transform, _cCtrl.transform.forward, out _climbVec3);
                _cCtrl.height = 1.4f; // 元のキャラクター高さに戻す
                _cCtrl.center = new Vector3(0, 0.72f, 0); // 中心を元に戻す
                break;
            case Motion.Attack: //追加時間：20240824_チョウハク
                break;
        }


    }
    /// <summary>
    /// 重力操作
    /// </summary>
    private void Gravity()
    {
        //重力操作--------------------------------
        switch (_motion)
        {
            ////重力を使用されたくないcaseをここに追加
            //case Motion.JumpToHangingTakeOff:
            //    break;
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
        //追加＿修正時間：20240825＿ワンユールン->20240917_チンキントウ_Pauseのフラグ追加
        Vector2 input;
        if (IsPlayerPaused)
        {
            input = Vector2.zero;
        }
        else
        {
            input = _ctx.ReadValue<Vector2>();
        }
        Vector3 moveInput = new Vector3(input.x, 0, input.y);

        //カメラの回転によってキャラクタの向きを調整します
        moveDirection = _vCam.transform.forward * moveInput.z + _vCam.transform.right * moveInput.x;
        moveDirection.y = 0f;
        if (moveDirection.magnitude > 0 && _canRotate)
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
            if (!_pushState && _canRotate)
            {
                targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            }

            //20240723＿チョウハク
            Vector3 playerDeltaMovement = _moveDirection *
           ((_walkSpeedMin += _walkAddSpeed) < _walkSpeedMax ? _walkSpeedMin : _walkSpeedMax) *
           _movementInput.magnitude *
           Time.deltaTime;




            ////20240723＿チョウハク
            //if (_pushState)
            //{
            //    //animator.ApplyBuiltinRootMotion();
            //    //animator.MatchTarget(_interactPoint.position, _interactPoint.rotation, AvatarTarget.Root,
            //    //new MatchTargetWeightMask(Vector3.one, 1f), 0.2f, 0.5f);
            //    //Debug.Log("PushState success");

            //    //20240723＿チョウハク
            //    //if (_movableObject)
            //    //{
            //    //    //Debug.Log("moveableObject success");
            //    //    //playerDeltaMovement = _cCtrl.transform.InverseTransformDirection(playerDeltaMovement);
            //    //    //playerDeltaMovement.x = 0;
            //    //    //playerDeltaMovement = _cCtrl.transform.TransformDirection(playerDeltaMovement);
            //    //    //playerDeltaMovement.y = 0f;
            //    //    //_movableObject.transform.Translate(playerDeltaMovement);
            //    //}
            //}

            //移動入力の大きさを基に速度を調整し、プレイヤーを移動させます
            _cCtrl.Move(playerDeltaMovement); //20240801_チョウハク　呼び出し所ここに移動して押す時左右移動を防ぎました

        }
    }
    /// <summary>
    /// ジャンプ処理を実行します
    /// </summary>
    private void HandleJumping()
    {
        //_velocity.y = initialJumpVelocity * 0.5f; //ジャンプ実行
        _velocity.y = _jumpForce; //ジャンプ実行_20241001更新_チンキントウ

        //_velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
    }

    /// <summary>
    /// 重力計算
    /// </summary>
    private void HandleGravity()//更新_追加時間：20241021_ワンユールン
    {



        if ((_motion == Motion.Landing || _motion == Motion.Stand || _motion == Motion.Walk))
        {
            _velocity.y = -2f;  // 地面に接地したとき、わずかに下向きの力を適用
        }

        else
        {
            float preYVelocity = _velocity.y;
            float newYVelocity = _velocity.y + (_gravity * Time.deltaTime);
            newYVelocity = Mathf.Clamp(newYVelocity, _maxGravity, Mathf.Infinity);
            float nextYVelocity = (preYVelocity + newYVelocity) * 0.5f;
            _velocity.y = nextYVelocity;
        }

        // キャラクターを移動
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
        Vector3 targetPos = _perClimbVec3 + new Vector3(0.3f, _playerClimbOffset, 0f);
        PlayerMoveToTarget(targetPos, _playerClimbSpeed);
        //DisableCharacterController();
        if (_cCtrl.transform.position == targetPos) //登るの位置に到達したら
        {
           // Debug.Log("good");
                                   // EnableCharacterController();
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
    public void SetInteractionTrigger(bool interactionTrigger)
    {
        _interactionTrigger = interactionTrigger;
    }
    public void interaction(InputAction.CallbackContext _ctx)
    {
        //InputActionPhase.Started;      <-これはGetKeyDown
        //InputActionPhase.Performed;    <-これはGetKey
        //InputActionPhase.Canceled;     <-これはGetKeyUp
        if (_ctx.phase == InputActionPhase.Started)
        {
            Debug.Log("Firwwwwwwwwwe!");
        }
    }

    //追加時間：20240723＿チョウハク
    public void OnPush(InputAction.CallbackContext _ctx)
    {
        //_isPushPressed = _ctx.ReadValueAsButton();

        if (_ctx.phase == InputActionPhase.Performed)
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
        float input = pushPullAction.ReadValue<float>();

        if (_isPushPressed==true)
        {
            // 移動可能なオブジェクトが近くにあるかチェック
            _movableObject = CheckForMovableObject();
            if (_movableObject != null)
            {
                animator.SetBool("isPush", true);
                _pushState = true;
                _interactPoint = _movableObject.GetClosestInteractPoint(transform);
                _movableObject.StartInteraction(this);
            }
        }
        else if (_isPushPressed==false)
        {
            // プッシュを解除
            animator.SetBool("isPush", false);
            _pushState = false;
            if (_movableObject != null)
            {
                _movableObject.StopInteraction();
                _movableObject = null;
            }
        }

        if (_pushState && _movableObject != null)
        {
            // 箱の移動を制御
            _movableObject.MoveBox(input);
        }
    }

    //更新_追加時間：20240824＿八子遥輝
    public void Crouch(InputAction.CallbackContext _ctx)
    {
        if (_crouchTrigger == true)
        {
            if (_movementInput.x == 0 && _movementInput.y == 0 && _ctx.phase == InputActionPhase.Started) //キャラクタが動いているときは受け付けない
            {
                _crouchFlag = true;
            }
        }

    }

    //更新_追加時間：20241002＿ワンユールン
    public void GrabHand(InputAction.CallbackContext _ctx)
    {
        if (_ctx.phase == InputActionPhase.Started)
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

    public void SetMaxSpeed(float newSpeed)
    {
        _walkSpeedMax = newSpeed;
    }

    void setJumpVariables()//更新時間：20241001_ワンユールン
    {
        // ジャンプの頂点に到達するまでの時間を計算
        float timeToApex = maxJumpTime / 2;
        // 重力を設定
        _gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        // 初期ジャンプ速度を設定
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        _jumpForce = initialJumpVelocity;
    }

    private void FaceTowardsWall(Vector3 wallPosition)//更新_追加時間：20241021_ワンユールン
    {
        // 壁の位置からキャラクターの位置へのベクトルを計算
        Vector3 directionToWall = (wallPosition - _cCtrl.transform.position).normalized;

        // キャラクターを壁の方向に向ける
        _cCtrl.transform.rotation = Quaternion.LookRotation(new Vector3(directionToWall.x, 0, directionToWall.z));
    }

    private void DisablePlayerInput()//更新_追加時間：20241021_ワンユールン
    {
        // プレイヤーの入力スクリプトを無効化
        _cCtrl.enabled = false;
    }

    private void EnablePlayerInput()//更新_追加時間：20241021_ワンユールン
    {
        // プレイヤーの入力スクリプトを再び有効化
        _cCtrl.enabled = true;
    }

    private void StopPushInteraction()
    {
        // プッシュ/プルを停止
        animator.SetBool("isPush", false);
        _canRotate = true;
        _pushState = false;
    }

    private MovableObject CheckForMovableObject()
    {
        // レイキャストで前方の移動可能なオブジェクトをチェック
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
        {
            return hit.collider.GetComponent<MovableObject>();
        }
        return null;
    }

    private void OnAnimatorMove()
    {
      //  if()
    }
}


