using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private float time;
    public Text tiempo;
    public Image blackBackground;
    public AudioMixer mixer;

    private void Start()
    {
        StartCoroutine(GameManager.CrossFadeMusic(mixer, 2, false));
        blackBackground.CrossFadeAlpha(0,2,true);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (tiempo.gameObject.activeInHierarchy)
            tiempo.text = TiempoActual();
    }
    
    private string TiempoActual()
    {
    
        string horas = Mathf.Floor((time / 60) / 24).ToString("00");
        string minutos = Mathf.Floor(time / 60).ToString("00");
        string segundos = Mathf.Floor(time % 60).ToString("00");
    
        return horas + ":" + minutos + ":" + segundos;
    }
}
