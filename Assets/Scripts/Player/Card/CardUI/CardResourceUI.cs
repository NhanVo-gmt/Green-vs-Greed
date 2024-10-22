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

    [Inject] private MiscManager miscManager;

    private ResourceRecord CardResourceRecord;

    private void Awake()
    {
        this.GetCurrentContainer().Inject(this);
    }

    public void BindData(ResourceRecord resourceRecord)
    {
        this.CardResourceRecord = resourceRecord;

        miscManager.GetIcon(resourceRecord.ResourceId.ToString()).ContinueWith((sprite) =>
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
