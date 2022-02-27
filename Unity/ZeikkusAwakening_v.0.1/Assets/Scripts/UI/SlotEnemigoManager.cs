using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotEnemigoManager : MonoBehaviour
{
    [SerializeField] private Text nombre, nivel;

    public void SetNameAndLevel(string nombre, string nivel)
    {
        this.nombre.text = nombre;
        this.nivel.text = nivel;
    }
}
