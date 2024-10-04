using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_PickState : PlayerState
{
    public Player_PickState(StateMachine stateMachine, PlayerController player, PlayerStateName stateName) : base(stateMachine, player, stateName)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.ChangeState(player.playerIdleState);
    }
}
