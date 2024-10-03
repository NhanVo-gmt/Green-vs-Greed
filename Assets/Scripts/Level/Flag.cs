using System;
using System.Collections;
using System.Collections.Generic;
using GameFoundation.Scripts.Utilities.Extension;
using UnityEngine;
using UserData.Controller;
using Zenject;

public class Flag : MonoBehaviour
{
    [Inject] private LevelManager levelManager;
    private void Start()
    {
        this.GetCurrentContainer().Inject(this);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Character>())
        {
            levelManager.FinishLevel();
        }
    }
}
