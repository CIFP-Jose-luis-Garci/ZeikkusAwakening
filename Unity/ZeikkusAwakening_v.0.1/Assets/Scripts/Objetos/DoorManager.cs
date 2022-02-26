using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool doorOpen;
    private PlayerBagManager bag;
    private AudioSource source;
    public AudioClip abrirPuerta;
    public AudioClip llaves;

    void Awake()
    {
        bag = FindObjectOfType<PlayerBagManager>();
        source = FindObjectOfType<GameManager>().GetComponent<AudioSource>();
    }
    
    void Start()
    {
        doorOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !doorOpen)
        {
            foreach(Item item in bag.GetBagContents(5))
            {
                if(item.itemName == "Llave pequeña")
                {
                    source.PlayOneShot(llaves);
                    bag.RemoveItem(bag.ItemSlot(item));
                    StartCoroutine(LiftDoor());
                    doorOpen = true;
                    break;
                }
            }
        }
    }

    IEnumerator LiftDoor()
    {
        source.PlayOneShot(abrirPuerta);
        Debug.Log(transform.position);
        while(transform.localPosition.y < 2.3f)
        {
            transform.localPosition += Vector3.up * 2f * Time.deltaTime;
            Debug.Log("hi");
            yield return null;
        }
    }
}
