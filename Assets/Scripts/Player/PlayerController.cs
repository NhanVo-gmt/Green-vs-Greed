using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int             playerIndex = 0;
    public PlayerStateName currentState;

    #region State

    private StateMachine stateMachine;

    private Player_IdleState playerIdleState;
    private Player_PickState playerPickState;

    #endregion

    private void Awake()
    {
        stateMachine = new();

        playerIdleState = new(stateMachine, this, PlayerStateName.Idle);
        playerPickState = new(stateMachine, this, PlayerStateName.Pick);
        
        stateMachine.Initialize(playerIdleState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void StartTurn()
    {
        stateMachine.ChangeState(playerPickState);
    }
}
