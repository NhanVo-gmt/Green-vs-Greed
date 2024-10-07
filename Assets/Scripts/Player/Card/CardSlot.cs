using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
using GameFoundation.Scripts.Utilities.Extension;
using UnityEngine;
using UnityEngine.EventSystems;
using Watermelon;
using Zenject;

public class CardSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Card card;

    [Header("Anim")]
    public float hoverY = 1f;

    public float hoverTime = 0.2f;

    public Action<CardSlot> OnPickCard;

    public bool CanPick { get; private set; } = true;
    public bool CanHover { get; private set; } = true;

    private Animator anim;
    private Vector3  startPos;

    [Inject] private IScreenManager screenManager; 
        
    private void Awake()
    {
        this.GetCurrentContainer().Inject(this);
        
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
        if (!card.HasCard()) return;
        
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            screenManager.OpenScreen<CardDetailsPopupPresenter, CardRecord>(this.card.GetCardRecord());
        }
        else
        {
            if (!CanPick) return;

            PickCard();
        }
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CanHover) return;
        transform.DOMoveY(startPos.y + hoverY, hoverTime, 0);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CanHover) return;
        transform.DOMoveY(startPos.y, hoverTime, 0);
    }

    #region Set Methods

    public void SetPickState(bool state)
    {
        CanPick = state;
    }
    
    public void SetHoverState(bool state)
    {
        CanHover = state;
    }

    #endregion

}
