using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput _input;

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _walkSpeed = 5f;
    [SerializeField] float _sprintSpeed = 10f;
    [SerializeField] float _jumpHeight = 2f;
    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _lookSpeed = 10f;
    [SerializeField] float _maxLookAngle = 30f;
    [SerializeField] float _minLookAngle = -10f;
    [SerializeField] Transform _cameraBoom;

#if true
    [SerializeField] Transform _leftHand;
    [SerializeField] Transform _rightHand;
    //[SerializeField] float _grabDistance = 1.5f;
    //[SerializeField] float _slowMoveFactor = 0.5f; // 押している時や引っ張っている時速度のレート
    //[SerializeField] float _pushForce = 2f; //　押す力

    //private bool _isInteracting = false;
    //private GameObject _currentBox;
#endif

    CharacterController _controller;

    Vector3 _velocity;

    bool _isGrounded;

    bool _isRunning = false;

    float _currentPitch = 0f; // Current Pitch
    Vector2 _currentMoveInput = Vector2.zero;

    Push _pushObject;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _pushObject = GetComponent<Push>();
    }

    void OnEnable()
    {
        _input._onMove += Move;
        _input._onStopMove += StopMove;
        _input._onJump += Jump;
        _input._onSprint += Sprint;
        _input._onLook += Look;
        _input._onGrab += OnStartGrab;
        _input._onStopGrab += OnStopGrab;
    }

    void OnDisable()
    {
        _input._onMove -= Move;
        _input._onStopMove -= StopMove;
        _input._onJump -= Jump;
        _input._onSprint -= Sprint;
        _input._onLook -= Look;
        _input._onGrab -= OnStartGrab;
        _input._onStopGrab -= OnStopGrab;
    }

    // Start is called before the first frame update
    void Start()
    {
        _input.EnableGameplayInput();
    }

    void Update()
    {
        if (_isRunning == true)
        {
            _moveSpeed = _sprintSpeed;
        }
        else
        {
            _moveSpeed = _walkSpeed;
        }

        if (_pushObject.IsInteracting())
        {
            _moveSpeed *= _pushObject.GetSlowMoveFactor();
        }

        // Check if the player is grounded
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Small negative value to keep the player grounded
        }

        // Apply gravity
        _velocity.y += _gravity * Time.deltaTime;

        Vector3 move = new Vector3(_currentMoveInput.x, 0, _currentMoveInput.y);
        move = Quaternion.Euler(0, _cameraBoom.eulerAngles.y, 0) * move;
        _controller.Move((move * _moveSpeed +_velocity) * Time.deltaTime);

    }

    void OnStartGrab()
    {
        _pushObject.StartGrab();
    }

    void OnStopGrab()
    {
        _pushObject.StopGrab();
    }

    public Vector2 GetCurrentMoveInput()
    { 
        return _currentMoveInput;
    }

    public void MoveHands(bool interacting)
    {
        if (interacting)
        {
            Debug.Log("Setting hands to grab position");
            _leftHand.localRotation = Quaternion.Euler(90, 0, 0);
            _rightHand.localRotation = Quaternion.Euler(90, 0, 0);
        }
        else
        {
            Debug.Log("Resetting hands to initial position");
            _leftHand.localRotation = Quaternion.identity; // Reset to initial rotation
            _rightHand.localRotation = Quaternion.identity; // Reset to initial rotation
        }
    }

    void Move(Vector2 moveInput)
    {
        _currentMoveInput = moveInput;
    }
    void StopMove()
    {
        _currentMoveInput = Vector2.zero;
        _isRunning = false;
    }

    void Jump()
    {
        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }

    void Look(Vector2 lookInput)
    {
        float mouseX = lookInput.x * _lookSpeed * Time.deltaTime;
        float mouseY = lookInput.y * _lookSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        _currentPitch = Mathf.Clamp(_currentPitch - mouseY, _minLookAngle, _maxLookAngle);

        _cameraBoom.localRotation = Quaternion.Euler(_currentPitch, 0f, 0f);

        //Camera.main.transform.Rotate(Vector3.right * -mouseY);
    }

    void Sprint()
    {
        if (_velocity.magnitude > 0)
        {
            _isRunning = !_isRunning;
        }
    }
}

