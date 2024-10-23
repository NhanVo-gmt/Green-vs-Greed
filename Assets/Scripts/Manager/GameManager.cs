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
using Watermelon;
using Zenject;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Player")]
    [SerializeField] private int numberPlayers;

    [Header("UI")]
    [SerializeField] private GameUI gameUI;

    [Header("Time")]
    [SerializeField] private float waitTimeBeforeChecking = 0.5f;
    [SerializeField] private float waitTimeAfterChecking = 1f;
    
    [Header("Debug")]
    [SerializeField] private int currentPlayerIndex = -1;

    private Dictionary<int, Player> PlayerControllers = new();

    [Inject] private PlayerManager  PlayerManager;
    [Inject] private CardManager    CardManager;
    [Inject] private IScreenManager ScreenManager;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        this.GetCurrentContainer().Inject(this);
        FindAllPlayers();
        gameUI.OnCloseHowToPlayScreen += StartGame;
    }

    void StartGame()
    {
        DrawAllCards();
    }
    
    void FindAllPlayers()
    {
        numberPlayers = 0;
        foreach (var player in GameObject.FindObjectsOfType<Player>(true))
        {
            player.BindData(PlayerManager.GetPlayerRecord(player.playerIndex));
            player.OnShuffle        += Shuffle;
            player.OnFinishTurn     += NextPlayerTurn;
            player.playerData.OnDie += EndGame;
            
            PlayerControllers.Add(player.playerIndex, player);
            numberPlayers++;
        }
    }
    

    private void OnDestroy()
    {
        gameUI.OnCloseHowToPlayScreen -= StartGame;
        
        foreach (var player in PlayerControllers.Values)
        {
            player.OnShuffle        -= Shuffle;
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
            DrawAllCards(player);
        }

        NextPlayerTurn();
    }

    public void DrawAllCards(Player player)
    {
        foreach (var cardRecord in CardManager.GetCards(player.playerType))
        {
            player.DrawCard(cardRecord);
        }
    }
    
    public void DrawRandomAllCards(Player player, int num)
    {
        var cards = CardManager.GetCards(player.playerType);
        for (int i = 0; i < num; i++)
        {
            player.DrawCard(cards[Random.Range(0, cards.Count)]);
        }
    }

    void NextPlayerTurn(Player player)
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
    
    void StartPlayerTurn()
    {
        gameUI.SetTurn(currentPlayerIndex);
        PlayerControllers[currentPlayerIndex].StartTurn();
    }

    #region Draw Card

    void DrawCardEndTurn()
    {
        if (currentPlayerIndex == -1) return;

        int index = currentPlayerIndex;
        Tween.DelayedCall(0.5f, () =>
        {
            PlayerControllers[index]
                    .DrawCard(CardManager.DrawRandomCard(PlayerControllers[index].playerType));
        });
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
    
    #endregion

    #region Shuffle

    public void UseEffect(EffectType type)
    {
        switch (type)
        {
            case EffectType.Shuffle:
                Shuffle(1);
                break;
        }
    }

    public void Shuffle()
    {
        Shuffle(0);
    }

    public void Shuffle(int usingCard)
    {
        Player currentPlayer = PlayerControllers[currentPlayerIndex];

        int numCard = currentPlayer.GetCurrentNumberPlayerDeck() + usingCard;
        currentPlayer.DiscardAllCards();
        
        DrawRandomAllCards(currentPlayer, numCard);
    }

    #endregion

    #region End Turn

    public void EndBothTurn()
    {
        StartCoroutine(EndBothTurnCoroutine());
    }


    IEnumerator EndBothTurnCoroutine()
    {
        Debug.Log($"[Game Manager]: Check End Turn");
        gameUI.EndTurn();
        
        yield return new WaitForSeconds(waitTimeBeforeChecking);
        
        List<CardSlot> player0Cards = PlayerControllers[0].playedCardDeck.CardSlots;
        List<CardSlot> player1Cards = PlayerControllers[1].playedCardDeck.CardSlots;
        for (int i = 0; i < player0Cards.Count; i++)
        {
            var player0CardResources = player0Cards[i].card.GetCardRecord().Resources;
            var player1CardResources = player1Cards[i].card.GetCardRecord().Resources;

            foreach (var cardResourceRecord in player0CardResources.Values)
            {
                PlayerControllers[1].ChangeResourceAmount(cardResourceRecord.ResourceId, cardResourceRecord.ResourceAmount);
            }
            
            foreach (var cardResourceRecord in player1CardResources.Values)
            {
                PlayerControllers[1].ChangeResourceAmount(cardResourceRecord.ResourceId, cardResourceRecord.ResourceAmount);
            }

            yield return null;
        }

        yield return new WaitForSeconds(waitTimeAfterChecking);

        foreach (Player controller in PlayerControllers.Values)
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


