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
    [NonSerialized] public GameObject worldEnemy;
    [NonSerialized] public Vector3 worldEnemyPosition;
    [NonSerialized] public Vector3 playerOrigin;
    [NonSerialized] public EnemyStats[] enemyStats;
    private bool battling;
    private Transform playerTransform;

    public int danoTotal;
    public float tiempoDeCombate;
    public int enemyAdvantage;

    private void Update()
    {
        if (GameManager.Instance.inPause || !battling) return;
        tiempoDeCombate += Time.deltaTime;
    }

    public void ControlScene(SlotEnemigoManager[] slotsEnemigos, bool isBoss)
    {
        battling = true;
        playerTransform = InputManager.Instance.transform;
        playerOrigin = playerTransform.position;
        playerTransform.position = playerSpawn.position;
        playerTransform.rotation = Quaternion.Euler(0,180, 0);
        
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            Transform spawner = spawners[i].transform;
            Instantiate(enemyToSpawn, spawner.position, spawner.rotation, spawner);
            if (isBoss) break;
        }
        enemyStats = GetComponentsInChildren<EnemyStats>();
        worldEnemy = enemyStats[0].GetComponent<EnemyBattleManager>().worldEnemy;
        //int teamlevel = GameManager.GetTeamLevel();
        foreach (EnemyStats enemy in enemyStats)
        {
            if (isBoss) break;
            int newLevel = Random.Range(1, 4);
            Debug.Log(newLevel);
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

    public void Purge()
    {
        foreach (GameObject spawner in spawners)
        {
            EnemyBattleManager enemy = spawner.GetComponentInChildren<EnemyBattleManager>();
            if (enemy)
                Destroy(enemy.gameObject);
        }
    }
}
