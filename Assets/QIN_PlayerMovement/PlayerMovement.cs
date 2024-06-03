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
        // 移動入力を基に方向ベクトルを計算し、正規化します。
        Vector3 direction = new Vector3(_movementInput.x, 0f, _movementInput.y).normalized;

        // 方向ベクトルの大きさが0.1以上の場合に移動を実行します。
        if (direction.magnitude >= 0.1f)
        {

            // Cinemachine仮想カメラのTransformを取得します。
            Transform _camTransform = _vCam.transform;

            // カメラの前方向を取得します。
            Vector3 _forward = _camTransform.forward;

            // Y軸の影響を無視するために前方向のY成分を0にします。
            _forward.y = 0f;

            // 前方向ベクトルを正規化します。
            _forward.Normalize();

            // カメラの右方向を取得します。
            Vector3 _right = _camTransform.right;

            // Y軸の影響を無視するために右方向のY成分を0にします。
            _right.y = 0f;

            // 右方向ベクトルを正規化します。
            _right.Normalize();

            // 前方向と右方向を基に移動方向を計算します。
            Vector3 _moveDirection = _forward * direction.z + _right * direction.x;

            // 計算した移動方向と速度を基にプレイヤーを移動させます。
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
