using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider bgmSlider, sfxSlider;
    public Button exit;
    public Toggle invertCameraX, invertCameraY;
    public AudioMixer mixer;
    public GameObject selecciones, gameLogo;
    private InputManager inputManager;

    private void OnEnable()
    {
        inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null)
            inputManager.inPause = true;
        bgmSlider.Select();
        bgmSlider.onValueChanged.AddListener(delegate { SetSound("BGMVolume", bgmSlider.value); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSound("SFXVolume", sfxSlider.value); });
        exit.onClick.AddListener(() =>
        {
            selecciones.SetActive(true);
            gameLogo.SetActive(true);
            selecciones.GetComponentInChildren<Button>().Select();
            gameObject.SetActive(false);
        });
        bgmSlider.value = GameManager.BGMVolume;
        sfxSlider.value = GameManager.SFXVolume;
        // Camera
        GameManager.invertCameraX = invertCameraX.isOn;
        GameManager.invertCameraY = invertCameraY.isOn;
        invertCameraX.onValueChanged.AddListener((value) => GameManager.invertCameraX = value);
        invertCameraY.onValueChanged.AddListener((value) => GameManager.invertCameraY = value);
    }

    private void OnDisable()
    {
        if (inputManager != null)
            inputManager.inPause = false;
        invertCameraX.isOn = GameManager.invertCameraX;
        invertCameraY.isOn = GameManager.invertCameraY;
        mixer.GetFloat("BGMVolume", out GameManager.BGMVolume);
        mixer.GetFloat("SFXVolume", out GameManager.SFXVolume);
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
            cameraManager.ChangeCameraInvert();
    }

    public void SetSound(string param, float soundLevel)
    {
        mixer.SetFloat(param, soundLevel);
    }
}
