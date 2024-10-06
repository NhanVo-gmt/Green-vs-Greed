
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image[] healthUI;

    private PlayerData PlayerData;

    public void BindData(PlayerData playerData)
    {
        this.PlayerData = playerData;
        
        playerData.OnLoseLife += UpdateUI;
    }

    private void OnDestroy()
    {
        PlayerData.OnLoseLife -= UpdateUI;
    }


    private void UpdateUI(int newHealth)
    {
        for (int i = 0; i < newHealth; i++)
        {
            healthUI[i].enabled = true;
        }

        for (int i = newHealth; i < healthUI.Length; i++)
        {
            healthUI[i].enabled = false;
        }
    }
}
