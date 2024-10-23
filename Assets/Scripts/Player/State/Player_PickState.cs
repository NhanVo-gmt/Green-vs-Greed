using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;

public class Player_PickState : PlayerState
{
    private float waitTime        = 2f;
    private float elapsedWaitTime = 0f;
    
    public Player_PickState(StateMachine stateMachine, Player player, PlayerStateName stateName) : base(stateMachine, player, stateName)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (player.isBot) elapsedWaitTime = Random.Range(0f, waitTime);
        else CanPick();
        
    }

    void CanPick()
    {
        player.playerCardDeck.OnPickCard += ChangeState;
        
        player.playerCardDeck.SetPickState(true);
    }

    public override void Update()
    {
        if (elapsedWaitTime < 0f) return;
        
        elapsedWaitTime -= Time.deltaTime;
        if (elapsedWaitTime <= 0f)
        {
            CanPick();
            elapsedWaitTime = Random.Range(0f, waitTime);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        
        player.playerCardDeck.SetPickState(false);
        
        player.playerCardDeck.OnPickCard -= ChangeState;
    }

    private void ChangeState(CardRecord record)
    {
        if (!record.IsEndTurn) return;
        
        stateMachine.ChangeState(player.playerIdleState);
    }
}
