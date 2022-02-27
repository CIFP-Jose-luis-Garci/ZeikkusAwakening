using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotEnemigoManager : MonoBehaviour
{
    [SerializeField] private Text nombre, nivel; 
    public Animator animator;

    public void Retract()
    {
        animator.enabled = true;
    }

    public void SetNameAndLevel(EnemyStats enemyStats)
    {
        if (!enemyStats)
        {
            gameObject.SetActive(false);
            return;
        }

        enemyStats.slotEnemigo = this;
        nombre.text = enemyStats.actorName;
        nivel.text = "Nv. " + enemyStats.level;
    }
}
