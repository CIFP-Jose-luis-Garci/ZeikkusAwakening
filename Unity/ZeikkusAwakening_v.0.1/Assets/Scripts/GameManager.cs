using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Dialogos")]
    public static int currentEvent = 0;
    public static int currentDialogue = 0;
    public static string talking;
    private static int nextScene = 0;
    
    [Header("Configuración")]
    public static float BGMVolume = -10;
    public static float SFXVolume = -10;
    public static float voiceVolume = -5;
    public static bool invertCameraX = true;
    public static bool invertCameraY = false;
    public static int cameraSensitivityX = 5;
    public static int cameraSensitivityY = 2;
    public static bool inPause;
    
    [Header("Transiciones")]
    public bool inWorld;
    public static bool winning;
    public PantallaPausaManager pause;
    
    [Header("Datos de juego")]
    public static int maru = 1000;
    public GameObject[] personajes;

    public bool Pause()
    {
        if (!pause.HasChildrenActive())
        {
            inPause = !inPause;
            pause.gameObject.SetActive(inPause);
            return true;
        }
        return false;
    }

    public static IEnumerator CrossFadeMusic(AudioMixer mixer, float time, bool muting)
    {
        float current;
        float min = -40;
        float timeLoop = time * 10;
        float valueStep = (min - BGMVolume) / timeLoop;
        if (!muting) valueStep *= -1;
        float timeStep = 0.1f;
        mixer.GetFloat("BGMVolume", out current);
        if (!muting && current < min)
            current = min;

        if (!muting && current > min) yield break;
        if (muting && current < BGMVolume) yield break;
        while (time > 0)
        {
            current += valueStep;
            mixer.SetFloat("BGMVolume", current);
            yield return new WaitForSeconds(timeStep);
            time -= timeStep;
        }
        
        if (!muting)
            mixer.SetFloat("BGMVolume", BGMVolume);
        else
            mixer.SetFloat("BGMVolume", min);
            

    }

    public static int CalcPhysDamage(Stats playerStats, Stats enemyStats, float baseDamage, bool forceCrit)
    {
        return CalcDamage(playerStats.strength, enemyStats.defense, baseDamage, forceCrit);
    }

    public static int CalcSpecDamage(Stats playerStats, Stats enemyStats, float baseDamage, bool forceCrit)
    {
        return CalcDamage(playerStats.magicPower, enemyStats.resistance, baseDamage, forceCrit);
    }

    private static int CalcDamage(int power, int defense, float baseDamage, bool forceCrit)
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
        if (forceCrit)
            resultado *= 2;
        else
        {
            float chance = Random.Range(0f, 1f);
            if (chance < 0.1f)
            {
                resultado *= 2;
            }
        }
        
        return (int) resultado;
    }

    public static string CalcExp(Stats[] enemies, PantallaResultadosManager resultados)
    {
        int resultado = 0;
        foreach (Stats current in enemies)
        {
            float baseExp = current.expBase * current.level;
            baseExp /= 5;
            float aCorrector = Mathf.Pow(2 * current.level, 2.5f);
            float bCorrector = (current.level * GetTeamLevel() + 10);
            float total = baseExp * (aCorrector / bCorrector) + 1;
            resultado += (int) total;
        }
        
        GameObject[] characters = FindObjectOfType<GameManager>().personajes;
        int countLevels = 0;
        foreach (GameObject character in characters)
        {
            resultados.levelUps[countLevels] = character.GetComponent<Stats>().AddExp(resultado);
            countLevels++;
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

        maru += totalMaru;
        return totalMaru.ToString();
    }

    public static int GetTeamLevel()
    {
        GameObject[] characters = FindObjectOfType<GameManager>().personajes;
        int level = 0;
        foreach (GameObject character in characters)
        {
            level = character.GetComponent<Stats>().level;
        }
        
        level /= characters.Length;
        if (level < 1)
            level = 1;
        
        return level;
    }
    
    
    public static void SpawnTutorial(GameObject container, GameObject tutorialToSpawn, GameObject caller)
    {
        TutorialManager contained = container.GetComponentInChildren<TutorialManager>();
        if (contained)
            Destroy(contained.gameObject);
        Instantiate(tutorialToSpawn, container.transform);
        if (caller)
            Destroy(caller);
        
    }

    public static IEnumerator LoadScene(float timeToLoad)
    {
        nextScene++;
        yield return new WaitForSeconds(timeToLoad);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextScene);
        while (!asyncOperation.isDone)
            yield return null;
    }
}
