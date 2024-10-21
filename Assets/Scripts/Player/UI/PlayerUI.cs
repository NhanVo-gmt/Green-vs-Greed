
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{
    [Header("Resource")]
    public Transform resourceContent;
    public ResourceItemUI resourceItemUIPrefab;
    
    private PlayerData                           playerData;
    private PlayerRecord                         playerRecord;
    private Dictionary<Resource, ResourceItemUI> resourceItemUis = new();

    public void BindData(PlayerData playerData)
    {
        this.playerData   = playerData;
        SpawnResourceUI();
        
        playerData.OnUpdateResource += UpdateUI;
    }

    private void OnDestroy()
    {
        playerData.OnUpdateResource -= UpdateUI;
    }

    private void SpawnResourceUI()
    {
        foreach (var resource in playerData.record.Resources.Values)
        {
            var resourceItemUI = Instantiate(resourceItemUIPrefab, resourceContent);
            resourceItemUI.BindData(resource);
            
            resourceItemUis.Add(resource.ResourceId, resourceItemUI);
        }
    }

    
    private void UpdateUI(Resource type, int newAmount)
    {
        resourceItemUis[type].UpdateAmountUI(newAmount);
    }
}
