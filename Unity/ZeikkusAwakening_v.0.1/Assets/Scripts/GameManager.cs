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
        return CalcDamage(playerStats.strength, enemyStats.defense, baseDamage);
    }

    public static int CalcSpecDamage(Stats playerStats, Stats enemyStats, float baseDamage)
    {
        return CalcDamage(playerStats.magicPower, enemyStats.resistance, baseDamage);
    }

    private static int CalcDamage(int power, int defense, float baseDamage)
    {
        float resultado = 0.2f * 2;
        resultado += 1;
        resultado *= power;
        resultado *= baseDamage;
        resultado /= (25 * defense);
        resultado += 2;
        float random = Random.Range(85, 100);
        resultado *= random;
        resultado *= 0.01f;
        resultado *= 5;
        
        return (int) resultado;
    }

    public static string CalcExp(Stats[] enemies)
    {
        int resultado = 0;
        foreach (Stats current in enemies)
        {
            float baseExp = current.expBase * current.level;
            baseExp /= 5;
            float aCorrector = Mathf.Pow(10 * current.level, 2.5f);
            float bCorrector = (current.level * GetTeamLevel() + 10);
            float total = baseExp * (aCorrector / bCorrector) + 1;
            resultado += (int) total;
        }
        
        return resultado.ToString();
    }

    public static string CalcMaru(Stats[] enemies)
    {
        int totalMaru = 0;
        foreach (Stats current in enemies)
        {
            totalMaru += (int)(current.marubase * Random.Range(0.85f, 1.1f));
        }
        
        return totalMaru.ToString();
    }

    private static int GetTeamLevel()
    {
        GameObject[] characters = FindObjectOfType<GameManager>().personajes;
        int level = 0;
        foreach (GameObject character in characters)
        {
            if (character)
                level = character.GetComponent<Stats>().level;
        }
        
        level /= characters.Length;
        
        return level;
    }

    public void ToBattle(GameObject spawn)
    {
        StartCoroutine(LoadBattle(spawn));
    }

    private IEnumerator LoadBattle(GameObject spawn)
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(1f);
        HUDManager hudManager = GameObject.FindGameObjectWithTag("UI").GetComponent<HUDManager>();
        escenaBatalla.enemyToSpawn = spawn;
        escenaBatalla.gameObject.SetActive(true);
        AudioSource musicSource = hudManager.GetComponent<AudioSource>();
        musicSource.Stop();
        musicSource.clip = battleMusic;
        musicSource.Play();
        yield return StartCoroutine(personajes[0].GetComponent<InputManager>().StartBattle());
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
