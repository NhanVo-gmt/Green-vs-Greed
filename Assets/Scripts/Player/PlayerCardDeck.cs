using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCardDeck : CardDeck
{
    private List<CardSlot>   AvailableCardSlots = new();

    public override void DrawCard(CardRecord cardRecord)
    {
        foreach (CardSlot slot in CardSlots)
        {
            if (slot.CanGetCard())
            {
                slot.DrawCard(cardRecord);
                OnDrawCard?.Invoke(cardRecord);
                
                AvailableCardSlots.Add(slot);
                return;
            }
        }
    }

    #region Pick
    
    
    public override void SetPickState(bool state)
    {
        CanPick = state;

        if (player.isBot)
        {
            PickRandomCard();
        }
    }
    
    public override void PickCard(CardSlot cardSlot)
    {
        if (!CanPick) return;
        
        cardSlot.card.gameObject.SetActive(false);
        
        AvailableCardSlots.Remove(cardSlot);

        OnPickCard?.Invoke(cardSlot.card.GetCardRecord());
    }

    #endregion

    #region AI

    public void PickRandomCard()
    {
        if (AvailableCardSlots.Count <= 0) return;
        
        CardSlot slot = AvailableCardSlots[Random.Range(0, AvailableCardSlots.Count)];
        
        PickCard(slot);
    }

    #endregion
}
