using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerDestroyed;

    //public int maxHealth = 10;
    //public int currentHealth;

    void Start()
    {
        //currentHealth = maxHealth;
    }

    void OnDestroy()
    {
        OnPlayerDestroyed?.Invoke();
    }

    //public void TakeDamage(int amount)
    //{
    //    currentHealth -= amount;

    //    if (currentHealth <= 0)
    //    {
    //        //Disable player movement and drop items if any
    //    }
    //}

}
