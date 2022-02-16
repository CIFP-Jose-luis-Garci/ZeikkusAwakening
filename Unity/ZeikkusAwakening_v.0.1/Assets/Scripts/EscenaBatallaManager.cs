using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscenaBatallaManager : MonoBehaviour
{
    public GameObject[] spawners;

    public GameObject enemyToSpawn;

    private Stats[] enemies;
    private bool alive;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Mathf.Floor(Random.Range(2, spawners.Length)); i++)
        {
            Instantiate(enemyToSpawn, spawners[i].transform);
        }
        enemies = GetComponentsInChildren<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        alive = false;
        foreach (Stats enemy in enemies)
        {
            if (enemy.alive)
            {
                alive = true;
            }
                
        }

        if (!alive)
        {
            Debug.Log("Win battle");
        }
    }
}
