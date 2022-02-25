using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Cutscene2Manager : CutsceneManager
{
    private AnimatorManager animatorManager;
    public GameObject triggerCutScene;

    int CameraChanges = 4;


    // Start is called before the first frame update
    void Start()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();

    }


    public override void DoStuff()
    {
        
        switch (CameraChanges)
        {
            case 1:
                cameras[0].SetActive(true);
                cameras[1].SetActive(false);
                break;
            case 2:
                StartCoroutine(ChangeCamera());
                break;
            case 3:
                StartCoroutine(GetSwordAndShow());
                break;
            case 4:
                EndCutScene();
                break;
            
            /*case 16:
                inputManager.WinBattle();
                break;*/
            
            default:
                break;
        }
    }

    public override void EndCutScene()
    {
        
        foreach (GameObject camera in cameras)
        {
            camera.SetActive(false);
        }

        triggerCutScene.SetActive(false);
    }

    private IEnumerator ChangeCamera()
    {
        cameras[0].SetActive(false);
        cameras[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        
    }

    private IEnumerator GetSwordAndShow()
    {
        
        FindObjectOfType<HUDManager>().StartBattleAnimation();
        yield return new WaitForSeconds(2f);
       
    }

}