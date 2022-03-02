using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ZagrantController : MonoBehaviour
{

    public Animator animator;
    private AudioSource source;
    public bool isAttacking;
    public bool onFire;
    private static readonly int Damage = Animator.StringToHash("damage");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
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
                Stats zeikkuStats = FindObjectOfType<PlayerManager>().GetComponent<Stats>();
                enemyBattleManager.RecieveDamage(zeikkuStats, animator.GetFloat(Damage), true, onFire);
                source.PlayOneShot(source.clip);
            }
            if (otherObject.CompareTag("EnemigoWorld"))
            {
                FindObjectOfType<HUDManager>().StartBattle(otherObject, false, 2);
            }
            if (otherObject.CompareTag("Boss"))
            {
                FindObjectOfType<HUDManager>().StartBattle(otherObject, true, 2);
            }
        }
    }
}
