using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private GameManager gameManager;

    public Text tiempo;
    public Image blackFade;

    [Header("Transitions")] 
    private AnimatorManager animatorManager;
    private PlayerLocomotion playerLocomotion;
    private InputManager inputManager;
    private EscenaBatallaManager escenaBatalla;
    [NonSerialized] public CinemachineFreeLook cmfl;
    private float cameraXAngle;
    private bool inBattle;
    public AudioMixer mixer;
    public GameObject resultScreen, minimap, checkpointPopup, hudPersonajes, loading;
    public FlashManager flash;
    public InterfazBatallaManager interfazBatalla;
    public InterfazMundoManager interfazMundo;
    public PantallaPausaManager pantallaPausa;
    public GameObject tutorialContainer;
    
    [Header("Audio")]
    public AudioClip worldMusic;
    public AudioClip battleMusic;
    public AudioClip bossMusic;
    public AudioClip fanfare;
    
    public static HUDManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            // You could also log a warning.
        }
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        inputManager = InputManager.Instance;
        animatorManager = inputManager.GetComponent<AnimatorManager>();
        playerLocomotion = animatorManager.GetComponent<PlayerLocomotion>();
        escenaBatalla = FindObjectOfType<EscenaBatallaManager>();
        StartCoroutine(gameManager.CrossFadeMusic(mixer, 2, false));
        blackFade.CrossFadeAlpha(0,2,true);
    }

    // Update is called once per frame
    void Update()
    {
        gameManager.playtime += Time.deltaTime;
        if (tiempo.gameObject.activeInHierarchy)
            tiempo.text = TiempoActual();
    }
    
    private string TiempoActual()
    {
        float time = GameManager.Instance.playtime;
        int hours = Mathf.FloorToInt(time / 60 / 60);
        int minutes = Mathf.FloorToInt(time / 60) % 60;
        int seconds = Mathf.FloorToInt(time % 60);
        string horas = hours.ToString("00");
        string minutos = minutes.ToString("00");
        string segundos = seconds.ToString("00");

        return horas + ":" + minutos + ":" + segundos;
    }

    public void GetCamera()
    {
        cmfl = FindObjectOfType<CinemachineFreeLook>();
    }
    
    public void StartBattle(GameObject worldEnemy, bool isBoss, int enemyAdvantage = 1)
    {
        if (inBattle || gameManager.inPause || gameManager.transitioning || inputManager.inDialogue || !playerLocomotion.isGrounded) return;
        inBattle = true;
        gameManager.inPause = true;
        gameManager.transitioning = true;
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
        yield return gameManager.CrossFadeMusic(mixer, 1, true);
        cmfl.m_XAxis.Value = -160;
        cmfl.m_YAxis.Value = 0.6f;
        if (boss)
            ChangeMusic(bossMusic);
        else 
            ChangeMusic(battleMusic);
        escenaBatalla.worldEnemyPosition = worldEnemy.transform.position;
        escenaBatalla.enemyToSpawn = worldEnemy.GetComponent<EnemyManager>().enemyToSpawn;
        escenaBatalla.enemyAdvantage = enemyAdvantage;
        interfazBatalla.ActivateSlots();
        escenaBatalla.ControlScene(interfazBatalla.slotsEnemigos, boss);
        Destroy(worldEnemy); 
        minimap.SetActive(false);
        StartBattleAnimation();
        yield return gameManager.CrossFadeMusic(mixer, 1, false);
        interfazBatalla.gameObject.SetActive(true);
        escenaBatalla.EnemiesStart();
        gameManager.inPause = false;
        gameManager.transitioning = false;
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
        interfazBatalla.Retract();
        ChangeMusic(fanfare, false);
        yield return new WaitForSeconds(1f);
        CameraManager cameraManager = cmfl.GetComponent<CameraManager>();
        cameraManager.ChangeTarget(animatorManager.transform);
        cameraManager.ResetRadius();
        gameManager.totalDamage += escenaBatalla.danoTotal;
        resultScreen.SetActive(true);
    }
    
    public void ToFadeBattle(EscenaBatallaManager escenaBatallaManager)
    {
        StartCoroutine(FadeOutBattle(escenaBatallaManager));
    }

    private IEnumerator FadeOutBattle(EscenaBatallaManager escenaBatallaManager)
    {
        // press a, goto transition fade in black
        blackFade.CrossFadeAlpha(1, 1, true);
        yield return gameManager.CrossFadeMusic(mixer, 1, true);
        cmfl.m_XAxis.Value = cameraXAngle;
        ChangeMusic(worldMusic);
        minimap.SetActive(true);
        escenaBatallaManager.ResetPlayer();
        yield return gameManager.CrossFadeMusic(mixer, 1, false);
        gameManager.inPause = false;
        gameManager.transitioning = false;
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

    public void ShowCheckpointArrived()
    {
        Stats playerStats = animatorManager.GetComponent<Stats>();
        playerStats.hp = playerStats.maxHP;
        playerStats.mp = playerStats.maxMP;
        Destroy(Instantiate(checkpointPopup, transform), 2f);
    }

    public void ToDieInBattle(Transform player, Stats stats, GameObject deathVolume)
    {
        StartCoroutine(DieInBattle(player, stats, deathVolume));
    }

    private IEnumerator DieInBattle(Transform player, Stats stats, GameObject deathVolume)
    {
        DisableHUD();
        yield return gameManager.CrossFadeMusic(mixer, 1, true);
        blackFade.CrossFadeAlpha(1, 2f, true);
        yield return new WaitForSeconds(3f);
        CameraManager cameraManager = cmfl.GetComponent<CameraManager>();
        cameraManager.ChangeTarget(animatorManager.transform);
        cameraManager.ResetRadius();
        deathVolume.SetActive(false);
        player.position = gameManager.checkpoint;
        Time.timeScale = 1;
        Instantiate(escenaBatalla.worldEnemy, escenaBatalla.worldEnemyPosition, Quaternion.Euler(0, 90,0));
        animatorManager.PlayTargetAnimation("Stand Up", true);
        animatorManager.ChangeWorld(true);
        escenaBatalla.Purge();
        ChangeMusic(worldMusic);
        stats.hp = stats.maxHP;
        stats.alive = true;
        blackFade.CrossFadeAlpha(0, 2, true);
        yield return new WaitForSeconds(8f);
        yield return gameManager.CrossFadeMusic(mixer, 2, false);
        interfazMundo.gameObject.SetActive(true);
        hudPersonajes.SetActive(true);
        minimap.SetActive(true);
        gameManager.transitioning = false;
        inBattle = false;

    }

    public void FinishLevel()
    {
        gameManager.transitioning = true;
        blackFade.CrossFadeAlpha(1, 0.3f, true);
        loading.SetActive(true);
        StartCoroutine(gameManager.LoadScene(1, true));
    }

    private void DisableHUD()
    {
        interfazBatalla.gameObject.SetActive(false);
        interfazMundo.gameObject.SetActive(false);
        hudPersonajes.SetActive(false);
    }

    public void ChangeZoneName(string zoneName)
    {
        interfazMundo.ChangeZoneName(zoneName);
    }
}
