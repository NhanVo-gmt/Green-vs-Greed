using System;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public Action<int>           OnLoseLife;
    public Action<Resource, int> OnAddResource;
    public Action<Resource, int> OnUpdateResource;
    public Action<int>                OnDie;

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
        OnAddResource?.Invoke(type, amount);
        OnUpdateResource?.Invoke(type, resources[type]);
        
        if (resources[type] <= 0)
        {
            resources[type] = 0;
            OnDie?.Invoke(record.Id);
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
    public bool showView = false;

    public PlayerType playerType => playerRecord.PlayerType;
    

    [Header("Card")]
    public PlayerCardDeck playerCardDeck;
    public PlayedCardDeck playedCardDeck;
    public Transform      target;

    [Header("Site")]
    public PlayerConstruction construction;

    [Header("UI")]
    public PlayerUI playerUI;

    [Header("VFX")]
    public VFX shieldVFX;

    [Header("Debug")]
    public int blindActivateRound = 0;

    public int block = 0;

    private PlayerRecord playerRecord;
    public  GameManager  gameManager { get; private set; }

    #region State

    private StateMachine stateMachine;

    public Player_IdleState playerIdleState { get; private set; }
    public Player_PickState playerPickState { get; private set; }
    public Player_DrawState playerDrawState { get; private set; }
    
    public Action<Player> OnFinishTurn;

    #endregion

    public void BindData(PlayerRecord playerRecord)
    {
        this.playerRecord = playerRecord;

        stateMachine = new();

        playerDrawState = new(stateMachine, this, PlayerStateName.Draw);
        playerPickState = new(stateMachine, this, PlayerStateName.Pick);
        playerIdleState = new(stateMachine, this, PlayerStateName.Idle);
        
        stateMachine.Initialize(playerIdleState);
        
        playerData.BindData(playerRecord);
        playerUI.BindData(playerData);
        construction.BindData(playerData, playerRecord.PlayerUpgrades);
    }

    public void PickCard(CardDeckType deckType, CardRecord record, Transform target)
    {
        switch (deckType)
        {
            case CardDeckType.Hand:
                playedCardDeck.DrawCard(record, target);
                break;
            case CardDeckType.Played:
                playerCardDeck.DrawCard(record, target);
                break;
        }
    }

    public bool CanPickCard(CardDeckType deckType)
    {
        switch (deckType)
        {
            case CardDeckType.Hand:
                return !playedCardDeck.IsFull();
            case CardDeckType.Played:
                return !playerCardDeck.IsFull();
        }

        return false;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    #region Turn

    public void StartTurn()
    {
        NextRound();
        playedCardDeck.SetBlindState(blindActivateRound > 0);
        stateMachine.ChangeState(playerPickState);
    }
    
    public void NextRound()
    {
        blindActivateRound--;
        block--;

        if (block == 0)
        {
            shieldVFX.Play("LoseShield");
            SoundManager.Instance.PlayOneShot(SoundType.LoseShield);
        }
    }
    
    public void FinishTurn()
    {
        OnFinishTurn?.Invoke(this);
    }
    

    #endregion

    #region Card

    public void DrawCard(CardRecord cardRecord, Transform target)
    {
        playerCardDeck.DrawCard(cardRecord, target);
    }

    public void DiscardAllCards()
    {
        playerCardDeck.DiscardAllCards();
    }
    

    #endregion
    
    #region Effect

    public void Blind(int blindRound)
    {
        blindActivateRound = blindRound;
        playedCardDeck.SetBlindState(true);
    }

    public void Permit(int blockRound)
    {
        block = blockRound;
        if (block > 0)
        {
            shieldVFX.Play("GetShield");
            SoundManager.Instance.PlayOneShot(SoundType.GetShield);
        }
    }

    #endregion
    

    #region Resource

    public void ChangeResourceAmount(Resource type, int amount)
    {
        if (block > 0)
        {
            if (isBot)
            {
                shieldVFX.Play("EnemyDefend");
            }
            else shieldVFX.Play("Defend");
            SoundManager.Instance.PlayOneShot(SoundType.UseShield);
            return;
        }
        
        playerData.ChangeResourceAmount(type, amount);
        Debug.Log($"[Player {playerIndex}]: {type} {amount}");
    }

    #endregion

    public int GetCurrentNumberPlayerDeck()
    {
        return playerCardDeck.GetCurrentNumberCards();
    }
}
