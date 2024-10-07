using System;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    [Header("Card Index")]
    public List<CardSlot> CardSlots = new();

    public Action<CardRecord> OnDrawCard;
    public Action<CardRecord> OnPickCard;

    public bool CanPick { get; protected set; } = false;
    
    protected PlayerController player;

    protected virtual void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        
        foreach (var slot in CardSlots)
        {
            slot.SetPickState(!player.isBot);
            slot.SetHoverState(!player.isBot);
            slot.DisableVisual();
            
            slot.OnPickCard += PickCard;
        }
    }

    public virtual void DrawCard(CardRecord cardRecord)
    {
        foreach (CardSlot slot in CardSlots)
        {
            if (slot.CanGetCard())
            {
                DrawSlot(slot, cardRecord);
                
                return;
            }
        }
    }

    #region Draw

    public virtual void DrawSlot(CardSlot slot, CardRecord record)
    {
        slot.DrawCard(record);
        slot.SetViewState(!player.isBot);
        
        OnDrawCard?.Invoke(record);
    }

    #endregion

    #region Pick
    
    
    public virtual void SetPickState(bool state)
    {
        CanPick = state;
    }
    
    public virtual void PickCard(CardSlot cardSlot)
    {
        if (!CanPick) return;
        
        cardSlot.DisableVisual();
        
        OnPickCard?.Invoke(cardSlot.card.GetCardRecord());
    }

    #endregion

    #region Discard
    
    public virtual void DiscardAllCards()
    {
        foreach (CardSlot slot in CardSlots)
        {
            slot.card.Use();
            slot.DisableVisual();
        }
    }

    #endregion
}