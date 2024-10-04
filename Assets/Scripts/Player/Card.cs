using System.Collections;
using System.Collections.Generic;
using Blueprints;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer imageSprite;

    private CardRecord CardRecord;

    public void BindData(CardRecord cardRecord)
    {
        this.CardRecord = cardRecord;
        
        // imageSprite.sprite
    }

    public bool HasCard()
    {
        return CardRecord != null;
    }

    public void Use()
    {
        if (this.CardRecord == null) return;
        
        // Use

        CardRecord = null;
    }
}
