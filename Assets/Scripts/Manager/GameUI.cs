using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI gameText;

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
