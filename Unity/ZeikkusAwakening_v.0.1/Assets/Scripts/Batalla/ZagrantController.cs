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
        isAttacking = stats.alive && animator.GetBool(IsAttacking);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            GameObject otherObject = other.gameObject;
            if (otherObject.CompareTag("Enemigo"))
            {
                EnemyBattleManager enemyBattleManager = otherObject.GetComponent<EnemyBattleManager>();
                Stats zeikkuStats = FindObjectOfType<PlayerManager>().GetComponent<Stats>();
                enemyBattleManager.RecieveDamage(zeikkuStats, animator.GetFloat(Damage), true, onFire);
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
                Stats zeikkuStats = FindObjectOfType<PlayerManager>().GetComponent<Stats>();
                bossBattleManager.RecieveDamage(zeikkuStats, animator.GetFloat(Damage), true, onFire);
                source.PlayOneShot(source.clip);
            }

            if (otherObject.CompareTag("Player"))
            {
                BossBattleManager bossBattleManager = FindObjectOfType<BossBattleManager>();
                PlayerLocomotion playerLocomotion = otherObject.GetComponent<PlayerLocomotion>();
                playerLocomotion.RecieveDamage(bossBattleManager.GetComponent<Stats>(), animator.GetFloat(Damage), false);
            }
        }
    }
}
