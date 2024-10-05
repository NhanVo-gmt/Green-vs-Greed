using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using GameFoundation.Scripts.Utilities.Extension;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UserData.Controller;
using Zenject;

public class GameManager : SingletonObject<GameManager>
{
    [Header("Player")]
    [SerializeField] private int numberPlayers;
    

    [Header("Debug")]
    [SerializeField] private int currentPlayerIndex = 0;

    private Dictionary<int, PlayerController> PlayerControllers = new();

    [Inject] private CardManager CardManager;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        this.GetCurrentContainer().Inject(this);
        
        FindAllPlayers();
        DrawAllCards();
        StartPlayerTurn();
    }

    private void OnDestroy()
    {
        foreach (var player in PlayerControllers.Values)
        {
            player.OnFinishTurn -= NextPlayerTurn;
        }
        PlayerControllers.Clear();
    }

    private void DrawAllCards()
    {
        foreach (var cardRecord in CardManager.GetCards(PlayerType.Environment))
        {
            PlayerControllers[0].DrawCard(cardRecord);
        }
        
        foreach (var cardRecord in CardManager.GetCards(PlayerType.Corporation))
        {
            PlayerControllers[1].DrawCard(cardRecord);
        }
    }

    void FindAllPlayers()
    {
        numberPlayers = 0;
        foreach (var player in GameObject.FindObjectsOfType<PlayerController>(true))
        {
            player.OnFinishTurn += NextPlayerTurn;
            
            PlayerControllers.Add(player.playerIndex, player);
            numberPlayers++;
        }
    }

    void NextPlayerTurn()
    {
        int newIndex = currentPlayerIndex >= numberPlayers - 1 ? 0 : currentPlayerIndex + 1;
        Debug.Log($"[Game Manager]: Change Player from {currentPlayerIndex} to {newIndex}");

        currentPlayerIndex = newIndex;
        StartPlayerTurn();
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

}
