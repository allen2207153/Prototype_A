using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : BChara
{
    //�d�͂̑傫����ݒ肵�܂�
    [SerializeField] private float _gravity = -9.8f;
    //�ړ����x��ݒ肵�܂�
    [SerializeField] private float _walkSpeed = 10f;
    //�W�����v�͂�ݒ肵�܂�
    [SerializeField] private float _jumpForce = 20.0f;

    [Header("_moveCnt�̒l���ϑ����邾��")]
    [SerializeField] private int _checkMoveCnt;

    //���z�J�����̎Q�Ƃ�ݒ肵�܂�
    [Header("CinemachineVirtualCamera")]
    [SerializeField] private CinemachineVirtualCamera _vCam;

    //�ړ����͂�ۑ�����ϐ�
    private Vector2 _movementInput = Vector2.zero;
    //�d�͂�W�����v�̑��x��ۑ�����ϐ�
    private Vector3 _velocity = Vector3.zero;

    //�L�����N�^�[�R���g���[���[�̎Q��
    private CharacterController _cCtrl;

    //�W�����v�̃t���O
    private bool _jumpFlag = false;

    void Start()
    {
        //�L�����N�^�[�R���g���[���[���擾���܂�
        _cCtrl = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        _moveCnt++;//��������
    }
    void Update()
    {
        //_moveCnt�̒l���ϑ����邾��
        _checkMoveCnt = _moveCnt;

        Think();
        Move();

        //�d�͏��������s���܂�
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

                break;
            case Motion.TakeOff:

                break;
        }
    }
    /// <summary>
    /// InputSystem��Walk_Action
    /// </summary>
    /// <param name="_ctx"></param>
    public void Walk(InputAction.CallbackContext _ctx)
    {
        //���͂̃t�F�[�Y��Performed�̏ꍇ�A�ړ����͂�ǂݎ��܂�
        if (_ctx.phase == InputActionPhase.Performed)
        {
            _movementInput = _ctx.ReadValue<Vector2>();
        }
        //���͂̃t�F�[�Y��Canceled�̏ꍇ�A�ړ����͂����Z�b�g���܂�
        else if (_ctx.phase == InputActionPhase.Canceled)
        {
            _movementInput = Vector2.zero;
        }
    }
    /// <summary>
    /// InputSystem��Jump_Action
    /// </summary>
    /// <param name="_ctx">InputSystem�̕ϐ�</param>
    public void Jump(InputAction.CallbackContext _ctx)
    {
        if (_ctx.phase == InputActionPhase.Started)
        {
            _jumpFlag = true;
        }
        else if (_ctx.phase == InputActionPhase.Canceled)
        {
            _jumpFlag = false;
        }
    }

    /// <summary>
    /// �ړ����������s���܂�
    /// </summary>
    private void HandleWalking()
    {
        //�ړ����͂Ɋ�Â��ĕ����x�N�g�����v�Z���A���K�����܂�
        Vector3 direction = new Vector3(_movementInput.x, 0f, _movementInput.y).normalized;

        //�����x�N�g���̑傫����0.1�ȏ�̏ꍇ�Ɉړ������s���܂�
        if (direction.magnitude >= 0.1f)
        {
            //Cinemachine���z�J������Transform���擾���܂�
            Transform _camTransform = _vCam.transform;
            //�J�����̑O�������擾���܂�
            Vector3 _forward = Vector3.Scale(_camTransform.forward, new Vector3(1, 0, 1)).normalized;
            //�J�����̉E�������擾���܂�
            Vector3 _right = Vector3.Scale(_camTransform.right, new Vector3(1, 0, 1)).normalized;

            //�O�����ƉE��������Ɉړ��������v�Z���܂�
            Vector3 _moveDirection = _forward * direction.z + _right * direction.x;
            //�ړ����͂̑傫������ɑ��x�𒲐����A�v���C���[���ړ������܂�
            _cCtrl.Move(_moveDirection * _walkSpeed * _movementInput.magnitude * Time.deltaTime);
        }
    }
    /// <summary>
    /// �W�����v���������s���܂�
    /// </summary>
    private void HandleJumping()
    {
        _velocity.y = _jumpForce;
        //_velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
    }

    /// <summary>
    /// �d�͌v�Z
    /// </summary>
    private void HandleGravity()
    {
        switch (_motion)
        {
            case Motion.Landing:
                if (_velocity.y < 0)
                {
                    _velocity.y = -2f; //�v���C���[��n�ʂɕۂ�
                }
                break;
            case Motion.Walk:
                if (_velocity.y < 0)
                {
                    _velocity.y = -2f; //�v���C���[��n�ʂɕۂ�
                }
                break;
            //�d�͂�K�p���Ȃ����[�V������ǉ�

            default:
                //�d�͂�K�p���܂�
                _velocity.y += _gravity * Time.deltaTime;
                break;
        }

        //�L�����N�^�[���d�͂Ɋ�Â��Ĉړ������܂�
        _cCtrl.Move(_velocity * Time.deltaTime);
    }
}