using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : Magic
{
    private AnimatorManager animatorManager;
    private EnemyBattleManager enemyFound;
    private Transform enemyLockedOn;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = InputManager.Instance.transform;
        enemyLockedOn = player.GetComponent<PlayerLocomotion>().enemyObject;
        if (enemyLockedOn)
        {
            enemyFound = enemyLockedOn.GetComponent<EnemyBattleManager>();
            transform.position = enemyLockedOn.position;
        }
        else
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = player.position;
            foreach (EnemyBattleManager enemyBattleManager in FindObjectsOfType<EnemyBattleManager>())
            {
                float dist = Vector3.Distance(enemyBattleManager.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = enemyBattleManager.transform;
                    minDist = dist;
                    enemyFound = enemyBattleManager;
                }
            }
            transform.position = tMin.position;
        }


        animatorManager = player.GetComponent<AnimatorManager>();
        animatorManager.PlayTargetAnimation("magic explosion", true, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemigo") || other.gameObject.CompareTag("Boss"))
        {
            EnemyBattleManager enemyBattleManager = other.gameObject.GetComponent<EnemyBattleManager>();
            Stats zeikkuStats = player.GetComponent<Stats>();
            enemyBattleManager.RecieveDamage(zeikkuStats, animatorManager.animator.GetFloat("damage"), false);
            Destroy(gameObject, 2f);
        }
    }
}
