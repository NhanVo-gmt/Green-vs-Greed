using System;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;

public enum CardDeckType
{
    Hand,
    Played
}

public class CardDeck : MonoBehaviour
{
    [Header("Card Index")]
    public List<CardSlot> CardSlots = new();

    public int occupiedSlot = 0;

    public Action<CardRecord> OnDrawCard;
    public Action<CardRecord> OnPickCard;

    public bool isPickState { get; protected set; } = false;
    
    protected Player player;

    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();

        GameUI.OnCloseHowToPlayScreen += InitSlot;
    }

    private void OnDestroy()
    {
        GameUI.OnCloseHowToPlayScreen -= InitSlot;
    }

    protected virtual void InitSlot()
    {
        foreach (var slot in CardSlots)
        {
            slot.SetPickState(!player.isBot);
            slot.SetHoverState(player.showView);
            slot.DisableVisual();
            
            slot.OnPickCard += PickCard;
        }
    }

    public virtual void DrawCard(CardRecord cardRecord, Transform target)
    {
        foreach (CardSlot slot in CardSlots)
        {
            if (slot.CanGetCard())
            {
                DrawSlot(slot, cardRecord, target);
                return;
            }
        }
    }

    #region Draw

    public virtual void DrawSlot(CardSlot slot, CardRecord record, Transform target)
    {
        occupiedSlot++;
        
        slot.DrawCard(record, target);
        slot.SetViewState(!player.isBot);

        if (target.GetComponent<CardSlot>())
        {
            SoundManager.Instance.PlayOneShot(SoundType.PickCard);
        }
        else SoundManager.Instance.PlayOneShot(SoundType.DrawCard);
        
        OnDrawCard?.Invoke(record);
    }

    #endregion
    
    public bool IsEmpty()
    {
        return occupiedSlot == 0;
    }

    public bool IsFull()
    {
        return occupiedSlot == CardSlots.Count;
    }

    public virtual CardDeckType GetCardDeckType()
    {
        return CardDeckType.Played;
    }

    #region Pick
    
    
    public virtual void SetPickState(bool state)
    {
        isPickState = state;
    }

    public virtual bool CanPickCard()
    {
        return isPickState;
    }
    
    public virtual void PickCard(CardSlot cardSlot)
    {
        if (!CanPickCard()) return;
        
        occupiedSlot--;

        CardRecord pickCard = cardSlot.card.GetCardRecord();
        
        cardSlot.DisableVisual();
        cardSlot.card.Pick();
        
        OnPickCard?.Invoke(pickCard);
    }

    #endregion

    #region Discard
    
    public virtual void DiscardAllCards()
    {
        occupiedSlot = 0;
        foreach (CardSlot slot in CardSlots)
        {
            slot.card.Use();
            slot.DisableVisual();
        }
    }

    #endregion
}