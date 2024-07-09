using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    /// <summary>
    /// PlayerState
    /// </summary>
    private enum State
    {
        Idle,
        Move,
        Jump,
        Dead,
    }

    private State _nowState;
    private State _nextState;
    // Start is called before the first frame update
    void Start()
    {
        IdleStart();
    }

    // Update is called once per frame
    void Update()
    {
        //現在のstateの処理
        switch (_nowState)
        {
            case State.Idle:
                IdleUpDate();
                break;
            case State.Move:
                MoveUpDate();
                break;
            case State.Jump:
                JumpUpDate();
                break;
            case State.Dead:
                DeadUpDate();
                break;
        }
        //stateが切り替わり
        if (_nowState != _nextState)
        {
            //現在のstateの処置を終わらせる
            switch (_nowState)
            {
                case State.Idle:
                    IdleEnd();
                    break;
                case State.Move:
                    MoveEnd();
                    break;
                case State.Jump:
                    JumpEnd();
                    break;
                case State.Dead:
                    DeadEnd();
                    break;
            }
            //次のstateに遷移する
            _nowState = _nextState;
            switch (_nowState)
            {
                case State.Idle:
                    IdleStart();
                    break;
                case State.Move:
                    MoveStart();
                    break;
                case State.Jump:
                    JumpStart();
                    break;
                case State.Dead:
                    DeadStart();
                    break;
            }
        }
    }
    private void ChangeState(State nextState)
    {
        _nextState = nextState;
    }
    //-----------------------------------------
    //IdleState管理関数
    //-----------------------------------------
    private void IdleStart()
    {
        Debug.Log("Idle状態開始");
    }
    private void IdleUpDate()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeState(State.Move);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeState(State.Jump);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeState(State.Dead);
        }
    }
    private void IdleEnd()
    {
        Debug.Log("Idle状態終了");
    }
    //-----------------------------------------
    //MoveState管理関数
    //-----------------------------------------
    private void MoveStart()
    {
        Debug.Log("Move状態開始");
    }
    private void MoveUpDate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChangeState(State.Idle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeState(State.Jump);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeState(State.Dead);
        }
    }
    private void MoveEnd()
    {
        Debug.Log("Move状態終了");
    }
    //-----------------------------------------
    //JumpState管理関数
    //-----------------------------------------
    private void JumpStart()
    {
        Debug.Log("Jump状態開始");
    }
    private void JumpUpDate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChangeState(State.Idle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeState(State.Move);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeState(State.Dead);
        }
    }
    private void JumpEnd()
    {
        Debug.Log("Jump状態終了");
    }
    //-----------------------------------------
    //DeadState管理関数
    //-----------------------------------------
    private void DeadStart()
    {
        Debug.Log("Dead状態開始");
    }
    private void DeadUpDate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChangeState(State.Idle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeState(State.Move);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeState(State.Jump);

        }
    }
    private void DeadEnd()
    {
        Debug.Log("Dead状態終了");
    }
}
