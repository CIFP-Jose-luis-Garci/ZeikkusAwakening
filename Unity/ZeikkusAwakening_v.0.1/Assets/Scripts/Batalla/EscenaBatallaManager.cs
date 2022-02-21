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

    // Start is called before the first frame update
    void OnEnable()
    {
        playerTransform = FindObjectOfType<PlayerManager>().transform;
        playerOrigin = playerTransform.position;
        playerTransform.position = playerSpawn.transform.position;
        
        for (int i = 0; i < Random.Range(2, spawners.Length); i++)
        {
            Transform spawner = spawners[i].transform;
            Instantiate(enemyToSpawn, spawner.position, spawner.rotation, spawner);
        }
        enemies = GetComponentsInChildren<Stats>();
        danoTotal = 0;
        tiempoDeCombate = 0;
    }

    private void Update()
    {
        if (GameManager.inPause) return;
        tiempoDeCombate += Time.deltaTime;
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
