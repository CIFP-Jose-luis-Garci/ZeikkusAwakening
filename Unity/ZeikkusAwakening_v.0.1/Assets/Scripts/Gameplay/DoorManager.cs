using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool hasKey;
    private bool doorOpen;
    public Transform player;
    
    void Start()
    {
        hasKey = false;
        doorOpen = false;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if( hasKey && other.gameObject.tag == "Player" && !doorOpen)
        {
            doorOpen = true;
            StartCoroutine("liftDoor");
        }
    }

    IEnumerator liftDoor()
    {
        while(transform.position.y < 2.3f)
        {
            transform.position += Vector3.up * 0.1f ;
            //print("habrir puerta");
            yield return null ;
        }
        print("abierto");
        StopCoroutine("liftDoor");
    }
}
