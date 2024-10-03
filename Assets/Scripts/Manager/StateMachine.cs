using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState;

    public void Initialize(State firstState)
    {
        currentState = firstState;
        currentState.OnEnter();
    }

    public void ChangeState(State newState)
    {
        if (currentState == newState) return;
        
        currentState.OnExit();

        currentState = newState;
        currentState.OnEnter();
    }

    public void Update()
    {
        currentState.Update();
    }

    public void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
}
