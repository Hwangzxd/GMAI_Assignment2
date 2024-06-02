using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    // A static event to signal when the player is destroyed
    public static event Action OnPlayerDestroyed;

    void Start()
    {

    }

    void OnDestroy()
    {
        // Invoke the OnPlayerDestroyed event if there are any subscribers
        OnPlayerDestroyed?.Invoke();
    }

}
