using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public int currentEvent;
    private void OnEnable()
    {
        currentEvent = GameManager.currentEvent;
        InputManager inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null)
            inputManager.NextDialogue();
        else
        {
            MemoryInputManager memoryInputManager = FindObjectOfType<MemoryInputManager>();
            memoryInputManager.NextDialogue();
        }
    }
}
