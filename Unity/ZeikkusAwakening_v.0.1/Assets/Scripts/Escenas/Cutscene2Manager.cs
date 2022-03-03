using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Cutscene2Manager : CutsceneManager
{
    public Transform badZ;
    [NonSerialized] public Transform player;
    private AnimatorManager animatorManager;
    private GameManager gameManager;
    private NavMeshAgent agente;
    private int count = 0;




    // Start is called before the first frame update
    void Start()
    {
        animatorManager = badZ.GetComponent<AnimatorManager>();
        gameManager = GameManager.Instance;
        agente = badZ.GetComponent<NavMeshAgent>();
        cameras[0] = FindObjectOfType<CinemachineFreeLook>().gameObject;
        gameManager.inCutscene = true;
        gameManager.transitioning = true;

        //Camaras estado inicial
        cameras[0].SetActive(true);
        cameras[1].SetActive(false);

        StartCoroutine("SceneBoss");
    }

    IEnumerator SceneBoss()
    {
        
        DoStuff();
        yield return new WaitForSeconds(2f);
        DoStuff();
        yield return new WaitForSeconds(2f);
        DoStuff();
        EndCutScene();
        yield return new WaitForSeconds(2f);
        gameManager.inCutscene = false;
    }
    public override void DoStuff()
    {
        switch (count)
        {
            case 0:
                cameras[0].SetActive(false);
                cameras[1].SetActive(true);
                break;
            case 1:
                animatorManager.PlayTargetAnimation("Get Sword", true);
                break;
            case 2:
                animatorManager.PlayTargetAnimation("Run", false);
                agente.SetDestination(player.position);
                break;
        }
        count++;
    }

    public override void EndCutScene()
    {
        cameras[0].SetActive(true);
        cameras[1].SetActive(false);
    }



}