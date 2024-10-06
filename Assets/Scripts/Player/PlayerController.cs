using System;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public int MaxLives;

    [Header("Debug")]
    public int lives;

    public Action<int> OnLoseLife;
    public Action      OnDie;
    
    public void Initialize()
    {
        lives = MaxLives;
    }

    public void LoseLife()
    {
        lives--;
        OnLoseLife?.Invoke(lives);

        if (lives <= 0)
        {
            OnDie?.Invoke();
        }
    }
}

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public int             playerIndex = 0;

    public PlayerType      playerType;
    public PlayerStateName currentState;
    public PlayerData      playerData;

    public bool isBot = false;
    

    [Header("Card")]
    public PlayerCardDeck playerCardDeck;
    public PlayedCardDeck playedCardDeck;

    [Header("UI")]
    public PlayerUI playerUI;

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

        playerData.Initialize();
        playerUI.BindData(playerData);
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

    #region Live

    public void LoseLife()
    {
        playerData.LoseLife();
        Debug.Log($"[Player {playerIndex}]: Player Lose Life");
    }

    #endregion
}
