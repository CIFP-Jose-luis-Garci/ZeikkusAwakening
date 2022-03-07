using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PantallaPausaManager : MonoBehaviour
{
    private InputManager inputManager;
    private Stats zeikkuStats;
    private AudioSource source;
    private GameManager gameManager;
    public GameObject pantallaInventario, pantallaEstado, pantallaOpciones, pantallaResultados;
    public GameObject container, tutorialToSpawn;
    public VidaSliderManager sliderVida;
    public MagiaSliderManager sliderMagia;
    public Image flash, blackFade;
    public Button inventario, magia, equipamiento, estado, opciones;
    public Text maru, zeikkuVida, zeikkuMagia;
    public AudioMixer mixer;
    public AudioClip sonidoAbirMenu, sonidoCerrarMenu, sonidoClickMenu, sonidoNoPosible;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        gameManager = GameManager.Instance;
        source = gameManager.source;
    }

    private void OnEnable()
    {
        gameManager.inPause = true;
        source.PlayOneShot(sonidoAbirMenu);
        blackFade.gameObject.SetActive(false);
        flash.gameObject.SetActive(false);
        inventario.Select();
        UpdateValues();
        mixer.SetFloat("BGMVolume", gameManager.BGMVolume - 5);
        mixer.SetFloat("EnemiesSFXVolume", -80);
        mixer.SetFloat("EnvironmentSFXVolume", -80);
        mixer.SetFloat("BossSFXVolume", -80);
    }

    private void OnDisable()
    {
        gameManager.inPause = false;
        source.PlayOneShot(sonidoCerrarMenu);
        blackFade.gameObject.SetActive(true);
        blackFade.CrossFadeAlpha(0, 0, true);
        
        mixer.SetFloat("BGMVolume", gameManager.BGMVolume);
        mixer.SetFloat("EnemiesSFXVolume", 0);
        mixer.SetFloat("EnvironmentSFXVolume", 0);
        mixer.SetFloat("BossSFXVolume", 0);
    }

    private void Start()
    {
        gameManager.SpawnTutorial(container, tutorialToSpawn, null);
        inventario.onClick.AddListener(() =>
        {
            source.PlayOneShot(sonidoClickMenu);
            pantallaInventario.SetActive(true);
        });
        equipamiento.onClick.AddListener(() => source.PlayOneShot(sonidoNoPosible));
        magia.onClick.AddListener(() => source.PlayOneShot(sonidoNoPosible));
        estado.onClick.AddListener(() =>
        {
            source.PlayOneShot(sonidoClickMenu);
            pantallaEstado.SetActive(true);
        });

        opciones.onClick.AddListener(() =>
        {
            source.PlayOneShot(sonidoClickMenu);
            pantallaOpciones.SetActive(true);
        });
        inventario.Select();

    }

    public void UpdateValues()
    {
        zeikkuStats = inputManager.gameObject.GetComponent<Stats>();
        zeikkuVida.text = zeikkuStats.hp + "/" + zeikkuStats.maxHP;
        zeikkuMagia.text = zeikkuStats.mp + "/" + zeikkuStats.maxMP;
        maru.text = gameManager.maru.ToString();
    }

    public bool HasChildrenActive()
    {
        return pantallaEstado.activeSelf || pantallaInventario.activeSelf || pantallaOpciones.activeSelf || pantallaResultados.activeSelf;
    }
}
