using System;
using Blueprints;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UserData.Controller;
using Zenject;

public class Card : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshPro    title;
    [SerializeField] private SpriteRenderer card;
    [SerializeField] private SpriteRenderer image;

    [Header("UI Resource")]
    [SerializeField] private CardResourceUI[] resourceUis;

    private CardRecord CardRecord;

    [Inject] private CardManager CardManager;

    private void Awake()
    {
        
    }

    public void BindData(CardRecord cardRecord)
    {
        ResetResources();
        
        this.CardRecord = cardRecord;

        title.text = cardRecord.Name;
        CardManager.GetCardImage(cardRecord.PlayerType).ContinueWith((sprite) => card.sprite = sprite).Forget();
        CardManager.GetIcon(cardRecord.Image).ContinueWith((sprite) => image.sprite = sprite).Forget();

        int i = 0;
        foreach (var cardResource in cardRecord.Resources)
        {
            resourceUis[i].BindData(cardResource);
            i++;
        }
    }

    void ResetResources()
    {
        foreach (var resourceUI in resourceUis)
        {
            resourceUI.Hide();
        }
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

    public CardRecord GetCardRecord()
    {
        return CardRecord;
    }
}
