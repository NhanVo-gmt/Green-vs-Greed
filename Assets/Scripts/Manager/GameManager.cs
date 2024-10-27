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
            player.OnShuffle        += Draw;
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
            player.OnShuffle        -= Draw;
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

        StartPlayerTurn();
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

    #region Draw Card

    IEnumerator DrawCardEndTurnCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        int index      = currentPlayerIndex;
        int numberDraw = 5 - PlayerControllers[index].playerCardDeck.GetCurrentNumberCards();
        
        for (int i = 0; i < numberDraw; i++)
        {
            PlayerControllers[index]
                .DrawCard(CardManager.DrawRandomCard(PlayerControllers[index].playerType));
            yield return null;
        }
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

    #region Effect

    public void UseEffect(EffectType type)
    {
        switch (type)
        {
            case EffectType.Draw:
                Draw();
                break;
            case EffectType.Blind:
                Blind();
                break;
        }
    }

    public void Draw()
    {
        Player currentPlayer = PlayerControllers[currentPlayerIndex];
        
        DrawRandomAllCards(currentPlayer, 1);
    }
    
    public void Blind()
    {
        PlayerControllers[currentPlayerIndex].Blind();
    }


    #endregion

    #region End Turn

    void NextPlayerTurn(Player player)
    {
        if (player.playerIndex != currentPlayerIndex) return;
        
        // Check coroutine
        StartCoroutine(EndCoroutine());
    }
    
    void StartPlayerTurn()
    {
        int newIndex = currentPlayerIndex >= numberPlayers - 1 ? 0 : currentPlayerIndex + 1;
        Debug.Log($"[Game Manager]: Change Player from {currentPlayerIndex} to {newIndex}");

        currentPlayerIndex = newIndex;
        
        gameUI.SetTurn(currentPlayerIndex);
        PlayerControllers[currentPlayerIndex].StartTurn();
    }

    IEnumerator EndCoroutine()
    {
        Debug.Log($"[Game Manager]: Draw Card End Turn");
        yield return DrawCardEndTurnCoroutine();
        
        Debug.Log($"[Game Manager]: Check End Turn");
        gameUI.EndTurn();
        
        yield return new WaitForSeconds(waitTimeBeforeChecking);
        
        List<CardSlot> playerCards = PlayerControllers[currentPlayerIndex].playedCardDeck.CardSlots;
        for (int i = 0; i < playerCards.Count; i++)
        {
            if (!playerCards[i].card.HasCard()) continue;
            var playerCardResources = playerCards[i].card.GetCardRecord().Resources;

            foreach (var cardResourceRecord in playerCardResources.Values)
            {
                if (cardResourceRecord.ResourceId == Resource.Money)
                {
                    // Money
                    PlayerControllers[0].ChangeResourceAmount(cardResourceRecord.ResourceId, cardResourceRecord.ResourceAmount);
                }
                else
                {
                    // Wood, Water
                    PlayerControllers[1].ChangeResourceAmount(cardResourceRecord.ResourceId, cardResourceRecord.ResourceAmount);
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(waitTimeAfterChecking);

        PlayerControllers[currentPlayerIndex].playedCardDeck.DiscardAllCards();
        
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


