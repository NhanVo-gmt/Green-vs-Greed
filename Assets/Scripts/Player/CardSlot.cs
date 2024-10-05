using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerDownHandler
{
    public Card card;

    public Action<CardSlot> OnPickCard;

    public bool CanPick { get; private set; } = true;


    public void DrawCard(CardRecord cardRecord)
    {
        this.card.BindData(cardRecord);
        this.card.gameObject.SetActive(true);
    }

    public bool CanGetCard()
    {
        return !this.card.HasCard();
    }

    public void PickCard()
    {
        OnPickCard?.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanPick) return;

        PickCard();
    }

    public void SetPickState(bool state)
    {
        CanPick = state;
    }
}
