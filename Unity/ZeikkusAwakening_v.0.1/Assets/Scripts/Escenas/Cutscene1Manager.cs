using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Cutscene1Manager : CutsceneManager
{
    public AnimatorManager animatorManager;
    private InputManager inputManager;
    private HUDManager hudManager;
    private GameManager gameManager;
    private bool gotSword;
    public Image blackFade;
    public Texture faceEyesOpen;
    public SkinnedMeshRenderer face;
    public CinemachineBrain brain;
    public AudioSource musicSource;
    public AudioMixer mixer;
    public GameObject tutorial;
    public GameObject minimapa;
    public GameObject saltarEscena;


    void Start()
    {
        hudManager = HUDManager.Instance;
        gameManager = GameManager.Instance;
        gameManager.personajes = new GameObject[3];
        gameManager.personajes[0] = animatorManager.gameObject;
        gameManager.pause = hudManager.pantallaPausa;
        saltarEscena.SetActive(true);
        inputManager = InputManager.Instance;
        dialogue.gameObject.SetActive(true);
        animatorManager.transform.rotation = Quaternion.identity;
        animatorManager.animator.CrossFade("Sleeping", 0);
        inputManager.inDialogue = true;
        musicSource.Stop();
        mixer.SetFloat("EnemiesSFXVolume", -80);
        mixer.SetFloat("EnvironmentSFXVolume", -80);
    }


    public override void DoStuff()
    {
        switch (dialogueCount)
        {
            case 1:
                Material[] materials = face.materials;
                materials[1].mainTexture = faceEyesOpen;
                break;
            case 3:
                animatorManager.PlayTargetAnimation("Get Up Short", false);
                break;
            case 5:
                StartCoroutine(ChangeCameraAndGetUp());
                break;
            case 7:
                cameras[1].SetActive(false);
                cameras[2].SetActive(true);
                break;
            case 10:
                animatorManager.PlayTargetAnimation("Idle Zinnia", false);
                break;
            case 12:
                cameras[2].SetActive(false);
                animatorManager.PlayTargetAnimation("Idle To Draw Zagrant", false);
                cameras[3].SetActive(true);
                break;
            case 13:
                StartCoroutine(GetSwordAndShow());
                gotSword = true;
                break;
            case 14:
                brain.m_DefaultBlend.m_Time = 0;
                cameras[3].SetActive(false);
                cameras[4].SetActive(true);
                break;
            case 15:
                cameras[4].SetActive(false);
                cameras[5].SetActive(true);
                // camara por nivel
                break;
            case 16:
                cameras[5].SetActive(false);
                cameras[3].SetActive(true);
                hudManager.WinBattleAnimation();
                gotSword = false;
                break;
            case 17:
                endingCutscene = true;
                StartCoroutine(FadeToBlack());
                break;
        }
    }

    public override void EndCutScene()
    {
        if (endingCutscene) return;
        endingCutscene = true;
        if (gotSword)
            hudManager.WinBattleAnimation();
        Material[] materials = face.materials;
        materials[1].mainTexture = faceEyesOpen;
        face.materials = materials;
        foreach (GameObject camera in cameras)
        {
            camera.SetActive(false);
        }

        gameManager.currentDialogue = 19;
        gameManager.currentEvent = 3;
        dialogue.gameObject.SetActive(false);
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator ChangeCameraAndGetUp()
    {
        cameras[0].SetActive(false);
        cameras[1].SetActive(true);
        yield return new WaitForSeconds(1.8f);
        if (endingCutscene) yield break;
        animatorManager.PlayTargetAnimation("Get Up", false);
    }

    private IEnumerator GetSwordAndShow()
    {
        hudManager.StartBattleAnimation();
        yield return new WaitForSeconds(2f);
        if (endingCutscene) yield break;
        dialogue.gameObject.SetActive(true);
    }

    private IEnumerator FadeToBlack()
    {
        brain.m_DefaultBlend.m_Time = 2;
        saltarEscena.SetActive(false);
        blackFade.CrossFadeAlpha(1,1,true);
        yield return new WaitForSeconds(1f);
        animatorManager.PlayTargetAnimation("World", false);
        cameras[5].SetActive(false);
        cameras[6].SetActive(true);
        hudManager.GetCamera();
        yield return new WaitForSeconds(1f);
        minimapa.SetActive(true);
        inputManager.inDialogue = false;
        mixer.SetFloat("EnemiesSFXVolume", 0);
        mixer.SetFloat("EnvironmentSFXVolume", 0);
        musicSource.Play();
        blackFade.CrossFadeAlpha(0,1,true);
        tutorial.SetActive(true);
    }
}