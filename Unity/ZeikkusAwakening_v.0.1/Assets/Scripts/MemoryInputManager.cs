using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MemoryInputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private DialogueManager dialogue;
    private Text dialogueText;

    public bool aInput;

    private void Start()
    {
        dialogue = FindObjectOfType<DialogueManager>();
        dialogueText = dialogue.GetComponent<Text>();
    }

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
            NextDialogue();
        }
    }

    private void NextDialogue()
    {
        StartCoroutine(LetraALetra());
    }

    private IEnumerator LetraALetra(string phrase)
    {
        dialogueText.text = "";
        // reproducir audio de doblaje
        StringBuilder sb = new StringBuilder(dialogueText.text);
        int count = 0;
        while (count < phrase.Length)
        {
            sb.Append(phrase[0]);
            count++;
            yield return new WaitForSeconds(0.1f);
        }
        // mostrar flecha de seguir diálogo
    }
}
