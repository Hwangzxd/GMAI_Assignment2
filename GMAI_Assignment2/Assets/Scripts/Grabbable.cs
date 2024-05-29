using Panda.Examples.PlayTag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{
    void PickUp();
    void Drop();
}

public class Grabbable : MonoBehaviour, IGrabbable
{
    private Transform ObjectGrabPoint;
    private Transform ObjectGrabPointBot;
    private Transform player;
    private Transform monsterBot;

    public void PickUp()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        this.transform.position = ObjectGrabPoint.position;
        this.transform.parent = GameObject.Find("ObjectGrabPoint").transform;

    }

    public void Drop()
    {
        this.transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().enabled = true;

    }
}
