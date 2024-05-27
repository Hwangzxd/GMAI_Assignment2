using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Panda;
using Panda.Examples.PlayTag;

public class AI : MonoBehaviour
{
    Unit enemy;
    Unit self;
    AIVision vision;

    float random_destination_radius = 1.0f;

    Vector3 enemyLastSeenPosition;

    [SerializeField]
    GameObject treasurePrefab; // Reference to the target object prefab

    List<GameObject> treasures = new List<GameObject>();

    Transform nest;
    Transform ObjectGrabPointBot;

    // Use this for initialization
    void Start()
    {
        self = this.GetComponent<Unit>();
        vision = this.GetComponentInChildren<AIVision>();
        nest = GameObject.Find("Nest").transform;

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
    bool SetTarget_EnemyLastSeenPosition()
    {
        if (enemy != null)
        {
            self.SetTarget(enemyLastSeenPosition);
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

            if (enemy == null && self.shotBy != null && self.shotBy.team != self.team && (Time.time - self.lastShotTime) < 1.0f)
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
    bool HasAmmo_Ememy()
    {
        bool has = false;
        if (enemy != null)
            has = enemy.ammo > 0;
        return has;
    }

    [Task]
    bool Clear_Enemy()
    {
        enemy = self.shotBy = null;
        return true;
    }

    float lastSeenTime = float.NegativeInfinity;

    [Task]
    bool IsVisible_Enemy()
    {
        if (enemy != null && enemy.gameObject != null)
        {
            foreach (var v in vision.visibles)
            {
                if (v == enemy.gameObject)
                {
                    lastSeenTime = Time.time;
                    enemyLastSeenPosition = enemy.transform.position;
                    break;
                }
            }
        }

        return (Time.time - lastSeenTime) < 0.5f;
    }

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
    bool SetDestination_Random(float radius)
    {
        random_destination_radius = radius;
        return SetDestination_Random();
    }

    [Task]
    bool SetDestination_Random()
    {
        var dst = this.transform.position + (Random.insideUnitSphere * random_destination_radius);
        self.SetDestination(dst);
        return true;
    }

    [Task]
    bool HasEnemy()
    {
        return enemy != null;
    }

    [Task]
    bool IsVisible_Treasure()
    {
        // Iterate through the list of visible objects
        foreach (var v in vision.visibles)
        {
            // Check if the visible object is a treasure (replace "Treasure" with the tag or name of your treasure objects)
            if (v != null && v.CompareTag("Treasure"))
            {
                // If a treasure is found, return true
                return true;
            }
        }

        // If no treasure is found among visible objects, return false
        return false;
    }

    [Task]
    bool SetDestination_Treasure()
    {
        foreach (var v in vision.visibles)
        {
            if (v != null && v.CompareTag("Treasure"))
            {
                self.SetDestination(v.transform.position);
                return true;
            }
        }

        return false;
    }

    //[Task]
    //bool PickUpTreasure()
    //{
    //    if (ObjectGrabPointBot.childCount > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    [Task]
    bool ReturnToNest()
    {
        if (nest != null && ObjectGrabPointBot.childCount > 1)
        {
            self.SetDestination(nest.position);
            return true;
        }
        return false;
    }

}