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
    private Coroutine coroutine;
    private string currentPhrase;
    private void OnEnable()
    {
        currentEvent = GameManager.currentEvent;
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
                return false;
            }
        }

        voice.clip = voiceOvers[GameManager.currentDialogue];
        voice.Play();
        string dialogue = DialogueLookupTable.DialogueLookup(GameManager.currentDialogue);
        dialogueName.text = GameManager.talking;
        coroutine = StartCoroutine(LetraALetra(dialogue));
        return true;
    }

    private IEnumerator LetraALetra(string phrase)
    {
        showingPhrase = true;
        currentPhrase = phrase;
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
        showingPhrase = false;
        // mostrar flecha de seguir diálogo
    }
}
