using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MonsterBotTasks : MonoBehaviour
{
    public float speed = 5f;
    public Transform[] treasures;
    public Text displayText;

    private Rigidbody rb;
    private NavMeshAgent navAgent;
    private MonsterBotController monsterBot;

    Transform target;
    GameObject player;

    private float stoppingDistance = 0f;
    public bool playerActive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        monsterBot = GetComponent<MonsterBotController>();
        player = GameObject.FindGameObjectWithTag("Player");
        treasures = monsterBot.treasures;
    }

    [Task]
    bool IsPlayerActive()
    {
        return playerActive;
    }

    [Task]
    bool SetPlayerActive(bool active)
    {
        playerActive = active;
        return true;
    }

    [Task]
    bool Idle()
    {
        displayText.text = "Idling";
        return true;
    }

    [Task]
    bool Display(string text)
    {
        if (displayText != null)
        {
            displayText.text = text;
            displayText.enabled = text != "";
        }
        return true;
    }
}
