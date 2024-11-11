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

    public void Play(string animName)
    {
        anim.Rebind();
        anim.Play(animName, 0, 0f);
    }

}
