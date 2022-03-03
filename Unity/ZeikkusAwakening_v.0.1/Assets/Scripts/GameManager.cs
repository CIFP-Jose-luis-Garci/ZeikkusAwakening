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
    public AudioSource source;
    public PlayerBagManager bag;
    
    [Header("Dialogos")]
    public int currentEvent = 0;
    public int currentDialogue = 0;
    public string talking;
    private int nextScene = 0;
    
    [Header("Configuración")]
    public float BGMVolume = -10;
    public float SFXVolume = -10;
    public float voiceVolume = -5;
    public bool invertCameraX = true;
    public bool invertCameraY = false;
    public int cameraSensitivityX = 5;
    public int cameraSensitivityY = 2;
    public bool inPause;
    
    [Header("Transiciones")]
    public bool inWorld;
    public bool transitioning;
    public bool viewingMinimap;
    public bool inCutscene;
    public PantallaPausaManager pause;
    
    [Header("Datos de juego")]
    public int maru = 1000;
    public GameObject[] personajes;
    public Vector3 checkpoint;
    public int dungeonLevel;
    public float playtime;
    public int totalDamage;
    public bool bossDefeated;
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            // You could also log a warning.
        }
    }
    public bool Pause()
    {
        if (!pause.HasChildrenActive() && !transitioning && !viewingMinimap)
        {
            inPause = !inPause;
            pause.gameObject.SetActive(inPause);
            return true;
        }
        return false;
    }

    public IEnumerator CrossFadeMusic(AudioMixer mixer, float time, bool muting)
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

    public int CalcPhysDamage(Stats playerStats, Stats enemyStats, float baseDamage, bool forceCrit)
    {
        return CalcDamage(playerStats.strength, enemyStats.defense, baseDamage, forceCrit);
    }

    public int CalcSpecDamage(Stats playerStats, Stats enemyStats, float baseDamage, bool forceCrit)
    {
        return CalcDamage(playerStats.magicPower, enemyStats.resistance, baseDamage, forceCrit);
    }

    private int CalcDamage(int power, int defense, float baseDamage, bool forceCrit)
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

    public string CalcExp(EnemyStats[] enemies, PantallaResultadosManager resultados)
    {
        int resultado = 0;
        foreach (EnemyStats current in enemies)
        {
            float exp = ((float)current.level / GetTeamLevel()) * current.expBase;
            resultado += (int) exp;
        }
        GameObject[] characters = Instance.personajes;
        int countLevels = 0;
        foreach (GameObject character in characters)
        {
            if (character)
            {
                resultados.levelUps[countLevels] = character.GetComponent<Stats>().AddExp(resultado);
                countLevels++;
            }
        }
        return resultado.ToString();
    }

    public string CalcMaru(EnemyStats[] enemies)
    {
        int totalMaru = 0;
        foreach (EnemyStats current in enemies)
        {
            totalMaru += (int)(current.marubase * Random.Range(0.85f, 1.1f));
        }

        maru += totalMaru;
        return totalMaru.ToString();
    }

    private int GetTeamLevel()
    {
        GameObject[] characters = personajes;
        int level = 0;
        int count = 0;
        foreach (GameObject character in characters)
        {
            if (character)
            {
                level = character.GetComponent<Stats>().level;
                count++;
            }
        }
        
        level /= count;
        if (level < 1)
            level = 1;
        
        return level;
    }
    
    
    public void SpawnTutorial(GameObject container, GameObject tutorialToSpawn, GameObject caller)
    {
        TutorialManager contained = container.GetComponentInChildren<TutorialManager>();
        if (contained)
            Destroy(contained.gameObject);
        Instantiate(tutorialToSpawn, container.transform);
        if (caller)
            Destroy(caller);
        
    }

    public IEnumerator LoadScene(float timeToLoad, bool isMemory = false)
    {
        yield return new WaitForSeconds(timeToLoad);
        AsyncOperation asyncOperation;
        if (isMemory)
        {
            asyncOperation = SceneManager.LoadSceneAsync(1);
        }
        else
        {
            nextScene++;
            asyncOperation = SceneManager.LoadSceneAsync(nextScene);
        }
        while (!asyncOperation.isDone)
            yield return null;
    }
}
