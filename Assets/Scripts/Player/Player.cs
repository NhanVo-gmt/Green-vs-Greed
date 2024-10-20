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

public class Player : MonoBehaviour
{
    [Header("Player")]
    public int             playerIndex = 0;
    
    public PlayerStateName currentState;
    public PlayerData      playerData;

    public bool isBot = false;

    public PlayerType playerType => playerRecord.PlayerType;
    

    [Header("Card")]
    public PlayerCardDeck playerCardDeck;
    public PlayedCardDeck playedCardDeck;
    public int            shufflePerTurn = 1;

    [Header("UI")]
    public PlayerUI playerUI;

    [Header("Debug")]
    public int shuffleLeft = 1;

    private PlayerRecord playerRecord;

    #region State

    private StateMachine stateMachine;

    public Player_IdleState playerIdleState { get; private set; }
    public Player_PickState playerPickState { get; private set; }
    public Player_DrawState playerDrawState { get; private set; }

    public Action OnShuffle;
    public Action<Player> OnFinishTurn;

    #endregion

    public void BindData(PlayerRecord playerRecord)
    {
        this.playerRecord = playerRecord;
        
        RegisterEvent();
        
        stateMachine = new();

        playerDrawState = new(stateMachine, this, PlayerStateName.Draw);
        playerPickState = new(stateMachine, this, PlayerStateName.Pick);
        playerIdleState = new(stateMachine, this, PlayerStateName.Idle);
        
        stateMachine.Initialize(playerIdleState);

        playerData.Initialize();
        playerUI.BindData(playerData, playerRecord);
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
        shuffleLeft           =  shufflePerTurn;
        ShuffleCard.OnShuffle += Shuffle;
        
        stateMachine.ChangeState(playerPickState);
    }

    public void FinishTurn()
    {
        ShuffleCard.OnShuffle -= Shuffle;
        
        OnFinishTurn?.Invoke(this);
    }

    public void DrawCard(CardRecord cardRecord)
    {
        playerCardDeck.DrawCard(cardRecord);
    }

    public void DiscardAllCards()
    {
        playerCardDeck.DiscardAllCards();
    }

    #region Shuffle

    public void Shuffle()
    {
        if (shuffleLeft <= 0) return;

        shuffleLeft--;
        OnShuffle?.Invoke();
    }

    #endregion

    #region Live

    public void LoseLife()
    {
        playerData.LoseLife();
        Debug.Log($"[Player {playerIndex}]: Player Lose Life");
    }

    #endregion

    public int GetCurrentNumberPlayerDeck()
    {
        return playerCardDeck.GetCurrentNumberCards();
    }
}
