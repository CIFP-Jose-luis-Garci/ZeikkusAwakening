using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InBetweenObjectManager : MonoBehaviour
{
    CinemachineFreeLook cmfl;
    Transform player;
    public Transform enemy;
    public EnemyManager enemyManager;
    public Stats enemyStats;
    public bool enemyFound;
    public float distance;
    private InputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        player = inputManager.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyFound)
        {
            distance = Vector3.Distance(player.position, enemy.position);
            if (distance < 10)
                transform.position = enemy.position + (player.position - enemy.position) / 2;
            else
                transform.position = player.position;
        }
    }

    public void FindEnemy()
    {
        if (inputManager.lTrigger && !enemyFound)
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            enemyFound = false;
            foreach (GameObject t in GameObject.FindGameObjectsWithTag("Enemigo"))
            {
                enemyFound = true;
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t.transform;
                    minDist = dist;
                }
            }
            enemy = tMin;
            if (enemyFound)
            {
                enemyManager = enemy.GetComponent<EnemyManager>();
                enemyStats = enemy.GetComponent<Stats>();
            }
        }

        if (enemyFound && !enemyStats.alive)
        {
            enemyFound = false;
        }
    }

}
