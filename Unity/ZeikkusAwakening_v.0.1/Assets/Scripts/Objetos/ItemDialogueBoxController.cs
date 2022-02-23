using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemDialogueBoxController : MonoBehaviour
{
    public Text itemName, description;
    public Image item, dialogue;
    public Graphic[] children;

    private InputManager inputManager;

    private void Start()
    {
        children = new Graphic[4];
        children[0] = itemName;
        children[1] = description;
        children[2] = item;
        children[3] = dialogue;
        foreach (Graphic child in children)
        {
            if (!child) continue;
            child.CrossFadeAlpha(0, 0, false);
            child.CrossFadeAlpha(1, 0.25f, false);
        }
        inputManager = FindObjectOfType<InputManager>();
        inputManager.inDialogue = true;
        inputManager.xInput = false;
    }

    private void Update()
    {
        if (inputManager.xInput && GameManager.inPause)
        {
            GameManager.inPause = false;
            foreach (Graphic child in children)
            {
                if (!child) continue;
                child.CrossFadeAlpha(0, 0.25f, false);
            }
            inputManager.inDialogue = false;
            Destroy(gameObject, 0.3f);
        }
    }
}
