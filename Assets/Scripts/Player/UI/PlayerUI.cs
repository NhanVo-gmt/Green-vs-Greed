
using System;
using Blueprints;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerUI : MonoBehaviour
{
    [Header("Health")]
    public Image[] healthUI;

    [Header("Resource")]
    public Transform resourceContent;
    public ResourceItemUI resourceItemUI;
    
    private PlayerData playerData;
    private PlayerRecord playerRecord;
    

    public void BindData(PlayerData playerData, PlayerRecord playerRecord)
    {
        this.playerData   = playerData;
        this.playerRecord = playerRecord;
        SpawnResourceUI();
        
        playerData.OnLoseLife += UpdateUI;
    }

    private void OnDestroy()
    {
        playerData.OnLoseLife -= UpdateUI;
    }

    private void SpawnResourceUI()
    {
        foreach (var resource in playerRecord.Resources.Values)
        {
            var resourceObject = Instantiate(resourceItemUI, resourceContent);
            resourceObject.BindData(resource);
        }
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
