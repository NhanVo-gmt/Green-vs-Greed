using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blueprints;
using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
using GameFoundation.Scripts.Utilities.Extension;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UserData.Controller;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int numberPlayers;
    
    [Header("Debug")]
    [SerializeField] private int currentPlayerIndex = -1;

    private Dictionary<int, PlayerController> PlayerControllers = new();

    [Inject] private CardManager    CardManager;
    [Inject] private IScreenManager ScreenManager;
    

    private void Start()
    {
        this.GetCurrentContainer().Inject(this);
        
        FindAllPlayers();
        
        CardManager.OnCardDataLoaded += DrawAllCards;
        DrawAllCards();
    }
    
    void FindAllPlayers()
    {
        numberPlayers = 0;
        foreach (var player in GameObject.FindObjectsOfType<PlayerController>(true))
        {
            player.OnFinishTurn     += NextPlayerTurn;
            player.playerData.OnDie += EndGame;
            
            PlayerControllers.Add(player.playerIndex, player);
            numberPlayers++;
        }
    }
    

    private void OnDestroy()
    {
        CardManager.OnCardDataLoaded -= DrawAllCards;
        foreach (var player in PlayerControllers.Values)
        {
            player.OnFinishTurn     -= NextPlayerTurn;
            player.playerData.OnDie -= EndGame;
        }
        PlayerControllers.Clear();
    }
    

    private void DrawAllCards()
    {
        if (CardManager.GetCards(PlayerType.Corporation).Count == 0) return;
        
        foreach (var player in PlayerControllers.Values)
        {
            foreach (var cardRecord in CardManager.GetCards(player.playerType))
            {
                player.DrawCard(cardRecord);
            }
        }

        NextPlayerTurn();
    }

    

    void NextPlayerTurn(PlayerController player)
    {
        if (player.playerIndex != currentPlayerIndex) return;
        
        NextPlayerTurn();
    }

    void NextPlayerTurn()
    {
        DrawCardEndTurn();
        
        if (currentPlayerIndex >= numberPlayers - 1)
        {
            EndBothTurn();
        }
        else
        {
            int newIndex = currentPlayerIndex >= numberPlayers - 1 ? 0 : currentPlayerIndex + 1;
            Debug.Log($"[Game Manager]: Change Player from {currentPlayerIndex} to {newIndex}");

            currentPlayerIndex = newIndex;
            StartPlayerTurn();
        }
    }

    void DrawCardEndTurn()
    {
        if (currentPlayerIndex != -1) PlayerControllers[currentPlayerIndex].DrawCard(CardManager.DrawRandomCard(PlayerControllers[currentPlayerIndex].playerType));
    }

    void StartPlayerTurn()
    {
        PlayerControllers[currentPlayerIndex].StartTurn();
    }

    [Button("Draw Card")]
    public void DrawCard()
    {
        if (currentPlayerIndex == 0)
        {
            PlayerControllers[currentPlayerIndex].DrawCard(CardManager.DrawRandomCard(PlayerType.Environment));
        }
        else PlayerControllers[currentPlayerIndex].DrawCard(CardManager.DrawRandomCard(PlayerType.Corporation));
    }

    #region End Turn

    public void EndBothTurn()
    {
        StartCoroutine(EndBothTurnCoroutine());
    }


    IEnumerator EndBothTurnCoroutine()
    {
        Debug.Log($"[Game Manager]: Check End Turn");
        
        List<CardSlot> player0Cards = PlayerControllers[0].playedCardDeck.CardSlots;
        List<CardSlot> player1Cards = PlayerControllers[1].playedCardDeck.CardSlots;
        for (int i = 0; i < player0Cards.Count; i++)
        {
            var player0CardResources = player0Cards[i].card.GetCardRecord().Resources;
            var player1CardResources = player1Cards[i].card.GetCardRecord().Resources;

            foreach (var cardResourceRecord in player0CardResources.Values)
            {
                if (!player1CardResources.ContainsKey(cardResourceRecord.ResourceId))
                {
                    // Lose life
                    PlayerControllers[1].LoseLife();
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        foreach (PlayerController controller in PlayerControllers.Values)
        {
            controller.playedCardDeck.DiscardAllCards();
        }
        
        currentPlayerIndex = 0;
        StartPlayerTurn();
    }

    #endregion

    #region End Game

    private void EndGame()
    {
        ScreenManager.OpenScreen<LoseScreenPopupPresenter>();
    }

    #endregion
}


