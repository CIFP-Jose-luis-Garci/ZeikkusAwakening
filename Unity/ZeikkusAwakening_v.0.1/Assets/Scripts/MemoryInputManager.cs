using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MemoryInputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    public DialogueManager dialogue;

    public bool aInput;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerActions.A.performed += i => aInput = true;
            playerControls.PlayerActions.A.canceled += i => aInput = false;

        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        HandleAInput();
    }


    private void HandleAInput()
    {
        if (aInput)
        {
            aInput = false;
            if (dialogue.currentEvent == GameManager.currentEvent || dialogue.showingPhrase)
            {
                dialogue.NextDialogue();
            }
            else
            {
                dialogue.gameObject.SetActive(false);
                StartCoroutine(GetComponent<MemorySceneManager>().FadeOutFlash(2));
            }
        }
    }
}
