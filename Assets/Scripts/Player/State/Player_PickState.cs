using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;

public class Player_PickState : PlayerState
{
    public Player_PickState(StateMachine stateMachine, PlayerController player, PlayerStateName stateName) : base(stateMachine, player, stateName)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.playerCardDeck.OnPickCard += ChangeState;
        
        player.playerCardDeck.SetPickState(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        
        player.playerCardDeck.SetPickState(false);
        
        player.playerCardDeck.OnPickCard -= ChangeState;
        
        
        player.FinishTurn();
    }

    private void ChangeState(CardRecord record)
    {
        stateMachine.ChangeState(player.playerIdleState);
    }
}
