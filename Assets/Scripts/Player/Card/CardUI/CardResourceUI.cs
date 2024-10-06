using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.Utilities.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserData.Controller;
using Zenject;

public class CardResourceUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI signText;
    [SerializeField] private TextMeshProUGUI numText;
    [SerializeField] private Image           image;

    [Inject] private CardManager CardManager;

    private CardResourceRecord CardResourceRecord;

    private void Awake()
    {
        this.GetCurrentContainer().Inject(this);
    }

    public void BindData(CardResourceRecord resourceRecord)
    {
        this.CardResourceRecord = resourceRecord;

        if (resourceRecord.ResourceAmount >= 0)
        {
            signText.text = "+";
        } else signText.text = "-";

        numText.text = Mathf.Abs(resourceRecord.ResourceAmount).ToString();

        CardManager.GetIcon(resourceRecord.ResourceId.ToString()).ContinueWith((sprite) =>
        {
            image.sprite = sprite;
        }).Forget();
        
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
