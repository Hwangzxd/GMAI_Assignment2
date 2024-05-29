using Panda.Examples.PlayTag;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Code referenced from: https://youtu.be/YlB9BlRIryk?si=e9Dq_L5rJIMVPY7W

public class PickUpItem : MonoBehaviour
{
    private Transform ObjectGrabPoint;
    private Transform ObjectGrabPointBot;
    private Transform player;
    private Transform monsterBot;

    AI ai;

    public float pickUpDistance;
    public float pickUpDistanceBot;
    public float forceMulti;

    public bool readyToThrow;
    public bool itemIsPicked;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        ai = GameObject.FindObjectOfType<AI>();

        //player = GameObject.Find("Player").transform;
        //ObjectGrabPoint = GameObject.Find("ObjectGrabPoint").transform;

        var playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Player.OnPlayerDestroyed += OnPlayerDestroyed; // Subscribe to the event
        }

        var botObj = GameObject.Find("MonsterBot");
        if (botObj != null)
        {
            monsterBot = botObj.transform;
        }

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

    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        Player.OnPlayerDestroyed -= OnPlayerDestroyed;
    }

    void Update()
    {
        if (player == null || ObjectGrabPoint == null)
        {
            // Player or ObjectGrabPoint has been destroyed, so stop further processing
            return;
        }

        OnPlayerPickUp();
        OnMonsterBotPickUp();

        //if (Input.GetKey(KeyCode.E) && itemIsPicked == true && readyToThrow)
        //{
        //    forceMulti += 300 * Time.deltaTime;
        //}

        //pickUpDistance = Vector3.Distance(player.position, transform.position);

        //if (pickUpDistance <= 2)
        //{
        //    if (Input.GetKeyDown(KeyCode.E) && itemIsPicked == false && ObjectGrabPoint.childCount < 1)
        //    {
        //        GetComponent<Rigidbody>().useGravity = false;
        //        GetComponent<BoxCollider>().enabled = false;
        //        this.transform.position = ObjectGrabPoint.position;
        //        this.transform.parent = GameObject.Find("ObjectGrabPoint").transform;

        //        itemIsPicked = true;
        //        forceMulti = 0;
        //    }
        //}

        //pickUpDistanceBot = Vector3.Distance(monsterBot.position, transform.position);

        //if (pickUpDistanceBot <= 2)
        //{
        //    if (itemIsPicked == false && ObjectGrabPointBot.childCount < 1)
        //    {
        //        GetComponent<Rigidbody>().useGravity = false;
        //        GetComponent<BoxCollider>().enabled = false;
        //        this.transform.position = ObjectGrabPointBot.position;
        //        this.transform.parent = GameObject.Find("ObjectGrabPointBot").transform;

        //        itemIsPicked = true;
        //        forceMulti = 0;
        //    }
        //}

        //if (Input.GetKeyUp(KeyCode.E) && itemIsPicked == true)
        //{
        //    readyToThrow = true;

        //    if (forceMulti > 10)
        //    {
        //        rb.AddForce(player.transform.forward * forceMulti);
        //        this.transform.parent = null;
        //        GetComponent<Rigidbody>().useGravity = true;
        //        GetComponent<BoxCollider>().enabled = true;

        //        itemIsPicked = false;
        //        forceMulti = 0;
        //        readyToThrow = false;
        //    }

        //    forceMulti = 0;
        //}

        //if (ai.returnedToNest && itemIsPicked == true)
        //{
        //    readyToThrow = true;

        //    rb.AddForce(monsterBot.transform.forward * 10f);
        //    //rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        //    this.transform.parent = null;
        //    //this.transform.localPosition = Vector3.zero; // Reset local position
        //    //this.transform.localRotation = Quaternion.identity; // Reset local rotation
        //    GetComponent<Rigidbody>().useGravity = true;
        //    GetComponent<BoxCollider>().enabled = true;

        //    itemIsPicked = false;       
        //    forceMulti = 0;
        //    readyToThrow = false;
        //}
    }

    void OnPlayerPickUp()
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

    void OnMonsterBotPickUp()
    {
        pickUpDistanceBot = Vector3.Distance(monsterBot.position, transform.position);

        if (pickUpDistanceBot <= 2)
        {
            if (ObjectGrabPointBot.childCount < 1)
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().enabled = false;
                this.transform.position = ObjectGrabPointBot.position;
                this.transform.parent = GameObject.Find("ObjectGrabPointBot").transform;

                //itemIsPicked = true;
                //forceMulti = 0;
            }
        }

        if (ai.returnedToNest && itemIsPicked == true)
        {
            //readyToThrow = true;

            //rb.AddForce(monsterBot.transform.forward * 10f);
            //rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
            this.transform.parent = null;
            //this.transform.localPosition = Vector3.zero; // Reset local position
            //this.transform.localRotation = Quaternion.identity; // Reset local rotation
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<BoxCollider>().enabled = true;

            //itemIsPicked = false;
            //forceMulti = 0;
            //readyToThrow = false;
        }
    }

    private void OnPlayerDestroyed()
    {
        if (itemIsPicked)
        {
            // Detach the item from the player
            this.transform.parent = null;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<BoxCollider>().enabled = true;

            itemIsPicked = false;
            forceMulti = 0;
            readyToThrow = false;
        }
    }
}
