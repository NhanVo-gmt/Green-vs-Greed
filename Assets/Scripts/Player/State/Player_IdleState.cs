using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleState : PlayerState
{
    public Player_IdleState(StateMachine stateMachine, Player player, PlayerStateName stateName) : base(stateMachine, player, stateName)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        player.FinishTurn();
    }
}
