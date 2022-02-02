using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CofreManager : MonoBehaviour
{
    public Item containedItem;
    public Image openChest;
    private InputManager inputManager;

    private void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        openChest.CrossFadeAlpha(0, 0f, false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hola!");
            openChest.CrossFadeAlpha(1, 0.5f, false);
        }
    }

    private void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
            if (containedItem != null && inputManager.bInput)
            {
                openChest.CrossFadeAlpha(0, 0.5f, false);
                FindObjectOfType<PlayerBagManager>().AddItem(containedItem);
                containedItem = null;
            }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            openChest.CrossFadeAlpha(0, 0.5f, false);
    }
}
