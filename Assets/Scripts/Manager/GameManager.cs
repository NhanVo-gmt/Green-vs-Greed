using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using GameFoundation.Scripts.Utilities.Extension;
using Sirenix.OdinInspector;
using UnityEngine;
using UserData.Controller;
using Zenject;

public class GameManager : SingletonObject<GameManager>
{
    [SerializeField] private int numberPlayers;

    [Header("Debug")]
    [SerializeField] private int currentPlayerIndex = 0;

    private Dictionary<int, PlayerController> PlayerControllers = new();

    [Inject] private CardManager CardManager;

    private void Start()
    {
        this.GetCurrentContainer().Inject(this);
        
        FindAllPlayers();
        
        StartPlayerTurn();
    }
    
    void FindAllPlayers()
    {
        numberPlayers = 0;
        foreach (var player in GameObject.FindObjectsOfType<PlayerController>(true))
        {
            PlayerControllers.Add(player.playerIndex, player);
            numberPlayers++;
        }
    }

    void NextPlayerTurn()
    {
        currentPlayerIndex = currentPlayerIndex >= numberPlayers - 1 ? 0 : currentPlayerIndex++;
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
