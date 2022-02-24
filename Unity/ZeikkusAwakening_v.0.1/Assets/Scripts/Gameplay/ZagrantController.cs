using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZagrantController : MonoBehaviour
{

    private Animator animator;
    private GameManager gameManager;
    private AudioSource source;
    public bool isAttacking;
    public bool onFire;
    private bool canStartBattle;

    // Start is called before the first frame update
    void Start()
    {
        animator = FindObjectOfType<AnimatorManager>().animator;
        gameManager = FindObjectOfType<GameManager>();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        isAttacking = animator.GetBool("isAttacking");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            if (other.gameObject.CompareTag("Enemigo"))
            {
                EnemyBattleManager enemyBattleManager = other.gameObject.GetComponent<EnemyBattleManager>();
                Stats zeikkuStats = FindObjectOfType<PlayerLocomotion>().gameObject.GetComponent<Stats>();
                enemyBattleManager.RecieveDamage(zeikkuStats, animator.GetFloat("damage"), true, onFire);
                source.PlayOneShot(source.clip);
            }
            if (other.gameObject.CompareTag("EnemigoWorld"))
            {
                gameManager.StartBattle(other.gameObject, false);
            }
            if (other.gameObject.CompareTag("Boss"))
            {
                gameManager.StartBattle(other.gameObject, true);
            }
        }
    }
}
