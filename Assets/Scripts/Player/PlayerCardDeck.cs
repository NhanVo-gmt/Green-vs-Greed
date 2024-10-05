using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCardDeck : MonoBehaviour
{
    [Header("Card Index")]
    public List<CardSlot> CardSlots = new();

    public Action<CardRecord> OnDrawCard;
    public Action<CardRecord> OnPickCard;

    public bool CanPick { get; private set; } = false;

    private PlayerController player;
    private List<CardSlot>   AvailableCardSlots = new();

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        
        foreach (var slot in CardSlots)
        {
            slot.SetPickState(!player.isBot);
            slot.OnPickCard += PickCard;
        }
    }
    

    public void DrawCard(CardRecord cardRecord)
    {
        foreach (CardSlot slot in CardSlots)
        {
            if (slot.CanGetCard())
            {
                slot.DrawCard(cardRecord);
                OnDrawCard?.Invoke(cardRecord);
                
                AvailableCardSlots.Add(slot);
                return;
            }
        }
    }

    #region Pick
    
    
    public void SetPickState(bool state)
    {
        CanPick = state;

        if (player.isBot)
        {
            PickRandomCard();
        }
    }
    
    public void PickCard(CardSlot cardSlot)
    {
        if (!CanPick) return;
        
        cardSlot.card.Use();
        cardSlot.card.gameObject.SetActive(false);
        
        AvailableCardSlots.Remove(cardSlot);

        OnPickCard?.Invoke(cardSlot.card.GetCardRecord());
    }

    #endregion

    #region AI

    public void PickRandomCard()
    {
        CardSlot slot = AvailableCardSlots[Random.Range(0, AvailableCardSlots.Count)];
        
        PickCard(slot);
    }

    #endregion
}
