using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZagrantController : MonoBehaviour
{

    private Animator animator;
    private GameManager gameManager;
    private AudioSource source;
    public bool isAttacking;
    public bool onFire;

    // Start is called before the first frame update
    void Start()
    {
        animator = FindObjectOfType<AnimatorManager>().animator;
        gameManager = FindObjectOfType<GameManager>();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!gameManager.inWorld)
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
            else
            {
                isAttacking = false;
            }
        }
        else
        {
            if (other.gameObject.CompareTag("EnemigoWorld"))
            {
                Destroy(other.gameObject);
                gameManager.ToBattle(other.gameObject.GetComponent<EnemyManager>().enemyToSpawn, false);
            }
            if (other.gameObject.CompareTag("Boss"))
            {
                Destroy(other.gameObject);
                gameManager.ToBattle(other.gameObject.GetComponent<EnemyManager>().enemyToSpawn, true);
            }
        }
    }
}
