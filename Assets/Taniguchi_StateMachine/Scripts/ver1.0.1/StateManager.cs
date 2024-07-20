using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    //protected BaseState<EState> currentState;

    protected bool IsTransitioningState = false;

    public BaseState<EState> currentState { get;protected set; }

    // Start is called before the first frame update
    void Start()
    {
        currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        EState nextStateKey = currentState.GetNextState();

        if (!IsTransitioningState&& nextStateKey.Equals(currentState.StateKey))
        {
            currentState.UpdateState();
        }
        else if(!IsTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    public void TransitionToState(EState statekey)
    {
        IsTransitioningState = true;
        currentState.ExitState();
        currentState = States[statekey];
        currentState.EnterState();
        IsTransitioningState = false;
    }

    void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }

    void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }
}
