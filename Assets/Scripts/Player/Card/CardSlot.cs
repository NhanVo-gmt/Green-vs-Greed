using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using UnityEngine.EventSystems;
using Watermelon;

public class CardSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Card card;

    [Header("Anim")]
    public float hoverY = 1f;

    public float hoverTime = 0.2f;

    public Action<CardSlot> OnPickCard;

    public bool CanPick { get; private set; } = true;

    private Animator anim;
    private Vector3  startPos;

    private void Awake()
    {
        anim     = GetComponent<Animator>();
        startPos = transform.position;
    }


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
        if (!CanPick || !card.gameObject.activeSelf) return;

        PickCard();
    }

    public void SetPickState(bool state)
    {
        CanPick = state;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOMoveY(startPos.y + hoverY, hoverTime, 0);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOMoveY(startPos.y, hoverTime, 0);
    }

}
