using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemDialogueBoxController : MonoBehaviour
{
    public Text itemName, description;
    public Image item, dialogue, xButton;
    private Graphic[] children;

    private InputManager inputManager;

    private void Start()
    {
        children = new Graphic[5];
        children[0] = itemName;
        children[1] = description;
        children[2] = item;
        children[3] = dialogue;
        children[4] = xButton;
        foreach (Graphic child in children)
        {
            if (!child) continue;
            child.CrossFadeAlpha(0, 0, false);
            child.CrossFadeAlpha(1, 0.25f, false);
        }
        inputManager = InputManager.Instance;
        inputManager.inDialogue = true;
        inputManager.xInput = false;
        StartCoroutine(BlinkX());
    }

    private void Update()
    {
        if (inputManager.xInput && GameManager.Instance.inPause)
        {
            GameManager.Instance.inPause = false;
            foreach (Graphic child in children)
            {
                if (!child) continue;
                child.CrossFadeAlpha(0, 0.25f, false);
            }
            inputManager.inDialogue = false;
            Destroy(gameObject, 0.3f);
        }
    }

    private IEnumerator BlinkX()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            xButton.CrossFadeAlpha(1,1,true);
            yield return new WaitForSeconds(1);
            xButton.CrossFadeAlpha(0,1,true);
        }
    }
}
