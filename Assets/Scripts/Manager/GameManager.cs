using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int numberPlayers;

    [Header("Debug")]
    [SerializeField] private int currentPlayerIndex = 0;

    private Dictionary<int, PlayerController> PlayerControllers = new();

    private void Awake()
    {
        FindAllPlayers();
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


    private void Start()
    {
        StartPlayerTurn();
    }

    void NextPlayerTurn()
    {
        currentPlayerIndex = currentPlayerIndex >= numberPlayers - 1 ? 0 : currentPlayerIndex++;
    }

    void StartPlayerTurn()
    {
        PlayerControllers[currentPlayerIndex].StartTurn();
    }
}
