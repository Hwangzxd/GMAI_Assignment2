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
    public Transform[] waypoints;    
    public Text displayText;

    int waypointIndex;
    Vector3 target;

    private Rigidbody rb;
    private NavMeshAgent navAgent;
    private MonsterBotController monsterBot;

    //Transform target;
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

        UpdateDestination();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target) < 1)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }
    }

    void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        navAgent.SetDestination(target);
    }

    void IterateWaypointIndex()
    {
        waypointIndex++;

        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
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
    bool CanSeePlayer()
    {
        return true;
    }

    [Task]
    bool IsRoaming()
    {
        return true;
    }

    [Task]
    bool IsGuarding()
    {
        displayText.text = "Guarding";
        return true;
    }

    [Task]
    bool IsAggro()
    {
        return true;
    }

    [Task]
    bool IsHardAggro()
    {
        return true;
    }

    [Task]
    bool IsReloading()
    {
        return true;
    }

    [Task]
    bool IsReturning()
    {
        return true;
    }

    [Task]
    bool IsRetrieving()
    {
        return true;
    }

    [Task]
    bool MeleeAttack()
    {
        return true;
    }

    [Task]
    bool RangedAttack()
    {
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
