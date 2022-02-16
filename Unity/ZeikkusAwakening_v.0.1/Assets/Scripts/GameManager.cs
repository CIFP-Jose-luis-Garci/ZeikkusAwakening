using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool inWorld;
    public static int currentEvent = 0;
    public static int currentDialogue = 0;
    private static int nextScene = 0;
    public static float BGMVolume = -10;
    public static float SFXVolume = -10;
    public static bool invertCameraX = true;
    public static bool invertCameraY = false;
    public GameObject pause;
    public GameObject mundo;
    public GameObject flash;
    public GameObject escenaBatalla;
    public int maru;
    public GameObject[] personajes;
    
    public void Pause()
    {
        pause.SetActive(!pause.activeSelf);
    }

    public static IEnumerator CrossFadeMusic(AudioMixer mixer, float time, bool muting)
    {
        float current;
        float timeLoop = time * 10;
        float valueStep = -80 / timeLoop;
        if (!muting) valueStep *= -1;
        float timeStep = 0.1f;
        mixer.GetFloat("BGMVolume", out current);

        if (!muting && current > -80) yield break;
        if (muting && current < BGMVolume) yield break;
        do
        {
            current += valueStep;
            mixer.SetFloat("BGMVolume", current);
            yield return new WaitForSeconds(timeStep);
            time -= timeStep;
        } while (time > 0);

    }

    public void ToBattle()
    {
        StartCoroutine(LoadBattle());
    }

    private IEnumerator LoadBattle()
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        mundo.SetActive(false);
        escenaBatalla.SetActive(true);
        StartCoroutine(personajes[0].GetComponent<InputManager>().StartBattle());
        yield return new WaitForSeconds(3f);
        escenaBatalla.transform.parent = null;
        flash.SetActive(false);
        Debug.Log("hola");
    }

    public static IEnumerator LoadScene(float timeToLoad)
    {
        nextScene++;
        yield return new WaitForSeconds(timeToLoad);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(GameManager.nextScene);
        while (!asyncOperation.isDone)
            yield return null;
    }
}
