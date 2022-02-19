using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaResultadosManager : MonoBehaviour
{
    public Text exp, maru, danoTotal, tiempoBatalla;
    // Start is called before the first frame update
    private void OnEnable()
    {
        EscenaBatallaManager escenaBatallaManager = FindObjectOfType<EscenaBatallaManager>();
        exp.text = GameManager.CalcExp(escenaBatallaManager.enemies);
        maru.text = GameManager.CalcMaru(escenaBatallaManager.enemies);
        danoTotal.text = escenaBatallaManager.danoTotal.ToString();
        tiempoBatalla.text = escenaBatallaManager.TiempoBatalla();
    }
}
