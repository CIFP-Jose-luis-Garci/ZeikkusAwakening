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
    PlayerLocomotion playerLocomotion;
    [SerializeField] GameObject CameraArray;

    


    // Start is called before the first frame update
    void Start()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
        playerLocomotion = FindObjectOfType<PlayerLocomotion>();
        playerLocomotion.runningSpeed = 0f;

        //Camaras estado inicial
        cameras[0].SetActive(true);
        cameras[1].SetActive(false);

        StartCoroutine("SceneBoss");
    }

    IEnumerator SceneBoss()
    {

        DoStuff();
        yield return new WaitForSeconds(2f);
        
        EndCutScene();
        yield return new WaitForSeconds(2f);
        yield break;
    }
    public override void DoStuff()
    {
 
        cameras[0].SetActive(false);
        cameras[1].SetActive(true);
 
    }

    public override void EndCutScene()
    {
        
        foreach (GameObject camera in cameras)
        {
            camera.SetActive(false);
        }

        triggerCutScene.SetActive(false);
        playerLocomotion.runningSpeed = 7f;
    }



}