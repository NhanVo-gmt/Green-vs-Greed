using System;
using System.Collections;
using System.Collections.Generic;
using GameFoundation.Scripts.Utilities.Extension;
using UnityEngine;
using UserData.Controller;
using Zenject;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterState currentState;
    [SerializeField] private Controller controller;
    [SerializeField] private float      speed;

    private CharacterVisual visual; 
    
    public Action<CharacterState> OnChangeState;

    [Inject] private LevelManager levelManager;
    

    private void Awake()
    {
        this.GetCurrentContainer().Inject(this);
        visual = GetComponentInChildren<CharacterVisual>();
        
    }

    private void Update()
    {
        if (levelManager.IsGameOver) return;
        
        if (controller.horizontalInput != 0 || controller.verticalInput != 0)
        {
            ChangeState(CharacterState.Move);
        }
        else ChangeState(CharacterState.Idle);
    }

    private void FixedUpdate()
    {
        if (levelManager.IsGameOver) return;
        
        Move();
    }

    void Move()
    {
        transform.position += (Vector3)(controller.horizontalInput * Vector2.right + controller.verticalInput * Vector2.up) * speed * Time.fixedDeltaTime;
        visual.ChangeDirection(controller.horizontalInput);
    }

    void ChangeState(CharacterState newState)
    {
        if (currentState == newState) return;
        
        currentState = newState;
        visual.UpdateAnim(currentState);
    }
}

public enum CharacterState
{
    Idle,
    Move
}
