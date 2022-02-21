using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PantallaOpcionesManager : MonoBehaviour
{
    public Slider bgmSlider, sfxSlider;
    public Button exit;
    public Toggle invertCameraX, invertCameraY;
    public AudioMixer mixer;
    private GameObject pantallaPausa;
    
    private void OnEnable()
    {
        bgmSlider.Select();
        bgmSlider.onValueChanged.AddListener(delegate { SetSound("BGMVolume", bgmSlider.value); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSound("SFXVolume", sfxSlider.value); });
        exit.onClick.AddListener(() =>
        {
            
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
