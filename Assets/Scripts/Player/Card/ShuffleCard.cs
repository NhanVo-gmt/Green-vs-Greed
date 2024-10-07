using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShuffleCard : MonoBehaviour, IPointerDownHandler
{
    public static Action OnShuffle;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Shuffle");
        OnShuffle?.Invoke();
    }

}
