using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaResultadosManager : MonoBehaviour
{
    public Text exp, maru, danoTotal, tiempoBatalla;

    private EscenaBatallaManager escenaBatallaManager;
    private Stats[] enemies;
    private GameManager gameManager;
    public bool fade;
    public Image blackFade;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        escenaBatallaManager = FindObjectOfType<EscenaBatallaManager>();
        enemies = escenaBatallaManager.enemies;
        gameManager = FindObjectOfType<GameManager>();
        exp.text = GameManager.CalcExp(enemies);
        maru.text = GameManager.CalcMaru(enemies);
        danoTotal.text = escenaBatallaManager.danoTotal.ToString();
        tiempoBatalla.text = escenaBatallaManager.TiempoBatalla();
    }

    private void Update()
    {
        if (!fade) return;
        if (fade)
        {
            
            GameManager.win = false;
            fade = false;
            gameManager.ToFade(blackFade, escenaBatallaManager);
            gameObject.SetActive(false);
        }
    }
}
