using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;

[Serializable]
public class PlayerSite
{
    public Resource     Resource;
    public GameObject[] Sites;
    public int          CurrentLevel = 0;
    
    public void UpdateLevel(int newLevel)
    {
        CurrentLevel = newLevel;
        ChangeSite();
    }
    
    public void ChangeSite()
    {
        for (int i = 0; i < Sites.Length; i++)
        {
            Sites[i].SetActive(i == CurrentLevel - 1);
        }
    }
}

public class PlayerConstruction : MonoBehaviour
{
    [Header("Site")]
    public PlayerSite[] sites;
    
    private PlayerData                          playerData;
    private Dictionary<Resource, PlayerUpgrade> playerUpgrades = new();
    

    public void BindData(PlayerData playerData, Dictionary<Resource, PlayerUpgrade> playerUpgrades)
    {
        this.playerData     = playerData;
        this.playerUpgrades = playerUpgrades;
        
        RegisterEvents();
        CheckLevel();
    }

    void RegisterEvents()
    {
        playerData.OnUpdateResource += OnUpdateResource;
    }

    private void OnDestroy()
    {
        playerData.OnUpdateResource -= OnUpdateResource;
    }

    private void OnUpdateResource(Resource updatedResource, int value)
    {
        CheckLevel();
    }

    void CheckLevel()
    {
        foreach (var site in sites)
        {
            if (!playerUpgrades.ContainsKey(site.Resource)) continue;
            
            var upgrade = playerUpgrades[site.Resource];
            for (int i = 0; i < upgrade.Requirements.Count; i++)
            {
                if (playerData.resources[upgrade.ResourceUpgrade] <= upgrade.Requirements[i].ResourceRequirementValue)
                {
                    site.UpdateLevel(i + 1);
                    break;
                }
            }
        }
    }

    

    
}
