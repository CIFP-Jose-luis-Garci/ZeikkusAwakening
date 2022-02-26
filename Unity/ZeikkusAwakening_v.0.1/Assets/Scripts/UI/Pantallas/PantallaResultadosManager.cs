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
    private SubesDeNivelManager sdnm;
    private Stats[] enemies;
    public Estadistica[][] levelUps;
    private int levelUpCount = 0;
    public Image blackFade;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        levelUps = new Estadistica[3][];
        escenaBatallaManager = FindObjectOfType<EscenaBatallaManager>();
        enemies = escenaBatallaManager.enemies;
        exp.text = GameManager.CalcExp(enemies, this);
        maru.text = GameManager.CalcMaru(enemies);
        danoTotal.text = escenaBatallaManager.danoTotal.ToString();
        tiempoBatalla.text = escenaBatallaManager.TiempoBatalla();
        LevelUpEvent();
    }
    
    private bool LevelUpEvent(){
        if (levelUps[levelUpCount] != null)
        {
            sdnm = subesDeNivelContainer.GetComponentInChildren<SubesDeNivelManager>();
            if (sdnm)
                sdnm.Retract();
            sdnm = Instantiate(subesDeNivelPrefab, subesDeNivelContainer.transform).GetComponent<SubesDeNivelManager>();
            sdnm.SetEstadísticas(levelUps[levelUpCount]);
            levelUpCount++;
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
        Destroy(sdnm.gameObject);
        FindObjectOfType<HUDManager>().ToFadeBattle(blackFade, escenaBatallaManager);
        gameObject.SetActive(false);
    }
}
