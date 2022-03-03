using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public int currentEvent;
    public bool showingPhrase;
    public AudioSource voice;
    public AudioClip[] voiceOvers;
    public Text dialogueText, dialogueName;
    public GameObject aButton;
    private Coroutine coroutine;
    private GameManager gameManager;
    private string currentPhrase;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        currentEvent = gameManager.currentEvent;
        NextDialogue();
    }

    public bool NextDialogue()
    {
        if (coroutine != null)
        {
            if (showingPhrase)
            {
                StopCoroutine(coroutine);
                dialogueText.text = currentPhrase;
                showingPhrase = false;
                aButton.SetActive(true);
                return false;
            }
        }
        aButton.SetActive(false);
        voice.clip = voiceOvers[gameManager.currentDialogue];
        voice.Play();
        string dialogue = DialogueLookupTable.DialogueLookup(gameManager.currentDialogue);
        gameManager.currentDialogue++;
        dialogueName.text = gameManager.talking;
        coroutine = StartCoroutine(LetraALetra(dialogue));
        return true;
    }

    private IEnumerator LetraALetra(string phrase)
    {
        showingPhrase = true;
        currentPhrase = phrase;
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
        showingPhrase = false;
        aButton.SetActive(true);
        // mostrar flecha de seguir diálogo
    }
}
