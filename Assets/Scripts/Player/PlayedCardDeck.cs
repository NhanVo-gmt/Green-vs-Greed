using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedCardDeck : CardDeck
{
    public CardSlot GetCardSlot(int index)
    {
        return CardSlots[index];
    }

    public void DiscardAllCards()
    {
        foreach (CardSlot slot in CardSlots)
        {
            slot.card.Use();
            slot.card.gameObject.SetActive(false);
        }
    }
}
