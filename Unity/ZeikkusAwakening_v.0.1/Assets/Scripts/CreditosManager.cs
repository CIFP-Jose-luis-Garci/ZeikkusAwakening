using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditosManager : MonoBehaviour
{
    public Titulo[] creditos;
    public GameObject container, titulo, subtitulo, datosJugador;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MostrarCreditos());
        Titulo datosJugadorCredito = datosJugador.GetComponent<Titulo>();
        datosJugadorCredito.subtitulos[0] += TiempoActual();
        datosJugadorCredito.subtitulos[1] += GameManager.Instance.totalDamage  + " puntos de vida";
        datosJugadorCredito.subtitulos[2] += GameManager.Instance.bossDefeated ? "SI" : "NO";
        if (GameManager.Instance.bossDefeated)
            datosJugadorCredito.subtitulos[3] += "¡Enhorabuena! Sí, se puede saltar el jefe.";
            
    }
    
    private string TiempoActual()
    {
        float time = GameManager.Instance.playtime;
        int hours = Mathf.FloorToInt(time / 60 / 60);
        int minutes = Mathf.FloorToInt(time / 60) % 60;
        int seconds = Mathf.FloorToInt(time % 60);
        string horas = hours.ToString("00");
        string minutos = minutes.ToString("00");
        string segundos = seconds.ToString("00");
    
        return horas + ":" + minutos + ":" + segundos;
    }

    private IEnumerator MostrarCreditos()
    {
        for (int i = 0; i < creditos.Length; i++)
        {
            Titulo credito = Instantiate(creditos[i].gameObject).GetComponent<Titulo>();
            GameObject tituloInstance = Instantiate(titulo, container.transform);
            tituloInstance.GetComponent<Text>().text = credito.titulo;
            Destroy(tituloInstance, 20f);
            yield return new WaitForSeconds(2);
            for (int j = 0; j < credito.subtitulos.Length; j++)
            {
                GameObject subtituloInstance = Instantiate(subtitulo, container.transform);
                subtituloInstance.GetComponent<Text>().text = credito.subtitulos[j];
                Destroy(subtituloInstance, 20f);
                yield return new WaitForSeconds(1f);
            }
            Destroy(credito.gameObject);
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(17);
        Application.Quit(0);
    }
}
