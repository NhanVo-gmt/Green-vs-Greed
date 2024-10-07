using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayUI : MonoBehaviour
{
    [Header("Tab")]
    [SerializeField] private GameObject[] tabs;
    
    [Header("Button")]
    [SerializeField] private Button nextBtn;
    [SerializeField] private TextMeshProUGUI btnText;

    public Action OnClose;

    public int currentIndex = 0;

    private void Awake()
    {
        nextBtn.onClick.RemoveAllListeners();
        nextBtn.onClick.AddListener(NextTab);
    }

    public void Show()
    {
        currentIndex = 0;
        gameObject.SetActive(true);
        tabs[currentIndex].SetActive(true);
    }

    public void NextTab()
    {
        tabs[currentIndex].SetActive(false);

        currentIndex++;
        if (currentIndex == tabs.Length - 1)
        {
            tabs[currentIndex].SetActive(true);
        }
        else if (currentIndex >= tabs.Length)
        {
            gameObject.SetActive(false);
            OnClose?.Invoke();
        }
    }
}
