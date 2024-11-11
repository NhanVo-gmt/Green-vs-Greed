using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f);
        }
        
        gameObject.SetActive(true);
        
        Destroy(gameObject, 1f);
    }

}
