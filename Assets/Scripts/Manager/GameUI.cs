using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameUI : MonoBehaviour
{
    [Header("How To Play")]
    public HowToPlayUI howToPlayUI;
    
    [Header("In Game")]
    public TextMeshProUGUI gameText;

    public Action OnCloseHowToPlayScreen;

    private void Awake()
    {
        howToPlayUI.Show();

        howToPlayUI.OnClose += HowToPlayUI_CloseEvent;
    }

    private void OnDestroy()
    {
        howToPlayUI.OnClose -= HowToPlayUI_CloseEvent;
    }

    void HowToPlayUI_CloseEvent()
    {
        OnCloseHowToPlayScreen.Invoke();
    }

    public void SetTurn(int player)
    {
        if (player == 0)
        {
            gameText.text = $"Bot Turn";
        }
        else gameText.text = $"Player Turn";
    }

    public void SetText(string text)
    {
        gameText.text = text;
    }

    public void EndTurn()
    {
        gameText.text = "Checking...";
    }
}
