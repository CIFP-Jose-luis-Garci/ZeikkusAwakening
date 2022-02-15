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

    private void Awake()
    {
        dialogue = FindObjectOfType<DialogueManager>();
        dialogueText = dialogue.GetComponentInChildren<Text>();
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

    public void NextDialogue()
    {
        StartCoroutine(LetraALetra(DialogueLookupTable.DialogueLookup(GameManager.currentDialogue)));
    }

    private IEnumerator LetraALetra(string phrase)
    {
        GameManager.currentDialogue++;
        dialogueText.text = "";
        // reproducir audio de doblaje
        string currentText = "";
        for (int i = 0; i < phrase.Length; i++)
        {
            currentText = phrase.Substring(0, i);
            dialogueText.text = currentText;
            yield return new WaitForSeconds(0.05f);
        }
        dialogueText.text = phrase;
        // mostrar flecha de seguir diálogo
    }
}
