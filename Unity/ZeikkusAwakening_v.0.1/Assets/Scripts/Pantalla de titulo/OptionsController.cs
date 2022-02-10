using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public Slider bgmSlider, sfxSlider;
    public Button exit;
    public AudioMixer mixer;
    public GameObject selecciones, gameLogo;

    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(delegate { SetSound("BGMVolume", bgmSlider.value); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSound("SFXVolume", sfxSlider.value); });
        exit.onClick.AddListener(() =>
        {
            Transform uiTransform = GameObject.FindGameObjectWithTag("UI").transform;
            GameObject seleccionesInstanciadas = Instantiate(selecciones, uiTransform);
            seleccionesInstanciadas.GetComponentInChildren<Button>().Select();
            Instantiate(gameLogo, uiTransform);
            Destroy(gameObject);
        });
    }
    
    public void SetSound(string param, float soundLevel)
    {
        mixer.SetFloat(param, soundLevel);
    }
}
