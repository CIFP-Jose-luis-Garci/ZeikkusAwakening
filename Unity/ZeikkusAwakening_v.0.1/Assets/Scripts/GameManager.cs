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
    public HUDManager hudManager;
    public PantallaPausaManager pause;
    public GameObject resultScreen;
    public FlashManager flash;
    public EscenaBatallaManager escenaBatalla;
    public CinemachineFreeLook cmfl;
    public float cameraXAngle;
    
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
        
        if (!muting)
            mixer.SetFloat("BGMVolume", -10f);

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

    public static string CalcExp(Stats[] enemies)
    {
        int resultado = 0;
        foreach (Stats current in enemies)
        {
            float baseExp = current.expBase * current.level;
            baseExp /= 5;
            float aCorrector = Mathf.Pow(4 * current.level, 2.5f);
            float bCorrector = (current.level * GetTeamLevel() + 10);
            float total = baseExp * (aCorrector / bCorrector) + 1;
            resultado += (int) total;
        }
        
        GameObject[] characters = FindObjectOfType<GameManager>().personajes;
        foreach (GameObject character in characters)
        {
            character.GetComponent<Stats>().AddExp(resultado);
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

    public void StartBattle(GameObject worldEnemy, bool isBoss, int enemyAdvantage = 1)
    {
        Debug.Log("hola");
        if (inPause) return;
        Debug.Log("halo");
        inPause = true;
        winning = false;
        ToBattle(worldEnemy, isBoss, enemyAdvantage);
    }

    public void ToBattle(GameObject spawn, bool boss, int enemyAdvantage)
    {
        StartCoroutine(LoadBattle(spawn, boss, enemyAdvantage));
    }

    private IEnumerator LoadBattle(GameObject worldEnemy, bool boss, int enemyAdvantage)
    {
        flash.gameObject.SetActive(true);
        switch (enemyAdvantage)
        {
            case 0:
                BattleAdvantageText("¡Asalto enemigo!", new Color(0.75f, 0.066f, 0.066f));
                break;
            case 1:
                BattleAdvantageText("¡Adelante!", Color.white);
                break;
            case 2:
                BattleAdvantageText("¡Emboscada!", new Color(0.09f, 0.82f, 0.31f));
                break;
        }
        flash.AnimateStart();
        
        cameraXAngle = cmfl.m_XAxis.Value;
        yield return CrossFadeMusic(hudManager.mixer, 1, true);
        if (boss)
            ChangeMusic(bossMusic);
        else 
            ChangeMusic(battleMusic);
        escenaBatalla.enemyToSpawn = worldEnemy.GetComponent<EnemyManager>().enemyToSpawn;
        escenaBatalla.enemyAdvantage = enemyAdvantage;
        escenaBatalla.gameObject.SetActive(true);
        Destroy(worldEnemy);
        personajes[0].GetComponent<InputManager>().StartBattle();
        yield return CrossFadeMusic(hudManager.mixer, 1, false);
        escenaBatalla.EnemiesStart();
        inPause = false;
        winning = false;
    }

    private void BattleAdvantageText(string text, Color color)
    {
        Text textoCarga = flash.GetComponentInChildren<Text>();
        textoCarga.text = text;
        textoCarga.color = color;
    }

    public void ToWinBattle()
    {
        StartCoroutine(WinBattle());
    }

    private IEnumerator WinBattle()
    {
        personajes[0].GetComponent<InputManager>().WinBattle();
        ChangeMusic(fanfare, false);
        yield return new WaitForSeconds(1.2f);
        CameraManager cameraManager = cmfl.GetComponent<CameraManager>();
        cameraManager.ChangeTarget(personajes[0].transform);
        cameraManager.ResetRadius();
        resultScreen.SetActive(true);
    }
    
    public void ToFadeBattle(Image blackFade, EscenaBatallaManager escenaBatallaManager)
    {
        StartCoroutine(FadeOutBattle(blackFade, escenaBatallaManager));
    }

    private IEnumerator FadeOutBattle(Image blackFade, EscenaBatallaManager escenaBatallaManager)
    {
        // press a, goto transition fade in black
        blackFade.CrossFadeAlpha(1, 1, true);
        yield return CrossFadeMusic(hudManager.mixer, 1, true);
        cmfl.m_XAxis.Value = cameraXAngle;
        ChangeMusic(worldMusic);
        escenaBatallaManager.ResetPlayer();
        yield return CrossFadeMusic(hudManager.mixer, 1, false);
        escenaBatallaManager.gameObject.SetActive(false);
        inPause = false;
        winning = false;
        blackFade.CrossFadeAlpha(0, 1, true);
    }

    private void ChangeMusic(AudioClip clip, bool isLooping = true)
    {
        AudioSource musicSource = hudManager.GetComponent<AudioSource>();
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
        musicSource.loop = isLooping;
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
