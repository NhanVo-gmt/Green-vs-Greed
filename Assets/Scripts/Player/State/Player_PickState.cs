using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;

public class Player_PickState : PlayerState
{
    private float waitTime        = 2f;
    private float elapsedWaitTime = 0f;

    private int maxCard         = 3;
    private int randomCardPick  = 0;
    private int currentCardPick = 0;
    
    public Player_PickState(StateMachine stateMachine, Player player, PlayerStateName stateName) : base(stateMachine, player, stateName)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        ResetState();
        RegisterEvent();
    }

    void ResetState()
    {
        currentCardPick = 0;
        if (player.isBot)
        {
            randomCardPick = Random.Range(0, maxCard);
            elapsedWaitTime = Random.Range(0f, waitTime);
        }
    }
    
    void RegisterEvent()
    {
        player.playerUI.OnEndTurnButtonClicked += ChangeState;
        if (!player.isBot) player.playerUI.SetStateEndTurn(true);
        
        player.playerCardDeck.OnPickCard += OnPickCard;
        player.playerCardDeck.SetPickState(true);
    }
    
    private void OnPickCard(CardRecord card)
    {
        currentCardPick++;
        if (currentCardPick >= randomCardPick && player.isBot)
        {
            ChangeState();
        }
    }

    public override void Update()
    {
        if (!player.isBot) return;
        
        if (elapsedWaitTime < 0f) return;
        
        elapsedWaitTime -= Time.deltaTime;
        if (elapsedWaitTime <= 0f)
        {
            player.playerCardDeck.PickRandomCard();
            elapsedWaitTime = Random.Range(0f, waitTime);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        
        DeregisterEvent();
    }

    void DeregisterEvent()
    {
        player.playerUI.SetStateEndTurn(false);
        player.playerUI.OnEndTurnButtonClicked -= ChangeState;
        
        player.playerCardDeck.SetPickState(false);
        player.playerCardDeck.OnPickCard -= OnPickCard;
    }

    private void ChangeState()
    {
        stateMachine.ChangeState(player.playerIdleState);
    }
}
