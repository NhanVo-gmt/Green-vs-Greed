using System;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public int             playerIndex = 0;
    public PlayerStateName currentState;

    public bool isBot = false;

    [Header("Life")]
    public int maxLife = 3;

    private int currentLife = 0;

    [Header("Card")]
    public PlayerCardDeck playerCardDeck;
    public PlayedCardDeck playedCardDeck;

    #region State

    private StateMachine stateMachine;

    public Player_IdleState playerIdleState { get; private set; }
    public Player_PickState playerPickState { get; private set; }
    public Player_DrawState playerDrawState { get; private set; }

    public Action<PlayerController> OnFinishTurn;

    #endregion
    

    private void Awake()
    {
        RegisterEvent();
        
        stateMachine = new();

        playerDrawState = new(stateMachine, this, PlayerStateName.Draw);
        playerPickState = new(stateMachine, this, PlayerStateName.Pick);
        playerIdleState = new(stateMachine, this, PlayerStateName.Idle);
        
        stateMachine.Initialize(playerIdleState);

        currentLife = maxLife;
    }

    void RegisterEvent()
    {
        playerCardDeck.OnPickCard += playedCardDeck.DrawCard;
    }

    private void OnDestroy()
    {
        playerCardDeck.OnPickCard -= playedCardDeck.DrawCard;
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

    public void FinishTurn()
    {
        OnFinishTurn?.Invoke(this);
    }

    public void DrawCard(CardRecord cardRecord)
    {
        playerCardDeck.DrawCard(cardRecord);
    }
}
