using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private float time;
    public Text tiempo;

  
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
