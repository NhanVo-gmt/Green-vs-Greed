using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCardDeck : CardDeck
{
    private List<CardSlot>   AvailableCardSlots = new();


    public override void DrawSlot(CardSlot slot, CardRecord record)
    {
        base.DrawSlot(slot, record);
        
        slot.SetViewState(!player.isBot);
        
        AvailableCardSlots.Add(slot);
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
        
        OnPickCard?.Invoke(cardSlot.card.GetCardRecord());
        
        cardSlot.DisableVisual();
        cardSlot.card.Pick();
        AvailableCardSlots.Remove(cardSlot);

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
