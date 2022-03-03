using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ZagrantController : MonoBehaviour
{

    public Animator animator;
    private AudioSource source;
    private Stats stats;
    public bool isAttacking;
    public bool onFire;
    private static readonly int Damage = Animator.StringToHash("damage");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        stats = animator.GetComponent<Stats>();
    }

    private void Update()
    {
        if (stats)
            isAttacking = stats.alive && animator.GetBool(IsAttacking);
        else
            isAttacking = animator.GetBool(IsAttacking);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            GameObject otherObject = other.gameObject;
            if (otherObject.CompareTag("Enemigo"))
            {
                EnemyBattleManager enemyBattleManager = otherObject.GetComponent<EnemyBattleManager>();
                enemyBattleManager.RecieveDamage(stats, animator.GetFloat(Damage), true, onFire);
                source.PlayOneShot(source.clip);
            }
            if (otherObject.CompareTag("EnemigoWorld"))
            {
                if (onFire)
                    FindObjectOfType<SwordFireManager>().Destroy();
                HUDManager.Instance.StartBattle(otherObject, false, 2);
            }

            if (otherObject.CompareTag("Boss"))
            {
                BossBattleManager bossBattleManager = otherObject.GetComponent<BossBattleManager>();
                bossBattleManager.RecieveDamage(stats, animator.GetFloat(Damage), true, onFire);
                source.PlayOneShot(source.clip);
            }

            if (otherObject.CompareTag("Player"))
            {
                PlayerLocomotion playerLocomotion = otherObject.GetComponent<PlayerLocomotion>();
                playerLocomotion.RecieveDamage(stats, animator.GetFloat(Damage), false);
            }
        }
    }
}
