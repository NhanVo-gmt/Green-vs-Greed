using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;

public class PlayedCardDeck : CardDeck
{
    protected override void Awake()
    {
        base.Awake();
        
        foreach (var slot in CardSlots)
        {
            slot.SetPickState(false);
            slot.SetHoverState(false);
            slot.SetViewState(true);
        }
    }

    public override void DrawSlot(CardSlot slot, CardRecord record)
    {
        base.DrawSlot(slot, record);
        slot.SetViewState(true);
    }

    public CardSlot GetCardSlot(int index)
    {
        return CardSlots[index];
    }

    public void DiscardAllCards()
    {
        foreach (CardSlot slot in CardSlots)
        {
            slot.card.Use();
            slot.DisableVisual();
        }
    }
}
