using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;

public class PlayedCardDeck : CardDeck
{
    public bool canView = true;

    protected override void InitSlot()
    {
        foreach (var slot in CardSlots)
        {
            slot.SetPickState(true);
            slot.SetHoverState(false);
            slot.SetViewState(canView);
        }
    }

    public override void DrawCard(CardRecord cardRecord, Transform target)
    {
        if (cardRecord.UseImmediately)
        {
            // Use
            GameManager.Instance.UseEffect(cardRecord.Effect);
            return;
        }
        
        base.DrawCard(cardRecord, target);
    }

    public override void DrawSlot(CardSlot slot, CardRecord record, Transform target)
    {
        base.DrawSlot(slot, record, target);
        slot.SetViewState(canView);
    }

    public CardSlot GetCardSlot(int index)
    {
        return CardSlots[index];
    }

    #region Pick

    public override void PickCard(CardSlot cardSlot)
    {
        if (!CanPickCard()) return;
        
        occupiedSlot--;

        CardRecord pickCard = cardSlot.card.GetCardRecord();
        
        cardSlot.DisableVisual();
        cardSlot.card.Pick();
        
        player.playerCardDeck.DrawCard(pickCard, cardSlot.transform);
        OnPickCard?.Invoke(pickCard);
    }

    public override bool CanPickCard()
    {
        return isPickState && player.CanPickCard(GetCardDeckType());
    }

    #endregion

    #region Effect
    
    public void View()
    {
        foreach (var slot in CardSlots)
        {
            slot.SetViewState(true);
        }
    }

    public void SetBlindState(bool state)
    {
        canView = !state;
        
        foreach (var slot in CardSlots)
        {
            slot.SetViewState(canView);
        }
    }

    #endregion
    
}
