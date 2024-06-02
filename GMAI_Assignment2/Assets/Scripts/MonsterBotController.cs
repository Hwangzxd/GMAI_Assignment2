using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Cinemachine.CinemachineFreeLook;

// Code referenced from PandaBT plugin "Shooter" example project

public class MonsterBotController : MonoBehaviour
{
    public Transform[] treasures;
    public Transform nest;
    public WaypointPath waypointPath;

    Unit self;
    int waypointIndex;

    void Start()
    {
        // Initialize the waypoint index
        waypointIndex = 0;
        self = GetComponent<Unit>();
    }

    void Update()
    {

    }

    // Task to move to the next waypoint
    [Task]
    bool NextWaypoint()
    {
        if (waypointPath != null)
        {
            // Increment waypoint index
            waypointIndex++;
            if (Task.isInspected)
                Task.current.debugInfo = string.Format("i={0}", waypointArrayIndex);
        }
        return true;
    }

    // Task to set the destination to the current waypoint
    [Task]
    bool SetDestination_Waypoint()
    {
        bool isSet = false;
        if (waypointPath != null)
        {
            var i = waypointArrayIndex; // Get the current waypoint index
            var p = waypointPath.waypoints[i].position; // Get the waypoint position
            isSet = self.SetDestination(p); // Set the destination
        }
        return isSet;
    }

    // Task to set the destination to a specific waypoint by index
    [Task]
    public bool SetDestination_Waypoint(int i)
    {
        bool isSet = false;
        if (waypointPath != null)
        {
            var p = waypointPath.waypoints[i].position; // Get the waypoint position
            isSet = self.SetDestination(p); // Set the destination
        }
        return isSet;
    }

    // Task to move to a specific waypoint by index
    [Task]
    public void MoveTo(int i)
    {
        SetDestination_Waypoint(i);
        self.MoveTo_Destination();
        self.WaitArrival();
    }

    // Task to look at a specific waypoint by index
    [Task]
    public void LookAt(int i)
    {
        // Set the target position
        self.SetTarget(waypointPath.waypoints[i].transform.position);
        self.AimAt_Target();
    }

    // Calculate the current waypoint index based on looping and path length
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