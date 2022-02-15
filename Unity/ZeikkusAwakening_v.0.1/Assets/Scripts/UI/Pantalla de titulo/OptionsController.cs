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
            selecciones.SetActive(true);
            gameLogo.SetActive(true);
            selecciones.GetComponentInChildren<Button>().Select();
            gameObject.SetActive(false);
        });
    }
    
    public void SetSound(string param, float soundLevel)
    {
        mixer.SetFloat(param, soundLevel);
    }
}
