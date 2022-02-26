using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private float time;
    public Text tiempo;
    public Image blackBackground;

    [Header("Transitions")] 
    private AnimatorManager animatorManager;
    private EscenaBatallaManager escenaBatalla;
    private CinemachineFreeLook cmfl;
    private float cameraXAngle;
    private bool inBattle;
    public AudioMixer mixer;
    public GameObject resultScreen;
    public FlashManager flash;
    
    [Header("Audio")]
    public AudioClip worldMusic;
    public AudioClip battleMusic;
    public AudioClip bossMusic;
    public AudioClip fanfare;

    private void Start()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
        escenaBatalla = FindObjectOfType<EscenaBatallaManager>();
        StartCoroutine(GameManager.CrossFadeMusic(mixer, 2, false));
        blackBackground.CrossFadeAlpha(0,2,true);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (tiempo.gameObject.activeInHierarchy)
            tiempo.text = TiempoActual();
    }
    
    private string TiempoActual()
    {
    
        string horas = Mathf.Floor((time / 60) / 24).ToString("00");
        string minutos = Mathf.Floor(time / 60).ToString("00");
        string segundos = Mathf.Floor(time % 60).ToString("00");
    
        return horas + ":" + minutos + ":" + segundos;
    }

    public void GetCamera()
    {
        cmfl = FindObjectOfType<CinemachineFreeLook>();
    }
    
    public void StartBattle(GameObject worldEnemy, bool isBoss, int enemyAdvantage = 1)
    {
        if (inBattle) return;
        inBattle = true;
        GameManager.inPause = true;
        GameManager.winning = false;
        ToBattle(worldEnemy, isBoss, enemyAdvantage);
    }

    private void ToBattle(GameObject spawn, bool boss, int enemyAdvantage)
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
        yield return GameManager.CrossFadeMusic(mixer, 1, true);
        if (boss)
            ChangeMusic(bossMusic);
        else 
            ChangeMusic(battleMusic);
        escenaBatalla.enemyToSpawn = worldEnemy.GetComponent<EnemyManager>().enemyToSpawn;
        escenaBatalla.enemyAdvantage = enemyAdvantage;
        escenaBatalla.ControlScene();
        Destroy(worldEnemy);
        StartBattleAnimation();
        yield return GameManager.CrossFadeMusic(mixer, 1, false);
        escenaBatalla.EnemiesStart();
        GameManager.inPause = false;
        GameManager.winning = false;
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
        WinBattleAnimation();
        ChangeMusic(fanfare, false);
        yield return new WaitForSeconds(1f);
        CameraManager cameraManager = cmfl.GetComponent<CameraManager>();
        cameraManager.ChangeTarget(animatorManager.transform);
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
        yield return GameManager.CrossFadeMusic(mixer, 1, true);
        cmfl.m_XAxis.Value = cameraXAngle;
        ChangeMusic(worldMusic);
        escenaBatallaManager.ResetPlayer();
        yield return GameManager.CrossFadeMusic(mixer, 1, false);
        GameManager.inPause = false;
        GameManager.winning = false;
        inBattle = false;
        blackFade.CrossFadeAlpha(0, 1, true);
    }

    private void ChangeMusic(AudioClip clip, bool isLooping = true)
    {
        AudioSource musicSource = GetComponent<AudioSource>();
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
        musicSource.loop = isLooping;
    }

    public void StartBattleAnimation()
    {
        animatorManager.PlayTargetAnimation("DrawSword", true, false, 0.05f);
        animatorManager.ChangeWorld(false);
    }

    public void WinBattleAnimation()
    {
        animatorManager.PlayTargetAnimation("WinBattle", true);
        animatorManager.ChangeWorld(true);
    }
}
