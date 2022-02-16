using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscenaBatallaManager : MonoBehaviour
{
    public GameObject[] spawners;

    public GameObject enemyToSpawn;

    private EnemyManager[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Mathf.Floor(Random.Range(0, spawners.Length)); i++)
        {
            Instantiate(enemyToSpawn, spawners[i].transform);
        }
        enemies = FindObjectsOfType<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (EnemyManager enemy in enemies)
        {
            if
        }
    }
}
