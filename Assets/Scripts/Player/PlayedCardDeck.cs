using System.Collections;
using System.Collections.Generic;
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
        }
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
