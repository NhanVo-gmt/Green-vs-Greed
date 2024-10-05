using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    
}

public class PlayerLife
{
    public int currentLife;

    void LoseLife()
    {
        currentLife--;
        if (currentLife <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        
    }
}
