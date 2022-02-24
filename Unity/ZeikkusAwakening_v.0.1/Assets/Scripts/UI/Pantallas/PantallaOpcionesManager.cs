using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PantallaOpcionesManager : MonoBehaviour
{
    public Slider bgmSlider, sfxSlider, voiceSlider, cameraSensitivityXSlider, cameraSensitivityYSlider;
    public Button exit;
    public Toggle invertCameraX, invertCameraY;
    public AudioMixer mixer;
    public GameObject pantallaPausa;
    private InputManager inputManager;
    
    private void OnEnable()
    {
        inputManager = FindObjectOfType<InputManager>();
        bgmSlider.Select();
        
        bgmSlider.value = GameManager.BGMVolume;
        sfxSlider.value = GameManager.SFXVolume;
        voiceSlider.value = GameManager.voiceVolume;
        bgmSlider.onValueChanged.AddListener((value) => SetSound("BGMVolume", value));
        sfxSlider.onValueChanged.AddListener( (value) => SetSound("SFXVolume", value));
        voiceSlider.onValueChanged.AddListener( (value) => SetSound("VoiceVolume", value));
        exit.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            pantallaPausa.SetActive(true);
            pantallaPausa.GetComponentInChildren<Button>().Select();
        });
        
        // Camera
        invertCameraX.isOn = GameManager.invertCameraX;
        invertCameraY.isOn = GameManager.invertCameraY;
        invertCameraX.onValueChanged.AddListener((value) => GameManager.invertCameraX = value);
        invertCameraY.onValueChanged.AddListener((value) => GameManager.invertCameraY = value);
        cameraSensitivityXSlider.value = GameManager.cameraSensitivityX;
        cameraSensitivityYSlider.value = GameManager.cameraSensitivityY;
        cameraSensitivityXSlider.onValueChanged.AddListener((value) => GameManager.cameraSensitivityX = (int) value);
        cameraSensitivityYSlider.onValueChanged.AddListener((value) => GameManager.cameraSensitivityY = (int) value);
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

    public void SetSound(string param, float soundLevel)
    {
        mixer.SetFloat(param, soundLevel);
    }
}
