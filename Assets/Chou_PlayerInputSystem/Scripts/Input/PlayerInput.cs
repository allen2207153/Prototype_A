using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, PlayerInputActions.IGamePlayActions
{
    public event UnityAction<Vector2> _onMove = delegate {};
    public event UnityAction _onStopMove = delegate {};
    public event UnityAction _onJump = delegate { };
    public event UnityAction _onSprint = delegate { };
    public event UnityAction<Vector2> _onLook = delegate { };
    public event UnityAction _onGrab = delegate { };
    public event UnityAction _onStopGrab = delegate { };

    PlayerInputActions _inputActions;

    void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.GamePlay.SetCallbacks(this);
    }

    void OnDisable()
    {
        DisableAllInputs();
    }

    public void EnableGameplayInput()
    {
        _inputActions.GamePlay.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DisableAllInputs()
    {
        _inputActions.GamePlay.Disable();
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _onGrab.Invoke();
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            _onStopGrab.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _onMove.Invoke(context.ReadValue<Vector2>());
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            _onStopMove.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
        {
            _onLook.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _onJump.Invoke();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _onSprint.Invoke();
        }
    }
}
