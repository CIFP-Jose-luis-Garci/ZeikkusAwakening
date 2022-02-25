using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CutsceneManager : MonoBehaviour
{
    public GameObject[] cameras;
    public DialogueManager dialogue;
    public int dialogueCount;
    public bool endingCutscene;

    public abstract void DoStuff();

    public abstract void EndCutScene();
}
