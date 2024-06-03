using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : BChara
{
    // �d�͂̑傫����ݒ肵�܂�
    [SerializeField] private float _gravity = -9.8f;
    // �ړ����x��ݒ肵�܂�
    [SerializeField] private float _moveSpeed = 10f;
    // �W�����v�͂�ݒ肵�܂�
    [SerializeField] private float _jumpForce = 10.0f;

    // ���z�J�����̎Q�Ƃ�ݒ肵�܂�
    [Header("CinemachineVirtualCamera")]
    [SerializeField] private CinemachineVirtualCamera _vCam;

    // �ړ����͂�ۑ�����ϐ�
    private Vector2 _movementInput = Vector2.zero;
    // �d�͂�W�����v�̑��x��ۑ�����ϐ�
    private Vector3 _velocity = Vector3.zero;

    // �L�����N�^�[�R���g���[���[�̎Q��
    private CharacterController _cCtrl;

    void Start()
    {
        // �L�����N�^�[�R���g���[���[���擾���܂�
        _cCtrl = GetComponent<CharacterController>();
    }

    void Update()
    {
        // �ړ����������s���܂�
        HandleMovement();
        // �d�͏��������s���܂�
        HandleGravity();
        if (CheckFoot())
        {
            Debug.Log("checkfoot");
        }
    }

    public void Move(InputAction.CallbackContext _ctx)
    {
        // ���͂̃t�F�[�Y��Performed�̏ꍇ�A�ړ����͂�ǂݎ��܂�
        if (_ctx.phase == InputActionPhase.Performed)
        {
            _movementInput = _ctx.ReadValue<Vector2>();
        }
        // ���͂̃t�F�[�Y��Canceled�̏ꍇ�A�ړ����͂����Z�b�g���܂�
        else if (_ctx.phase == InputActionPhase.Canceled)
        {
            _movementInput = Vector2.zero;
        }
    }

    public void Jump(InputAction.CallbackContext _ctx)
    {
        // ���͂̃t�F�[�Y��Started�ł���A�L�����N�^�[���n�ʂɐڂ��Ă���ꍇ�A�W�����v�͂�K�p���܂�
        if (_ctx.phase == InputActionPhase.Started && CheckFoot())
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
        }
    }

    private void HandleMovement()
    {
        // �ړ����͂Ɋ�Â��ĕ����x�N�g�����v�Z���A���K�����܂�
        Vector3 direction = new Vector3(_movementInput.x, 0f, _movementInput.y).normalized;

        // �����x�N�g���̑傫����0.1�ȏ�̏ꍇ�Ɉړ������s���܂�
        if (direction.magnitude >= 0.1f)
        {
            // Cinemachine���z�J������Transform���擾���܂�
            Transform _camTransform = _vCam.transform;
            // �J�����̑O�������擾���܂�
            Vector3 _forward = Vector3.Scale(_camTransform.forward, new Vector3(1, 0, 1)).normalized;
            // �J�����̉E�������擾���܂�
            Vector3 _right = Vector3.Scale(_camTransform.right, new Vector3(1, 0, 1)).normalized;

            // �O�����ƉE��������Ɉړ��������v�Z���܂�
            Vector3 _moveDirection = _forward * direction.z + _right * direction.x;
            // �ړ����͂̑傫������ɑ��x�𒲐����A�v���C���[���ړ������܂�
            _cCtrl.Move(_moveDirection * _moveSpeed * _movementInput.magnitude * Time.deltaTime);
        }
    }

    private void HandleGravity()
    {
        // �L�����N�^�[���n�ʂɐڂ��Ă���ꍇ
        if (CheckFoot())
        {
            // �L�����N�^�[�����~���ł���΁AY�����x�����Z�b�g���܂�
            if (_velocity.y < 0)
            {
                _velocity.y = -2f; // �v���C���[��n�ʂɕۂ�
            }
        }
        // �L�����N�^�[���󒆂ɂ���ꍇ
        else
        {
            // �d�͂�K�p���܂�
            _velocity.y += _gravity * Time.deltaTime;
        }

        // �L�����N�^�[���d�͂Ɋ�Â��Ĉړ������܂�
        _cCtrl.Move(_velocity * Time.deltaTime);
    }
}