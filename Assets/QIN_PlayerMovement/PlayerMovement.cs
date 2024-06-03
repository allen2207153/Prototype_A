using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    [SerializeField] private float _jumpForce = 10.0f;

    private Vector2 _movementInput = Vector2.zero;
    private Vector3 _gravityVec3 = Vector3.zero;

    private CharacterController _cCtrl;
    [SerializeField] private CinemachineVirtualCamera _vCam;
    // Start is called before the first frame update
    void Start()
    {
        _cCtrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // �ړ����͂���ɕ����x�N�g�����v�Z���A���K�����܂��B
        Vector3 direction = new Vector3(_movementInput.x, 0f, _movementInput.y).normalized;

        // �����x�N�g���̑傫����0.1�ȏ�̏ꍇ�Ɉړ������s���܂��B
        if (direction.magnitude >= 0.1f)
        {

            // Cinemachine���z�J������Transform���擾���܂��B
            Transform _camTransform = _vCam.transform;

            // �J�����̑O�������擾���܂��B
            Vector3 _forward = _camTransform.forward;

            // Y���̉e���𖳎����邽�߂ɑO������Y������0�ɂ��܂��B
            _forward.y = 0f;

            // �O�����x�N�g���𐳋K�����܂��B
            _forward.Normalize();

            // �J�����̉E�������擾���܂��B
            Vector3 _right = _camTransform.right;

            // Y���̉e���𖳎����邽�߂ɉE������Y������0�ɂ��܂��B
            _right.y = 0f;

            // �E�����x�N�g���𐳋K�����܂��B
            _right.Normalize();

            // �O�����ƉE��������Ɉړ��������v�Z���܂��B
            Vector3 _moveDirection = _forward * direction.z + _right * direction.x;

            // �v�Z�����ړ������Ƒ��x����Ƀv���C���[���ړ������܂��B
            _cCtrl.Move(_moveDirection * _moveSpeed * Time.deltaTime);
        }
        if (!_cCtrl.isGrounded)
        {
            PlayerGravity();
        }
        else
        {
            _gravityVec3 = Vector3.zero;
        }
    }
    public void Move(InputAction.CallbackContext _ctx)
    {
        if (_ctx.phase == InputActionPhase.Performed)
        {
            _movementInput = _ctx.ReadValue<Vector2>();
        }
        else if (_ctx.phase == InputActionPhase.Canceled)
        {
            _movementInput = Vector2.zero;
        }
    }
    public void Jump(InputAction.CallbackContext _ctx)
    {
        if (_ctx.phase == InputActionPhase.Started)
        {
            _cCtrl.SimpleMove(Vector3.up * _jumpForce);
        }
    }
    private void PlayerGravity()
    {
        _gravityVec3.y += _gravity * Time.deltaTime;
        _cCtrl.Move(_gravityVec3 * Time.deltaTime);
    }
}
