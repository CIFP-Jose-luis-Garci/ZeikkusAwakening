using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EscenaBatallaManager : MonoBehaviour
{
    public GameObject[] spawners;
    public Transform playerSpawn;
    [NonSerialized] public GameObject enemyToSpawn;
    [NonSerialized] public Vector3 playerOrigin;
    [NonSerialized] public EnemyStats[] enemyStats;
    private bool battling;
    private Transform playerTransform;

    public int danoTotal;
    public float tiempoDeCombate;
    public int enemyAdvantage;

    private void Update()
    {
        if (GameManager.inPause || !battling) return;
        tiempoDeCombate += Time.deltaTime;
    }

    public void ControlScene(SlotEnemigoManager[] slotsEnemigos)
    {
        battling = true;
        playerTransform = FindObjectOfType<PlayerManager>().transform;
        playerOrigin = playerTransform.position;
        playerTransform.position = playerSpawn.position;
        playerTransform.rotation = Quaternion.Euler(0,180, 0);
        
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            Transform spawner = spawners[i].transform;
            Instantiate(enemyToSpawn, spawner.position, spawner.rotation, spawner);
        }
        enemyStats = GetComponentsInChildren<EnemyStats>();

        //int teamlevel = GameManager.GetTeamLevel();
        foreach (EnemyStats enemy in enemyStats)
        {
            int newLevel = Random.Range(1, 4);
            enemy.SetLevel(newLevel);
            
            switch (enemyAdvantage)
            {
                case 0:
                    Stats playerStats = playerTransform.GetComponent<Stats>();
                    playerStats.hp -= (int) (playerStats.hp * 0.05);
                    break;
                case 2:
                    enemy.hp -= (int) (enemy.hp * 0.2);
                    break;
            }

            Debug.Log(newLevel);
        }
        
        for (int i = 0; i < slotsEnemigos.Length; i++)
        {
            SlotEnemigoManager slot = slotsEnemigos[i];
            slot.SetNameAndLevel(i >= enemyStats.Length ? null : enemyStats[i]);
        }
        
        danoTotal = 0;
        tiempoDeCombate = 0;
    }

    public void EnemiesStart()
    {
        foreach (EnemyBattleManager enemy in GetComponentsInChildren<EnemyBattleManager>())
        {
            enemy.battleStarted = true;
        }
    }

    public void ResetPlayer()
    {
        playerTransform.position = playerOrigin;
        battling = false;
    }

    public string TiempoBatalla()
    {
        string minutos = Mathf.Floor(tiempoDeCombate / 60).ToString("00");
        string segundos = Mathf.Floor(tiempoDeCombate % 60).ToString("00");
        string milis = ((tiempoDeCombate - Math.Floor(tiempoDeCombate)) * 1000).ToString("00");
    
        return minutos + ":" + segundos + ";" + milis;
    }
}
