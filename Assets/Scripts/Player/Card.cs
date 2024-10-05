using Blueprints;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UserData.Controller;
using Zenject;

public class Card : MonoBehaviour
{
    [SerializeField] private TextMeshPro    title;
    [SerializeField] private SpriteRenderer card;
    [SerializeField] private SpriteRenderer image;

    private CardRecord CardRecord;

    [Inject] private CardManager CardManager;

    public void BindData(CardRecord cardRecord)
    {
        this.CardRecord = cardRecord;

        title.text = cardRecord.Name;
        CardManager.GetCardImage(cardRecord.PlayerType).ContinueWith((sprite) => card.sprite = sprite).Forget();
        CardManager.GetIcon(cardRecord.Image).ContinueWith((sprite) => image.sprite = sprite).Forget();
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
