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
    private EnemyStats[] enemies;
    public Estadistica[][] levelUps;
    private int levelUpCount = 0;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        levelUps = new Estadistica[4][]; // uno de mas para asegurar
        levelUpCount = 0;
        escenaBatallaManager = FindObjectOfType<EscenaBatallaManager>();
        enemies = escenaBatallaManager.enemyStats;
        exp.text = GameManager.Instance.CalcExp(enemies, this);
        maru.text = GameManager.Instance.CalcMaru(enemies);
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
                return true;
            }
            opcion.text = "Siguiente";
            return true;
        }
        return false;
        
    }

    public void End()
    {
        if (!gameObject.activeSelf) return;
        if (LevelUpEvent()) return;
        if (sdnm)
            Destroy(sdnm.gameObject);
        HUDManager.Instance.ToFadeBattle(escenaBatallaManager);
        gameObject.SetActive(false);
    }
}
