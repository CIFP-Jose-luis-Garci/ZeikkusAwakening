using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static int currentEvent = 0;
    public static int currentDialogue = 0;
    private static int nextScene = 0;
    public static float BGMVolume = -10;
    public static float SFXVolume = -10;
    public static bool invertCameraX = true;
    public static bool invertCameraY = false;
    public static bool inPause;
    
    [Header("Transiciones")]
    public bool inWorld;
    public GameObject pause;
    public GameObject flash;
    public EscenaBatallaManager escenaBatalla;
    
    [Header("Audio")]
    public AudioClip battleMusic;
    public AudioClip worldMusic;
    
    [Header("Datos de juego")]
    public int maru;
    public GameObject[] personajes;
    
    public void Pause()
    {
        inPause = !inPause;
        pause.SetActive(inPause);
        Time.timeScale = inPause ? 0 : 1;
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

    public static int CalcPhysDamage(Stats playerStats, Stats enemyStats, float baseDamage)
    {
        float resultado = 0.2f * 2;
        resultado += 1;
        resultado *= playerStats.strength;
        resultado *= baseDamage;
        resultado /= (25 * enemyStats.defense);
        resultado += 2;
        float random = Random.Range(85 ,100);
        resultado *= random;
        resultado *= 0.01f;
        resultado *= 5;
        return (int) resultado;
    }

    public void ToBattle(GameObject spawn)
    {
        StartCoroutine(LoadBattle(spawn));
    }

    public void ToWin()
    {
        StartCoroutine(WinBattle());
    }

    private IEnumerator LoadBattle(GameObject spawn)
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(1f);
        HUDManager hudManager = FindObjectOfType<Canvas>().GetComponent<HUDManager>();
        escenaBatalla.enemyToSpawn = spawn;
        escenaBatalla.gameObject.SetActive(true);
        AudioSource musicSource = hudManager.GetComponent<AudioSource>();
        musicSource.Stop();
        musicSource.clip = battleMusic;
        musicSource.Play();
        yield return StartCoroutine(personajes[0].GetComponent<InputManager>().StartBattle());
        flash.SetActive(false);
    }

    private IEnumerator WinBattle()
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(1f);
        HUDManager hudManager = FindObjectOfType<Canvas>().GetComponent<HUDManager>();
        escenaBatalla.gameObject.SetActive(false);
        AudioSource musicSource = hudManager.GetComponent<AudioSource>();
        musicSource.Stop();
        musicSource.clip = worldMusic;
        musicSource.Play();
        yield return StartCoroutine(personajes[0].GetComponent<InputManager>().WinBattle());
        flash.SetActive(false);
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
