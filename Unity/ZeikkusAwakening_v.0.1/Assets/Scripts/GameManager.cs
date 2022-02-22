using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static int currentEvent = 0;
    public static int currentDialogue = 0;
    private static int nextScene = 1;
    public static float BGMVolume = -10;
    public static float SFXVolume = -10;
    public static bool invertCameraX = true;
    public static bool invertCameraY = false;
    public static int cameraSensitivityX = 5;
    public static int cameraSensitivityY = 2;
    public static bool inPause;
    
    [Header("Transiciones")]
    public bool inWorld;
    public PantallaPausaManager pause;
    public GameObject flash;
    public EscenaBatallaManager escenaBatalla;
    
    [Header("Audio")]
    public AudioClip worldMusic;
    public AudioClip battleMusic;
    public AudioClip bossMusic;
    public AudioClip fanfare;
    
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
                resultado *= 2;
        }
        
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
        
        GameObject[] characters = FindObjectOfType<GameManager>().personajes;
        foreach (GameObject character in characters)
        {
            character.GetComponent<Stats>().AddExp(resultado);
            Debug.Log(character.name);
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

    private static int GetTeamLevel()
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

    public void ToBattle(GameObject spawn, bool boss)
    {
        StartCoroutine(LoadBattle(spawn, boss));
    }
    
    public void ToFade(Image blackFade, EscenaBatallaManager escenaBatallaManager)
    {
        StartCoroutine(FadeOutBattle(blackFade, escenaBatallaManager));
    }

    private IEnumerator LoadBattle(GameObject spawn, bool boss)
    {
        flash.SetActive(true);
        HUDManager hudManager = GameObject.FindGameObjectWithTag("UI").GetComponent<HUDManager>();
        AudioSource musicSource = hudManager.GetComponent<AudioSource>();
        yield return CrossFadeMusic(hudManager.mixer, 1, true);
        musicSource.Stop();
        if (boss)
            musicSource.clip = bossMusic;
        else
            musicSource.clip = battleMusic;
        musicSource.Play();
        musicSource.loop = true;
        escenaBatalla.enemyToSpawn = spawn;
        escenaBatalla.gameObject.SetActive(true);
        personajes[0].GetComponent<InputManager>().StartBattle();
        yield return CrossFadeMusic(hudManager.mixer, 1, false);
        foreach (EnemyBattleManager enemy in FindObjectsOfType<EnemyBattleManager>())
        {
            enemy.battleStarted = true;
        }
        flash.SetActive(false);
    }

    private IEnumerator FadeOutBattle(Image blackFade, EscenaBatallaManager escenaBatallaManager)
    {
        // press a, goto transition fade in black
        blackFade.CrossFadeAlpha(1, 1, true);
        HUDManager hudManager = FindObjectOfType<Canvas>().GetComponent<HUDManager>();
        yield return CrossFadeMusic(hudManager.mixer, 1, true);
        escenaBatallaManager.ResetPlayer();
        AudioSource musicSource = hudManager.GetComponent<AudioSource>();
        musicSource.Stop();
        musicSource.clip = worldMusic;
        musicSource.Play();
        yield return CrossFadeMusic(hudManager.mixer, 1, false);
        escenaBatallaManager.gameObject.SetActive(false);
        inPause = false;
        blackFade.CrossFadeAlpha(0, 1, true);
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
