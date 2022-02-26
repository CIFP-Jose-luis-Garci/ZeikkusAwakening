using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaResultadosManager : MonoBehaviour
{
    public Text exp, maru, danoTotal, tiempoBatalla, opcion;
    public GameObject subesDeNivelContainer, subesDeNivelPrefab;

    private EscenaBatallaManager escenaBatallaManager;
    private Stats[] enemies;
    public Estadística[][] levelUps;
    private int levelUpCount = 0;
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
        LevelUpEvent();
    }
    
    private bool LevelUpEvent(){
        if (levelUps[levelUpCount] != null)
        {
            levelUpCount++;
            SubesDeNivelManager sdnm = subesDeNivelContainer.GetComponentInChildren<SubesDeNivelManager>();
            if (sdnm)
                sdnm.Retract();
            Instantiate(subesDeNivelPrefab, subesDeNivelContainer.transform);
            if (levelUps[levelUpCount] == null)
            {
                opcion.text = "Fin";
                return false;
            }
            opcion.text = "Siguiente";
            return true;
        }
        return false;
        
    }

    public void End()
    {
        if (LevelUpEvent()) return;
        FindObjectOfType<HUDManager>().ToFadeBattle(blackFade, escenaBatallaManager);
        gameObject.SetActive(false);
    }
}
