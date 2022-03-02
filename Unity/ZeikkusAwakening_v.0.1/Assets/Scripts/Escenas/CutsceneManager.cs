using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CutsceneManager : MonoBehaviour
{
    public GameObject[] cameras;
    public DialogueManager dialogue;
    [NonSerialized] public int dialogueCount;
    [NonSerialized] public bool endingCutscene;

    public abstract void DoStuff();

    public abstract void EndCutScene();
}
