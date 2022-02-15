using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemorySceneManager : MonoBehaviour
{
    public Image flash;
    public DialogueManager dialogo;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDialogue()
    {
        dialogo.gameObject.SetActive(true);
    }
}
