using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private bool doorOpen1;
    private PlayerBagManager bag;
    private AudioSource source;
    public SpriteRenderer minimapSprite;
    public AudioClip abrirPuerta;
    public AudioClip llaves;

    void Awake()
    {
        bag = GameManager.Instance.bag;
        source = GameManager.Instance.source;
    }
    
    void Start()
    {
        doorOpen1 = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !doorOpen1)
        {
            foreach(Item item in bag.GetBagContents(5))
            {
                if(item.itemName == "Llave pequeña")
                {
                    source.PlayOneShot(llaves);
                    minimapSprite.gameObject.SetActive(false);
                    bag.RemoveItem(bag.ItemSlot(item));
                    StartCoroutine(LiftDoor());
                    doorOpen1 = true;
                    break;
                }
            }
        }
    }

    IEnumerator LiftDoor()
    {
        source.PlayOneShot(abrirPuerta);
        while(transform.localPosition.y < 2.3f)
        {
            transform.localPosition += Vector3.up * 2f * Time.deltaTime;
            yield return null;
        }
    }
}
