using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Panda;
using UnityEngine.AI;
using Panda.Examples.PlayTag;

// Code referenced from PandaBT plugin "Shooter" example project

public class AI : MonoBehaviour
{
    Unit enemy;
    Unit self;
    AIVision vision;

    Vector3 enemyLastSeenPosition;

    Transform nest;
    Transform ObjectGrabPoint;
    Transform ObjectGrabPointBot;

    public bool returnedToNest;

    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent navMeshAgent;

    void Start()
    {
        self = this.GetComponent<Unit>();
        vision = this.GetComponentInChildren<AIVision>();
        nest = GameObject.Find("Nest").transform;

        navMeshAgent = GetComponent<NavMeshAgent>();

        var objectGrabPointObj = GameObject.Find("ObjectGrabPoint");
        if (objectGrabPointObj != null)
        {
            ObjectGrabPoint = objectGrabPointObj.transform;
        }

        var objectGrabPointObjBot = GameObject.Find("ObjectGrabPointBot");
        if (objectGrabPointObjBot != null)
        {
            ObjectGrabPointBot = objectGrabPointObjBot.transform;
        }
    }

    [Task]
    bool SetTarget_Enemy()
    {
        if (enemy != null)
        {
            self.SetTarget(enemy.transform.position);
            return true;
        }
        return false;
    }

    [Task]
    bool SetTarget_Angle(float angle)
    {
        var p = this.transform.position + Quaternion.AngleAxis(angle, Vector3.up) * this.transform.forward;
        self.SetTarget(p);
        return true;
    }

    float lastEnemyAcquisitionTime = float.NegativeInfinity;
    [Task]
    void Acquire_Enemy()
    {
        if (Time.time - lastEnemyAcquisitionTime > 0.5f)
        {
            enemy = null;

            if (enemy == null && self.shotBy != null && self.shotBy.team != self.team)
                enemy = self.shotBy;

            if (enemy == null && vision.visibles != null)
            {
                foreach (var v in vision.visibles)
                {
                    if (v == null)
                        continue;

                    var shooter = v.GetComponent<Unit>();

                    if (shooter == null)
                    {
                        var bullet = v.GetComponent<Bullet>();
                        shooter = bullet != null && bullet.shooter != null ? bullet.shooter.GetComponent<Unit>() : null;

                        if (shooter != null && self.team == shooter.team)
                            shooter = null;
                    }

                    if (shooter != null && shooter.team != self.team)
                    {
                        enemy = shooter;
                        break;
                    }
                }
            }
            lastEnemyAcquisitionTime = Time.time;
        }

        Task.current.Complete(enemy != null);

    }

    [Task]
    bool Clear_Enemy()
    {
        enemy = self.shotBy = null;
        return true;
    }

    float lastSeenTime = float.NegativeInfinity;

    // Task to check if a player is visible
    [Task]
    bool IsVisible_Player()
    {
        // Iterate through the list of visible objects
        foreach (var v in vision.visibles)
        {
            // Check if the tag of the visible object is "Player"
            if (v != null && v.CompareTag("Player"))
            {
                // If a player is found, return true
                return true;
            }
        }

        // If no player is found, return false
        return false;
    }

    // Task to set the destination to the player's position
    [Task]
    bool SetDestination_Enemy()
    {
        bool succeeded = false;

        if (enemy != null)
        {
            self.SetDestination(enemy.transform.position);
            succeeded = true;
        }
        return succeeded;
    }

    [Task]
    bool HasEnemy()
    {
        return enemy != null;
    }

    // Task to check for visible treasure
    [Task]
    bool IsVisible_Treasure()
    {
        // Iterate through the list of visible objects
        foreach (var v in vision.visibles)
        {
            // Check if the tag of the visible object is "Treasure"
            if (v != null && v.CompareTag("Treasure"))
            {
                // If a treasure is found, return true
                return true;
            }
        }

        // If no treasure is found, return false
        return false;
    }

    // Task to check for visible stolen treasure
    [Task]
    bool IsVisible_StolenTreasure()
    {
        // Iterate through the list of visible objects
        foreach (var v in vision.visibles)
        {
            // Check if the tag of the visible object is "Stolen"
            if (v != null && v.CompareTag("Stolen"))
            {
                // If a stolen treasure is found, return true
                return true;
            }
        }

        // If no stolen treasure is found, return false
        return false;
    }

    // Task to set the destination to the treasure
    [Task]
    bool SetDestination_Treasure()
    {
        bool succeeded = false;

        // Iterate through the list of visible objects
        foreach (var v in vision.visibles)
        {
            // Check if the tag of the visible object is "Treasure"
            if (v != null && v.CompareTag("Treasure"))
            {
                self.SetDestination(v.transform.position);
                succeeded = true;
            }
        }

        return succeeded;
    }

    // Task to set the destination to the stolen treasure
    [Task]
    bool SetDestination_StolenTreasure()
    {
        bool succeeded = false;

        // Iterate through the list of visible objects
        foreach (var v in vision.visibles)
        {
            // Check if the tag of the visible object is "Stolen"
            if (v != null && v.CompareTag("Stolen"))
            {
                self.SetDestination(v.transform.position);
                succeeded = true;
            }
        }

        return succeeded;
    }

    // Task to set the destination to the nest
    [Task]
    void SetDestination_Nest()
    {
        if (nest != null)
        {
            self.SetDestination(nest.position);
        }

        // Set stopping distance for the NavMeshAgent
        navMeshAgent.stoppingDistance = 1f;

        // Check if the bot has reached the destination
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Task.current.Succeed();
            returnedToNest = true;
        }
    }

    // Task to pick up visible treasures
    [Task]
    public void PickUp()
    {
        // Iterate through the list of visible objects
        foreach (var v in vision.visibles)
        {
            if (v != null && v.CompareTag("Treasure") || v.CompareTag("Stolen"))
            {
                v.transform.position = ObjectGrabPointBot.position; // Move object to grab point
                v.transform.SetParent(ObjectGrabPointBot); // Set parent to grab point
                v.transform.localRotation = Quaternion.identity; // Reset local rotation
                v.GetComponent<Rigidbody>().useGravity = false; // Disable gravity
                v.GetComponent<BoxCollider>().enabled = false; // Disable collider
                v.tag = "Marked"; // Change tag to "Marked"
            }
        }

        Task.current.Succeed();
    }

    // Task to drop the held object
    [Task]
    public void Drop()
    {
        if (ObjectGrabPointBot.childCount >= 1)
        {
            foreach (Transform child in ObjectGrabPointBot)
            {
                child.SetParent(null); // Detach the child
                var rb = child.GetComponent<Rigidbody>();
                var collider = child.GetComponent<BoxCollider>();

                if (rb != null)
                {
                    rb.useGravity = true; // Enable gravity
                }

                if (collider != null)
                {
                    collider.enabled = true; // Enable collider
                }
            }
        }

        Task.current.Succeed();
    }

    // Task to check if the player has marked treasure
    [Task]
    public bool PlayerHasMarkedTreasure()
    {
        if (ObjectGrabPoint == null)
        {
            // If player or ObjectGrabPoint has been destroyed, stop processing
            return false;
        }

        if (ObjectGrabPoint.childCount >= 1)
        {
            // Check if the player is holding anything
            foreach (Transform child in ObjectGrabPoint)
            {
                // Check if the player is holding a marked treasure
                if (child.tag == "Marked")
                {
                    // If the player is holding a marked treasure, return true
                    return true;
                }
            }
        }

        // If the player is not holding a marked treasure, return false
        return false;
    }

}