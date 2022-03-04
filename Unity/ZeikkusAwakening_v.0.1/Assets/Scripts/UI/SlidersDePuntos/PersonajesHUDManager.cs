using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonajesHUDManager : MonoBehaviour
{
    public Sprite[] spritesPuntosDeTurno;
    public Image valorPuntosDeTurno;

    private Stats stats;

    private void Start()
    {
        stats = InputManager.Instance.GetComponent<Stats>();
    }

    private void Update()
    {
        valorPuntosDeTurno.sprite = spritesPuntosDeTurno[stats.turnPoints];
    }
}
