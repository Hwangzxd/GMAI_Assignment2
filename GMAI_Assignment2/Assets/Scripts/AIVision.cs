using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Code taken from PandaBT plugin "Shooter" example project

public class AIVision : MonoBehaviour
{
    public float fieldOfView = 90.0f; // Objects within this field of view are seen
    public float closeFieldDistance = 1.0f;

    public List<Collider> colliders = new List<Collider>(); // List of colliders within vision
    public List<GameObject> visibles = new List<GameObject>(); // List of visible game objects

    public Unit shooter;

    public float lastBulletSeenTime;

    // Handle event when a collider enters the trigger
    void OnTriggerEnter(Collider other)
    {
        var triggerType = other.GetComponent<TriggerType>();
        if (triggerType != null && triggerType.collidesWithVision && !colliders.Contains(other))
        {
            colliders.Add(other);
            colliders.RemoveAll((c) => c == null);
        }
    }

    // Handle event when a collider exits the trigger
    void OnTriggerExit(Collider other)
    {
        if (colliders.Contains(other))
        {
            colliders.Remove(other);
            colliders.RemoveAll((c) => c == null);
        }
    }

    // Update list of visible objects
    void UpdateVisibility()
    {
        visibles.Clear();
        foreach (var c in colliders)
        {
            if (c == null)
                continue;

            var go = c.attachedRigidbody != null ? c.attachedRigidbody.gameObject : null;

            bool isVisible = false;

            if (go != null)
            {

                float angle = Vector3.Angle(this.transform.forward, go.transform.position - this.transform.position);

                bool isInClosedField = Vector3.Distance(go.transform.position, this.transform.position) <= closeFieldDistance;
                bool isInFieldOfView = Mathf.Abs(angle) <= fieldOfView * 0.5f;


                isVisible = isInClosedField || (isInFieldOfView && HasLoS(go.gameObject));

            }

            if (isVisible && !visibles.Contains(go))
            {
                var bullet = go.GetComponent<Bullet>();
                if (bullet != null)
                {
                    var attacker = bullet.shooter != null ? bullet.shooter.GetComponent<Unit>() : null;
                    if (attacker != null && attacker.team != shooter.team)
                        lastBulletSeenTime = Time.time;
                }
                visibles.Add(go);
            }

        }
    }

    // Check if there is line of sight to the target object
    bool HasLoS(GameObject target)
    {
        bool has = false;
        var targetDirection = (target.transform.position - this.transform.position).normalized;

        Ray ray = new Ray(this.transform.position, targetDirection);
        var hits = Physics.RaycastAll(ray, float.PositiveInfinity);

        float minD = float.PositiveInfinity;
        GameObject closest = null;

        foreach (var h in hits)
        {
            var ct = h.collider.GetComponent<TriggerType>();
            if (ct == null || !ct.collidesWithVision)
                continue;

            float d = Vector3.Distance(h.point, this.transform.position);
            var o = h.collider.attachedRigidbody != null ? h.collider.attachedRigidbody.gameObject : h.collider.gameObject;
            if (d <= minD && o != this.gameObject)
            {
                minD = d;
                closest = o;
            }
        }

        has = closest == target;

        return has;
    }

    void Start()
    {
        lastBulletSeenTime = float.NegativeInfinity;
    }

    void Update()
    {
        UpdateVisibility();
    }
}