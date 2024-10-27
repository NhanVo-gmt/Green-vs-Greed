using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;

public class PlayerConstruction : MonoBehaviour
{
    [Header("Site")]
    [SerializeField] private GameObject[] sites;

    [Header("Debug")]
    public int currentLevel = 0;
    
    
    private PlayerData          playerData;
    private List<PlayerUpgrade> playerUpgrades;
    
    public void BindData(PlayerData playerData, List<PlayerUpgrade> playerUpgrades)
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
        for (int i = 0; i < playerUpgrades.Count; i++)
        {
            PlayerUpgrade upgrade = playerUpgrades[i];

            foreach (var resource in upgrade.Requirements.Values)
            {
                if (resource.ResourceRequirementValue > playerData.resources[resource.ResourceRequirement])
                {
                    UpdateLevel(i + 1);
                    return;
                }
            }
        }
    }

    void UpdateLevel(int newLevel)
    {
        currentLevel = newLevel;
        ChangeSite();
    }

    void ChangeSite()
    {
        for (int i = 0; i < sites.Length; i++)
        {
            sites[i].SetActive(i == currentLevel - 1);
        }
    }
}
