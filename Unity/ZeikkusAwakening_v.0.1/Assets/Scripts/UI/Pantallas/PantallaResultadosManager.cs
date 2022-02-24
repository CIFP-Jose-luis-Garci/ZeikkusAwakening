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
    public bool animated;
    public Image blackFade;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        escenaBatallaManager = FindObjectOfType<EscenaBatallaManager>();
        enemies = escenaBatallaManager.enemies;
        gameManager = FindObjectOfType<GameManager>();
        exp.text = GameManager.CalcExp(enemies);
        maru.text = GameManager.CalcMaru(enemies);
        animated = false;
        danoTotal.text = escenaBatallaManager.danoTotal.ToString();
        tiempoBatalla.text = escenaBatallaManager.TiempoBatalla();
    }

    public void Animate()
    {
        if (animated) return;
        animated = true;
        gameManager.ToFade(blackFade, escenaBatallaManager);
        gameObject.SetActive(false);
    }
}
