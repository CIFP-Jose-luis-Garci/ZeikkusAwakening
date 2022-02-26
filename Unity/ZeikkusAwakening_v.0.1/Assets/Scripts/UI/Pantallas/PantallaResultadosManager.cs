using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaResultadosManager : MonoBehaviour
{
    public Text exp, maru, danoTotal, tiempoBatalla;
    public GameObject subesDeNivel;

    private EscenaBatallaManager escenaBatallaManager;
    private Stats[] enemies;
    public Estadística[][] levelUps;
    public bool animated;
    public Image blackFade;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        escenaBatallaManager = FindObjectOfType<EscenaBatallaManager>();
        enemies = escenaBatallaManager.enemies;
        exp.text = GameManager.CalcExp(enemies, this);
        maru.text = GameManager.CalcMaru(enemies);
        animated = false;
        danoTotal.text = escenaBatallaManager.danoTotal.ToString();
        tiempoBatalla.text = escenaBatallaManager.TiempoBatalla();
    }

    public void End()
    {
        if (animated) return;
        animated = true;
        FindObjectOfType<HUDManager>().ToFadeBattle(blackFade, escenaBatallaManager);
        gameObject.SetActive(false);
    }
}
