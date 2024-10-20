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

public class ResourceItemUI : MonoBehaviour
{
    public Image           image;
    public TextMeshProUGUI text;
    
    private ResourceRecord resourceRecord;

    [Inject] private MiscManager miscManager;

    public void BindData(ResourceRecord resourceRecord)
    {
        this.GetCurrentContainer().Inject(this);
        this.resourceRecord = resourceRecord;
        
        UpdateUI();
    }

    void UpdateUI()
    {
        miscManager.GetIcon(resourceRecord.ResourceId.ToString()).ContinueWith((sprite) =>
        {
            image.sprite = sprite;
        }).Forget();

        text.text = resourceRecord.ResourceAmount.ToString();
    }
}
