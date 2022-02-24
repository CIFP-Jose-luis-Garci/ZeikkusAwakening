using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EscenaBatallaManager : MonoBehaviour
{
    public GameObject[] spawners;

    public GameObject playerSpawn;
    [NonSerialized] public GameObject enemyToSpawn;
    [NonSerialized] public Vector3 playerOrigin;

    [NonSerialized] public Stats[] enemies;
    private bool alive;
    private Transform playerTransform;

    public int danoTotal;
    public float tiempoDeCombate;
    public int enemyAdvantage;

    // Start is called before the first frame update
    void OnEnable()
    {
        playerTransform = FindObjectOfType<PlayerManager>().transform;
        playerOrigin = playerTransform.position;
        playerTransform.position = playerSpawn.transform.position;
        int maxSpawns;
        if (enemyToSpawn.name.StartsWith("Mino"))
            maxSpawns = 4;
        else
            maxSpawns = 5;
        
        for (int i = 0; i < Random.Range(2, maxSpawns); i++)
        {
            Transform spawner = spawners[i].transform;
            Instantiate(enemyToSpawn, spawner.position, spawner.rotation, spawner);
        }
        enemies = GetComponentsInChildren<Stats>();

        int teamlevel = GameManager.GetTeamLevel();
        foreach (Stats enemy in enemies)
        {
            int newLevel = Random.Range(teamlevel - 3, teamlevel + 2);
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
        danoTotal = 0;
        tiempoDeCombate = 0;
    }

    private void Update()
    {
        if (GameManager.inPause) return;
        tiempoDeCombate += Time.deltaTime;
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
    }

    public string TiempoBatalla()
    {
        string minutos = Mathf.Floor(tiempoDeCombate / 60).ToString("00");
        string segundos = Mathf.Floor(tiempoDeCombate % 60).ToString("00");
        string milis = ((tiempoDeCombate - Math.Floor(tiempoDeCombate)) * 1000).ToString("00");
    
        return minutos + ":" + segundos + ";" + milis;
    }
}
