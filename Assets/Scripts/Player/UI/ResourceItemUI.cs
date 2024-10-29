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
using Watermelon;
using Zenject;

public class ResourceItemUI : MonoBehaviour
{
    public Image           image;
    public TextMeshProUGUI text;

    [Header("Add")]
    public Transform addedContent;
    public TextMeshProUGUI addedText;
    
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

    public void SpawnAddedAmountText(int amount)
    {
        TextMeshProUGUI spawnAddedText = Instantiate(addedText, addedContent);
        spawnAddedText.gameObject.SetActive(true);
        
        if (amount > 0)
        {
            spawnAddedText.text = WrapText("green", amount);
        }
        else
        {
            spawnAddedText.text = WrapText("red", amount);
        }
        
        Destroy(spawnAddedText, 1f);
    }

    public string WrapText(string color, int amount)
    {
        string text = amount > 0 ? $"+{amount}" : $"{amount}";
        return $"<color={color}>{text}</color>";
    }

    public void UpdateAmountUI(int newAmount)
    {
        text.text = newAmount.ToString();
    }
}
