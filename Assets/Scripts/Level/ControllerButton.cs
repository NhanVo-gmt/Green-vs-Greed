using System;
using System.Collections;
using System.Collections.Generic;
using GameFoundation.Scripts.Utilities.Extension;
using UnityEngine;
using UserData.Controller;
using Zenject;

public class ControllerButton : MonoBehaviour
{
    public Action OnClickButton;
    public Action OnExitButton;

    public Sprite normal;
    public Sprite pressed;

    private SpriteRenderer spriteRenderer;

    [Inject] protected LevelManager levelManager;

    protected virtual void Awake()
    {
        this.GetCurrentContainer().Inject(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Character>())
        {
            OnClick();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Character>())
        {
            OnExit();
        }
    }

    public void OnClick()
    {
        if (levelManager.IsGameOver) return;
        
        spriteRenderer.sprite = pressed;
        OnClickButton?.Invoke();
    }

    public void OnExit()
    {
        if (levelManager.IsGameOver) return;
        
        spriteRenderer.sprite = normal;
        OnExitButton?.Invoke();
    }
}
