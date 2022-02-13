using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private bool doorOpen;
    public Transform player;
    private PlayerBagManager bag;

    void Awake()
    {
        bag = FindObjectOfType<PlayerBagManager>();
    }
    
    void Start()
    {
        doorOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !doorOpen)
        {
            foreach( Item item in bag.GetBagContents(5))
            {
                //print(item.itemName);
                if(item.itemName == "Llave pequeña")
                {
                    bag.RemoveItem(bag.ItemSlot(item));
                    //print(item.itemName);
                    StartCoroutine("liftDoor");
                    doorOpen = true;
                    break;
                }
            }
        }
    }

    IEnumerator liftDoor()
    {
        while(transform.position.y < 2.3f)
        {
            transform.position += Vector3.up * 2.3f * Time.deltaTime;
            //print("habrir puerta");
            yield return null ;
        }
        //print("abierto");
    }
}
