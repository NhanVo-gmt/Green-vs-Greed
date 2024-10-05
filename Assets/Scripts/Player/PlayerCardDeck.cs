using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;

public class PlayerCardDeck : MonoBehaviour
{
    [Header("Card Index")]
    public List<CardSlot> CardSlots = new();

    public Action<CardRecord> OnDrawCard;
    public Action<CardRecord> OnPickCard;

    public bool CanPick { get; private set; } = false;

    private void Awake()
    {
        foreach (var slot in CardSlots)
        {
            slot.OnPickCard += PickCard;
        }
    }

    public void DrawCard(CardRecord cardRecord)
    {
        foreach (CardSlot slot in CardSlots)
        {
            if (slot.CanGetCard())
            {
                slot.DrawCard(cardRecord);
                OnDrawCard?.Invoke(cardRecord);
                return;
            }
        }
    }

    #region Pick
    
    
    public void SetPickState(bool state)
    {
        CanPick = state;
    }
    
    public void PickCard(CardSlot cardSlot)
    {
        if (!CanPick) return;
        
        cardSlot.card.Use();
        cardSlot.card.gameObject.SetActive(false);

        OnPickCard?.Invoke(cardSlot.card.GetCardRecord());
    }

    #endregion
}
