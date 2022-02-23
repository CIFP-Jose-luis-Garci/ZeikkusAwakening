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
    public GameObject pantallaInventario, pantallaEstado, pantallaOpciones, pantallaResultados;
    public GameObject container, tutorialToSpawn;
    public Image flash, blackFade;
    public Button inventario, estado, opciones;
    public Text maru, zeikkuVida, zeikkuMagia;
    public AudioMixer mixer;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void OnEnable()
    {
        GameManager.inPause = true;
        blackFade.gameObject.SetActive(false);
        flash.gameObject.SetActive(false);
        inventario.Select();
        UpdateValues();
        mixer.SetFloat("BGMVolume", GameManager.BGMVolume - 5);
        mixer.SetFloat("EnemiesSFXVolume", -80);
        mixer.SetFloat("EnvironmentSFXVolume", -80);
    }

    private void OnDisable()
    {
        GameManager.inPause = false;
        blackFade.gameObject.SetActive(true);
        blackFade.CrossFadeAlpha(0, 0, true);
        flash.gameObject.SetActive(true);
        flash.CrossFadeAlpha(0, 0, true);
        
        mixer.SetFloat("BGMVolume", GameManager.BGMVolume);
        mixer.SetFloat("EnemiesSFXVolume", 0);
        mixer.SetFloat("EnvironmentSFXVolume", 0);
    }

    private void Start()
    {
        GameManager.SpawnTutorial(container, tutorialToSpawn, null);
        inventario.onClick.AddListener(() =>
        {
            pantallaInventario.SetActive(true);
        });
        estado.onClick.AddListener(() =>
        {
            pantallaEstado.SetActive(true);
        });

        opciones.onClick.AddListener(() =>
        {
            pantallaOpciones.SetActive(true);
        });
        inventario.Select();

    }

    public void UpdateValues()
    {
        zeikkuStats = inputManager.gameObject.GetComponent<Stats>();
        zeikkuVida.text = zeikkuStats.hp + "/" + zeikkuStats.maxHP;
        zeikkuMagia.text = zeikkuStats.mp + "/" + zeikkuStats.maxMP;
        maru.text = GameManager.maru.ToString();
    }

    public bool HasChildrenActive()
    {
        return pantallaEstado.activeSelf || pantallaInventario.activeSelf || pantallaOpciones.activeSelf || pantallaResultados.activeSelf;
    }
}
