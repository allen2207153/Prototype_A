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
    [SerializeField] float _minLookAngle = -50f;
    [SerializeField] Transform _cameraBoom;

    CharacterController _controller;

    Vector3 _velocity;

    bool _isGrounded;

    bool _isRunning = false;

    float _currentPitch = 0f; // Current Pitch
    Vector2 _currentMoveInput = Vector2.zero;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        _input._onMove += Move;
        _input._onStopMove += StopMove;
        _input._onJump += Jump;
        _input._onSprint += Sprint;
        _input._onLook += Look;
    }

    void OnDisable()
    {
        _input._onMove -= Move;
        _input._onStopMove -= StopMove;
        _input._onJump -= Jump;
        _input._onSprint -= Sprint;
        _input._onLook -= Look;
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

