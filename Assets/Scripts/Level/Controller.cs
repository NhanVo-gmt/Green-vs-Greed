using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserData.Controller;
using Zenject;

public class Controller : MonoBehaviour
{
    [SerializeField] private List<ControllerButtonWithDirection> controllerButtons;

    public float horizontalInput { get; private set; } = 0;
    public float verticalInput   { get; private set; } = 0;

    [Inject] private LevelManager levelManager;
    
    private void Awake()
    {
        
        
        foreach (var controllerButton in controllerButtons)
        {
            controllerButton.button.OnClickButton += () =>
            {
                Move(controllerButton);
            };
            controllerButton.button.OnExitButton += () =>
            {
                Stop(controllerButton);
            };
        }
    }
    
    private void Move(ControllerButtonWithDirection button)
    {
        switch (button.direction)
        {
            case ButtonDirection.Up:
                verticalInput = 1f;
                break;
            case ButtonDirection.Down:
                verticalInput = -1f;
                break;
            case ButtonDirection.Left:
                horizontalInput = -1f;
                break;
            case ButtonDirection.Right:
                horizontalInput = 1f;
                break;
        }
    }
    
    private void Stop(ControllerButtonWithDirection button)
    {
        switch (button.direction)
        {
            case ButtonDirection.Up:
                if (verticalInput > 0f) verticalInput = 0f;
                break;
            case ButtonDirection.Down:
                if (verticalInput < 0f) verticalInput = 0f;
                break;
            case ButtonDirection.Left:
                if (horizontalInput < 0f) horizontalInput = 0f;
                break;
            case ButtonDirection.Right:
                if (horizontalInput > 0f) horizontalInput = 0f;
                break;
        }
    }
}

[Serializable]
public class ControllerButtonWithDirection
{
    public ControllerButton button;
    public ButtonDirection  direction;
}

public enum ButtonDirection
{
    Up,
    Down,
    Left,
    Right
}
