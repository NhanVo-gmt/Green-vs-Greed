
using System;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{
    [Header("End Turn")]
    public Button endTurnBtn;
    
    [Header("Resource")]
    public Transform resourceContent;
    public ResourceItemUI resourceItemUIPrefab;
    
    private PlayerData                           playerData;
    private PlayerRecord                         playerRecord;
    private Dictionary<Resource, ResourceItemUI> resourceItemUis = new();

    public Action OnEndTurnButtonClicked;

    public void BindData(PlayerData playerData)
    {
        this.playerData   = playerData;
        SpawnResourceUI();
        
        endTurnBtn.onClick.AddListener(EndTurn);
        playerData.OnUpdateResource += UpdateUI;
    }

    private void OnDestroy()
    {
        endTurnBtn.onClick.RemoveListener(EndTurn);
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

    #region End Turn

    public void SetStateEndTurn(bool state)
    {
        endTurnBtn.gameObject.SetActive(state);
    }

    public void EndTurn()
    {
        OnEndTurnButtonClicked?.Invoke();
    }

    #endregion
}
