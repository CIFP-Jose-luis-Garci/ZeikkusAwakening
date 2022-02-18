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

    private Stats[] enemies;
    private bool alive;
    private Transform playerTransform;
    // Start is called before the first frame update
    void OnEnable()
    {
        playerTransform = FindObjectOfType<PlayerManager>().transform;
        playerOrigin = playerTransform.position;
        playerTransform.position = playerSpawn.transform.position;
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
            // win battle anim
            // result screen
            // press a, goto transition fade in black
            playerTransform.position = playerOrigin;
            gameObject.SetActive(false);
            FindObjectOfType<GameManager>().ToWin();
            // fade out black
        }
    }
}
