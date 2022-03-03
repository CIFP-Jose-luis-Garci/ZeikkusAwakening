using System;
using UnityEngine;

public class Titulo : MonoBehaviour 
{
    public string titulo;
    public string[] subtitulos;

    private void Start()
    {
        titulo = titulo.Replace(" - ", "\n");
    }
}