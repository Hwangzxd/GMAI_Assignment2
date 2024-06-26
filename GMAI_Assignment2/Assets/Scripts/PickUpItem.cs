using Panda.Examples.PlayTag;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Code referenced from: https://youtu.be/YlB9BlRIryk?si=e9Dq_L5rJIMVPY7W

public class PickUpItem : MonoBehaviour
{
    private Transform ObjectGrabPoint;
    private Transform player;

    public float pickUpDistance;
    public float pickUpDistanceBot;
    public float forceMulti;

    public bool readyToThrow;
    public bool itemIsPicked;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Find and set the player transform
        var playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            // Subscribe to the OnPlayerDestroyed event
            Player.OnPlayerDestroyed += OnPlayerDestroyed;
        }

        // Find and set the ObjectGrabPoint transform
        var objectGrabPointObj = GameObject.Find("ObjectGrabPoint");
        if (objectGrabPointObj != null)
        {
            ObjectGrabPoint = objectGrabPointObj.transform;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event
        Player.OnPlayerDestroyed -= OnPlayerDestroyed;
    }

    void Update()
    {
        if (player == null || ObjectGrabPoint == null)
        {
            // If player or ObjectGrabPoint has been destroyed, stop processing
            return;
        }

        OnPlayerPickUp();
    }

    // Handle item pickup logic
    void OnPlayerPickUp()
    {
        if (Input.GetKey(KeyCode.E) && itemIsPicked == true && readyToThrow)
        {
            forceMulti += 300 * Time.deltaTime;
        }

        // Calculate the distance between player and item
        pickUpDistance = Vector3.Distance(player.position, transform.position);

        // Check if the player is within pickup distance
        if (pickUpDistance <= 2)
        {
            // When not holding any items, pick up the item when the "E" key is pressed
            if (Input.GetKeyDown(KeyCode.E) && itemIsPicked == false && ObjectGrabPoint.childCount < 1)
            {
                this.transform.localRotation = Quaternion.identity; // Reset local rotation
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().enabled = false;
                this.transform.position = ObjectGrabPoint.position;
                this.transform.parent = GameObject.Find("ObjectGrabPoint").transform;

                itemIsPicked = true;
                forceMulti = 0;
            }
        }

        // When holding an item, drop it when the "E" key is pressed again
        if (Input.GetKeyDown(KeyCode.E) && itemIsPicked == true)
        {
            readyToThrow = true;

            if (forceMulti > 10)
            {
                // Check if the tag of the item is "Marked"
                if (this.transform.tag == "Marked")
                {
                    // Change the tag of the item to "Stolen"
                    this.transform.tag = "Stolen";
                }

                rb.AddForce(player.transform.forward * forceMulti);
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<BoxCollider>().enabled = true;

                itemIsPicked = false;
                forceMulti = 0;
                readyToThrow = false;
            }

            forceMulti = 0;
        }
    }

    // Handle player destroyed event
    private void OnPlayerDestroyed()
    {
        if (itemIsPicked)
        {
            // Check if the tag of the item is "Marked"
            if (this.transform.tag == "Marked")
            {
                // Change the tag of the item to "Stolen"
                this.transform.tag = "Stolen";
            }

            // Detach the item from the player and reset properties
            this.transform.parent = null;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<BoxCollider>().enabled = true;

            itemIsPicked = false;
            forceMulti = 0;
            readyToThrow = false;
        }
    }
}
