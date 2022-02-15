using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private void OnEnable()
    {
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
