using Panda.Examples.PlayTag;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Code taken from: https://youtu.be/YlB9BlRIryk?si=e9Dq_L5rJIMVPY7W

public class PickUpItem : MonoBehaviour
{
    private Transform ObjectGrabPoint;
    private Transform player;

    public float pickUpDistance;
    public float forceMulti;

    public bool readyToThrow;
    public bool itemIsPicked;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
        ObjectGrabPoint = GameObject.Find("ObjectGrabPoint").transform;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E) && itemIsPicked == true && readyToThrow)
        {
            forceMulti += 300 * Time.deltaTime;
        }

        pickUpDistance = Vector3.Distance(player.position, transform.position);

        if (pickUpDistance <= 2)
        {
            if (Input.GetKeyDown(KeyCode.E) && itemIsPicked == false && ObjectGrabPoint.childCount < 1)
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().enabled = false;
                this.transform.position = ObjectGrabPoint.position;
                this.transform.parent = GameObject.Find("ObjectGrabPoint").transform;

                itemIsPicked = true;
                forceMulti = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && itemIsPicked == true)
        {
            readyToThrow = true;

            if (forceMulti > 10)
            {
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
}
