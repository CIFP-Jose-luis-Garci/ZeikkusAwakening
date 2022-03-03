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
    public AudioSource source;

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
            if (!dialogue.gameObject.activeSelf) return;
            if (dialogue.currentEvent == GameManager.Instance.currentEvent || dialogue.showingPhrase)
            {
                if (dialogue.NextDialogue())
                    source.PlayOneShot(source.clip);;
            }
            else
            {
                dialogue.gameObject.SetActive(false);
                StartCoroutine(GetComponent<MemorySceneManager>().FadeOutFlash(2));
                source.PlayOneShot(source.clip);
            }
        }
    }
}
