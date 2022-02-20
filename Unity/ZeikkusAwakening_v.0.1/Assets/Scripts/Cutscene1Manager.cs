using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene1Manager : CutsceneManager
{
    private AnimatorManager animatorManager;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
        inputManager = animatorManager.GetComponent<InputManager>();
        dialogue = FindObjectOfType<DialogueManager>();
        animatorManager.transform.rotation = Quaternion.identity;
        animatorManager.animator.CrossFade("Sleeping", 0);
        inputManager.inDialogue = true;
    }


    public override void DoStuff()
    {
        if (dialogueCount == 5)
        {
            StartCoroutine(ChangeCameraAndGetUp());
        }

        if (dialogueCount == 8)
        {
            cameras[1].SetActive(false);
            cameras[2].SetActive(true);
        }

        if (dialogueCount == 12)
        {
            cameras[2].SetActive(false);
            cameras[3].SetActive(true);
        }

        if (dialogueCount == 13)
        {
            
        }
    }

    private IEnumerator ChangeCameraAndGetUp()
    {
        cameras[0].SetActive(false);
        cameras[1].SetActive(true);
        yield return new WaitForSeconds(1.8f);
        animatorManager.PlayTargetAnimation("Get Up", false);
    }

    public IEnumerator GetSwordAndShow()
    {
        
    }
}
