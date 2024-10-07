using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI turnText;

    public void SetTurn(int player)
    {
        if (player == 0)
        {
            turnText.text = $"Bot Turn";
        }
        else turnText.text = $"Player Turn";
    }

    public void EndTurn()
    {
        turnText.text = "Checking...";
    }
}
