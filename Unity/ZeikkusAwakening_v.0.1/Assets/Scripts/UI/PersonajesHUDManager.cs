using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonajesHUDManager : MonoBehaviour
{
    public Sprite[] spritesPuntosDeTurno;
    public Image valorPuntosDeTurno;

    private Stats usedCharacterTurnPoints;

    private void Start()
    {
        usedCharacterTurnPoints = FindObjectOfType<GameManager>().personajes[0].GetComponent<Stats>();
    }

    private void Update()
    {
        valorPuntosDeTurno.sprite = spritesPuntosDeTurno[usedCharacterTurnPoints.turnPoints];
    }
}
