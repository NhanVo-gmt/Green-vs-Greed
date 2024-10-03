using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControllerButton : ControllerButton
{
    private void OnMouseDown()
    {
        OnClick();
    }

    private void OnMouseUp()
    {
        OnExit();
    }

    private void OnMouseExit()
    {
        OnExit();
    }

}
