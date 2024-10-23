using System;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public Action<int>           OnLoseLife;
    public Action<Resource, int> OnUpdateResource;
    public Action                OnDie;

    public PlayerRecord              record;
    public Dictionary<Resource, int> resources = new();

    public void BindData(PlayerRecord playerRecord)
    {
        record = playerRecord;
        foreach (var resource in record.Resources.Values)
        {
            resources.Add(resource.ResourceId, resource.ResourceAmount);
        }
    }

    public void ChangeResourceAmount(Resource type, int amount)
    {
        resources[type] += + amount;
        OnUpdateResource?.Invoke(type, resources[type]);
        
        if (resources[type] <= 0)
        {
            resources[type] = 0;
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
    public  GameManager  gameManager { get; private set; }

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
        
        playerData.BindData(playerRecord);
        
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

    #region Resource

    public void ChangeResourceAmount(Resource type, int amount)
    {
        playerData.ChangeResourceAmount(type, amount);
        Debug.Log($"[Player {playerIndex}]: {type} {amount}");
    }

    #endregion

    public int GetCurrentNumberPlayerDeck()
    {
        return playerCardDeck.GetCurrentNumberCards();
    }
}
