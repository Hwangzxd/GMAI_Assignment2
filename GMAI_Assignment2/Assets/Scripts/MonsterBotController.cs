using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Cinemachine.CinemachineFreeLook;

public class MonsterBotController : MonoBehaviour
{
    public Transform[] treasures;
    public Transform nest;
    public WaypointPath waypointPath;

    //private Rigidbody rb;
    //private NavMeshAgent navAgent;
    //private MonsterBotController robot;

    //Transform target;
    //GameObject player;

    Unit self;
    int waypointIndex;

    void Start()
    {
        waypointIndex = 0;
        self = GetComponent<Unit>();
    }

    void Update()
    {

    }

    [Task]
    bool NextWaypoint()
    {
        if (waypointPath != null)
        {
            waypointIndex++;
            if (Task.isInspected)
                Task.current.debugInfo = string.Format("i={0}", waypointArrayIndex);
        }
        return true;
    }

    [Task]
    bool SetDestination_Waypoint()
    {
        bool isSet = false;
        if (waypointPath != null)
        {
            var i = waypointArrayIndex;
            var p = waypointPath.waypoints[i].position;
            isSet = self.SetDestination(p);
        }
        return isSet;
    }

    [Task]
    public bool SetDestination_Waypoint(int i)
    {
        bool isSet = false;
        if (waypointPath != null)
        {
            var p = waypointPath.waypoints[i].position;
            isSet = self.SetDestination(p);
        }
        return isSet;
    }

    [Task]
    public void MoveTo(int i)
    {
        SetDestination_Waypoint(i);
        self.MoveTo_Destination();
        self.WaitArrival();
    }

    [Task]
    public void LookAt(int i)
    {
        self.SetTarget(waypointPath.waypoints[i].transform.position);
        self.AimAt_Target();
    }


    int waypointArrayIndex
    {
        get
        {
            int i = 0;
            if (waypointPath.loop)
            {
                i = waypointIndex % waypointPath.waypoints.Length;
            }
            else
            {
                int n = waypointPath.waypoints.Length;
                i = waypointIndex % (n * 2);

                if (i > n - 1)
                    i = (n - 1) - (i % n);
            }

            return i;
        }
    }

}