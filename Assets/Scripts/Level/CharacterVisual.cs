using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Animator       anim;

    private void Awake()
    {
        anim   = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void ChangeDirection(float horizontalInput)
    {
        if (horizontalInput == 0) return;

        else if (horizontalInput < 0)
        {
            sprite.flipX = true;
        }
        else sprite.flipX = false;
    }

    public void UpdateAnim(CharacterState newState)
    {
        switch (newState)
        {
            case CharacterState.Idle:
                anim.Play("Idle");
                break;
            case CharacterState.Move:
                anim.Play("Move");
                break;
        }
    }
}
