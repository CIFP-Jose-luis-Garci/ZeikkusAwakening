using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Cutscene1Manager : CutsceneManager
{
    private AnimatorManager animatorManager;
    private InputManager inputManager;
    public Image blackFade;
    public Material faceEyesOpen;
    public SkinnedMeshRenderer face;
    public AudioSource musicSource;
    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
        inputManager = animatorManager.GetComponent<InputManager>();
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
        Debug.Log(dialogueCount);
        switch (dialogueCount)
        {
            case 1:
                Debug.Log(dialogueCount);
                Material[] materials = face.materials;
                materials[1] = faceEyesOpen;
                face.materials = materials;
                break;
            case 5:
                StartCoroutine(ChangeCameraAndGetUp());
                break;
            case 7:
                cameras[1].SetActive(false);
                cameras[2].SetActive(true);
                break;
            case 12:
                cameras[2].SetActive(false);
                cameras[3].SetActive(true);
                break;
            case 13:
                StartCoroutine(GetSwordAndShow());
                break;
            case 16:
                inputManager.WinBattle();
                break;
            case 17:
                StartCoroutine(FadeToBlack());
                break;
            default:
                break;
        }
    }

    public override void EndCutScene()
    {
        Material[] materials = face.materials;
        materials[1] = faceEyesOpen;
        face.materials = materials;
        foreach (GameObject camera in cameras)
        {
            camera.SetActive(false);
        }

        GameManager.currentDialogue = 18;
        GameManager.currentEvent = 3;
        dialogue.gameObject.SetActive(false);
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator ChangeCameraAndGetUp()
    {
        cameras[0].SetActive(false);
        cameras[1].SetActive(true);
        yield return new WaitForSeconds(1.8f);
        animatorManager.PlayTargetAnimation("Get Up", false);
    }

    private IEnumerator GetSwordAndShow()
    {
        //animatorManager.animator.CrossFade("Get Sword", 0.05f);
        inputManager.StartBattle();
        yield return new WaitForSeconds(2f);
        dialogue.gameObject.SetActive(true);
    }

    private IEnumerator FadeToBlack()
    {
        blackFade.CrossFadeAlpha(1,1,true);
        yield return new WaitForSeconds(1f);
        animatorManager.PlayTargetAnimation("Empty", false);
        cameras[3].SetActive(false);
        cameras[4].SetActive(true);
        yield return new WaitForSeconds(1f);
        inputManager.inDialogue = false;
        mixer.SetFloat("EnemiesSFXVolume", 0);
        mixer.SetFloat("EnvironmentSFXVolume", 0);
        musicSource.Play();
        blackFade.CrossFadeAlpha(0,1,true);
    }
}