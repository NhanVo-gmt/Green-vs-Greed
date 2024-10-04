using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Card card;

    public void DrawCard(CardRecord cardRecord)
    {
        this.card.BindData(cardRecord);
        this.card.gameObject.SetActive(true);
    }

    public bool CanGetCard()
    {
        return !this.card.HasCard();
    }

    public void UseCard()
    {
        this.card.Use();
        this.card.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Use Card");
        UseCard();
    }
}
