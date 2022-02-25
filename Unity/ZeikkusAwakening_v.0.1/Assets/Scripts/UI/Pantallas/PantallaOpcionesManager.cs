using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PantallaOpcionesManager : MonoBehaviour
{
    public Slider bgmSlider, sfxSlider, voiceSlider, cameraSensitivityXSlider, cameraSensitivityYSlider;
    public Toggle invertCameraX, invertCameraY;
    public Button exit;
    public AudioMixer mixer;
    public GameObject pantallaPausa;
    private InputManager inputManager;
    
    private void OnEnable()
    {
        inputManager = FindObjectOfType<InputManager>();
        bgmSlider.Select();
        
        // BGM, Sound and Voices
        bgmSlider.value = GameManager.BGMVolume;
        sfxSlider.value = GameManager.SFXVolume;
        voiceSlider.value = GameManager.voiceVolume;
        bgmSlider.onValueChanged.AddListener((value) => SetSound(bgmSlider, "BGMVolume", value));
        sfxSlider.onValueChanged.AddListener( (value) => SetSound(sfxSlider, "SFXVolume", value));
        voiceSlider.onValueChanged.AddListener( (value) => SetSound(voiceSlider, "VoiceVolume", value));
        
        // Camera
        invertCameraX.isOn = GameManager.invertCameraX;
        invertCameraY.isOn = GameManager.invertCameraY;
        cameraSensitivityXSlider.value = GameManager.cameraSensitivityX;
        cameraSensitivityYSlider.value = GameManager.cameraSensitivityY;
        invertCameraX.onValueChanged.AddListener((value) => GameManager.invertCameraX = value);
        invertCameraY.onValueChanged.AddListener((value) => GameManager.invertCameraY = value);
        cameraSensitivityXSlider.onValueChanged.AddListener((value) => GameManager.cameraSensitivityX = (int) value);
        cameraSensitivityYSlider.onValueChanged.AddListener((value) => GameManager.cameraSensitivityY = (int) value);
        
        exit.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            pantallaPausa.SetActive(true);
            pantallaPausa.GetComponentInChildren<Button>().Select();
        });
    }

    private void Update()
    {
        inputManager.GoBack(gameObject, pantallaPausa);
    }

    private void OnDisable()
    {
        GameManager.BGMVolume = bgmSlider.value;
        GameManager.SFXVolume = sfxSlider.value;
        GameManager.voiceVolume = voiceSlider.value;
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        cameraManager.ChangeCameraInvert();
        cameraManager.ChangeCameraSensitivity();
    }

    private void SetSound(Slider slider, string param, float soundLevel)
    {
        if (soundLevel <= slider.minValue)
            mixer.SetFloat(param, -80f);
        else
            mixer.SetFloat(param, soundLevel);
    }
}
