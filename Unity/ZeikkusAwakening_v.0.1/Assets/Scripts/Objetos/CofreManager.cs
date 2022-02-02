using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CofreManager : MonoBehaviour
{
    public Item containedItem;
    public Image openChest;
    public GameObject dialogue;
    private InputManager inputManager;
    private Animator animator;

    private void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        animator = GetComponent<Animator>();
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
                animator.enabled = true;
            }
            
    }

    public void OpenEvent()
    {
        openChest.CrossFadeAlpha(0, 0.5f, false);
        FindObjectOfType<PlayerBagManager>().AddItem(containedItem);
        GameObject currentDialogue = Instantiate(dialogue, FindObjectOfType<Canvas>().transform);
        ItemDialogueBoxController idbc = currentDialogue.GetComponent<ItemDialogueBoxController>();
        idbc.description.text = containedItem.description;
        idbc.name.text = containedItem.name;
        idbc.item.sprite = containedItem.itemSprite;
        containedItem = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            openChest.CrossFadeAlpha(0, 0.5f, false);
    }
}
