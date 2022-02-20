using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitboxManager : MonoBehaviour
{
    public EnemyBattleManager enemy;
    private Animator animator;
    private AudioSource source;
    private bool isAttacking;

    private void Update()
    {
        animator = enemy.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        isAttacking = animator.GetBool("isAttacking");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                source.PlayOneShot(source.clip);
                PlayerLocomotion playerLocomotion = other.gameObject.GetComponent<PlayerLocomotion>();
                playerLocomotion.RecieveDamage(enemy.GetComponent<Stats>(), animator.GetFloat("damage"), true);
                animator.SetBool("isAttacking", false);
            }
        }
    }
}
