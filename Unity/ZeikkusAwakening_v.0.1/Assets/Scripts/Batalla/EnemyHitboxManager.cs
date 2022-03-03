using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitboxManager : MonoBehaviour
{
    public EnemyBattleManager enemy;
    public EnemyManager enemyWorld;
    private Animator animator;
    private AudioSource source;
    private bool isAttacking;

    private void Start()
    {
        if (enemyWorld)
        {
            animator = enemyWorld.GetComponent<Animator>();
        }

        if (enemy)
        {
            animator = enemy.GetComponent<Animator>();
            source = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        isAttacking = animator.GetBool("isAttacking");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            if (enemy && other.gameObject.CompareTag("Player"))
            {
                source.PlayOneShot(source.clip);
                PlayerLocomotion playerLocomotion = other.gameObject.GetComponent<PlayerLocomotion>();
                playerLocomotion.RecieveDamage(enemy.GetComponent<Stats>(), animator.GetFloat("damage"), true);
                animator.SetBool("isAttacking", false);
            }

            if (enemyWorld && other.gameObject.CompareTag("Player"))
            {
                HUDManager.Instance.StartBattle(enemyWorld.gameObject, false, 0);
            }
        }
    }
}
