using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitboxManager : MonoBehaviour
{
    public EnemyBattleManager enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy.DoDamage(other.gameObject.GetComponent<Stats>());
        }
    }
}
