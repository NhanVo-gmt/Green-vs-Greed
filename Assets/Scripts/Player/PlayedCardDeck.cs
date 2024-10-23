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

    public override void DrawCard(CardRecord cardRecord)
    {
        if (cardRecord.UseImmediately)
        {
            // Use
            GameManager.Instance.UseEffect(cardRecord.Effect);
            return;
        }
        
        base.DrawCard(cardRecord);
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
    
}
