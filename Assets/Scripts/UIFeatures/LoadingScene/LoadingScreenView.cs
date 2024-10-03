using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameFoundation.Scripts.Utilities.Extension;
using GameFoundationBridge;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenView : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private bool isAnimate = false;

    public Action OnFinishShow;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_OnSceneLoaded;
    }

    private void SceneManager_OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Hide();
    }

    public async UniTask Show()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0f, 0f));
        sequence.Append(canvasGroup.DOFade(1f, 1f).SetEase(Ease.Linear));
        sequence.AppendInterval(0.5f);
        
        await sequence.AsyncWaitForCompletion();
    }

    async void Hide()
    {
        await canvasGroup.DOFade(0f, 1f).SetEase(Ease.Linear).AsyncWaitForCompletion();
    }
}
