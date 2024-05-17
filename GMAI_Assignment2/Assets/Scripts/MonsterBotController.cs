using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBotController : MonoBehaviour
{
    public GameObject player;
    public Transform[] treasures;
    public Transform nest;

    public float interactDistance = 2f;
    public float approachDistance = 5f;

    public float speed { get; private set; } = 5f;

    void Start()
    {

    }

    void Update()
    {
        
    }
}
