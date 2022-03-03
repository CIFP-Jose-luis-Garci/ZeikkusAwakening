using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider bgmSlider, sfxSlider, voiceSlider, cameraSensitivityXSlider, cameraSensitivityYSlider;
    public Toggle invertCameraX, invertCameraY;
    public Button exit;
    public AudioMixer mixer;
    public GameObject selecciones, gameLogo;
    private GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        bgmSlider.Select();
        
        // BGM, Sound and Voices
        bgmSlider.value = gameManager.BGMVolume;
        sfxSlider.value = gameManager.SFXVolume;
        voiceSlider.value = gameManager.voiceVolume;
        bgmSlider.onValueChanged.AddListener((value) => SetSound(bgmSlider, "BGMVolume", value));
        sfxSlider.onValueChanged.AddListener( (value) => SetSound(sfxSlider, "SFXVolume", value));
        voiceSlider.onValueChanged.AddListener( (value) => SetSound(voiceSlider, "VoiceVolume", value));
        
        // Camera
        invertCameraX.isOn = gameManager.invertCameraX;
        invertCameraY.isOn = gameManager.invertCameraY;
        cameraSensitivityXSlider.value = gameManager.cameraSensitivityX;
        cameraSensitivityYSlider.value = gameManager.cameraSensitivityY;
        invertCameraX.onValueChanged.AddListener((value) => gameManager.invertCameraX = value);
        invertCameraY.onValueChanged.AddListener((value) => gameManager.invertCameraY = value);
        cameraSensitivityXSlider.onValueChanged.AddListener((value) => gameManager.cameraSensitivityX = (int) value);
        cameraSensitivityYSlider.onValueChanged.AddListener((value) => gameManager.cameraSensitivityY = (int) value);
        
        exit.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnDisable()
    {
        gameManager.BGMVolume = bgmSlider.value;
        gameManager.SFXVolume = sfxSlider.value;
        gameManager.voiceVolume = voiceSlider.value;
        GetComponentInParent<TitleScreenManager>().zeikkuInstatiated.SetActive(true);
        selecciones.SetActive(true);
        gameLogo.SetActive(true);
        selecciones.GetComponentInChildren<Button>().Select();
    }

    private void SetSound(Slider slider, string param, float soundLevel)
    {
        if (soundLevel <= slider.minValue)
            mixer.SetFloat(param, -80f);
        else
            mixer.SetFloat(param, soundLevel);
    }
}
