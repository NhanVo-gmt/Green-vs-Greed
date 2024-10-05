using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateName
{
    Idle = 0,
    Pick = 1,
    Draw = 2,
}

public abstract class PlayerState : State
{
    protected PlayerController player;
    protected PlayerStateName  stateName;

    protected PlayerState(StateMachine stateMachine, PlayerController player, PlayerStateName stateName) : base(stateMachine)
    {
        this.player    = player;
        this.stateName = stateName;
    }

    public override void OnEnter()
    {
        player.currentState = stateName;
    }

    public override void OnExit()
    {
        
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }
}
