using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InBetweenObjectManager : MonoBehaviour
{
    CinemachineFreeLook cmfl;
    Transform player;
    public Transform enemy;
    public EnemyBattleManager enemyManager;
    public Stats enemyStats;
    public bool enemyFound;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (enemyFound && enemy)
        {
            distance = Vector3.Distance(player.position, enemy.position);
            if (distance < 10)
                transform.position = enemy.position + (player.position - enemy.position) / 2;
            else
                transform.position = player.position;
        }
    }

    public void FindEnemy(bool isZTargeting)
    {
        if (isZTargeting && !enemyFound)
        {
            Transform tMin = null;
            float minDist = 10;
            Vector3 currentPos = transform.position;
            enemyFound = false;
            foreach (EnemyBattleManager t in FindObjectsOfType<EnemyBattleManager>())
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    enemyFound = true;
                    tMin = t.transform;
                    minDist = dist;
                }
            }
            enemy = tMin;
            if (enemyFound)
            {
                enemyManager = enemy.GetComponent<EnemyBattleManager>();
                enemyStats = enemy.GetComponent<Stats>();
                enemyManager.ImTarget(true);
            }
        } else if (!isZTargeting && enemyFound)
        {
            enemyManager.ImTarget(false);
            enemyFound = false;
        }

        if (enemyFound && !enemyStats.alive)
        {
            enemyFound = false;
        }
    }

}
