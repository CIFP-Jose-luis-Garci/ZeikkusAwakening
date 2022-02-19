using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZagrantController : MonoBehaviour
{

    private AnimatorManager animatorManager;
    private GameManager gameManager;
    private AudioSource source;
    public bool isAttacking;
    public GameObject damage;

    // Start is called before the first frame update
    void Start()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
        gameManager = FindObjectOfType<GameManager>();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!gameManager.inWorld)
            isAttacking = animatorManager.animator.GetBool("isAttacking");
    }

    private void OnTriggerStay(Collider other)
    {
        if (isAttacking)
        {
            if (other.gameObject.CompareTag("Enemigo"))
            {
                animatorManager.animator.SetBool("isAttacking", false);
                source.PlayOneShot(source.clip);
                Stats enemyStats = other.gameObject.GetComponent<Stats>();
                EnemyBattleManager enemyBattleManager = enemyStats.GetComponent<EnemyBattleManager>();
                enemyBattleManager.recoiled = true;
                Stats zeikkuStats = FindObjectOfType<PlayerLocomotion>().gameObject.GetComponent<Stats>();
                int resultado = GameManager.CalcPhysDamage(zeikkuStats, enemyStats, animatorManager.animator.GetFloat("damage"));
                enemyStats.hp -= resultado;
                DamageNumberManager damageNumber = Instantiate(damage, enemyStats.transform.position, Quaternion.identity, enemyStats.transform).GetComponent<DamageNumberManager>();
                damageNumber.GetComponent<TextMesh>().text = Mathf.FloorToInt(resultado).ToString();
                if (enemyStats.hp < 0)
                {
                    enemyStats.alive = false;
                    isAttacking = false;
                    enemyStats.gameObject.SetActive(false);
                }
            } 
        }
        else
        {
            if (other.gameObject.CompareTag("EnemigoWorld"))
            {
                FindObjectOfType<GameManager>().ToBattle(other.gameObject.GetComponent<EnemyManager>().enemyToSpawn);
                Destroy(other.gameObject);
            }
        }
    }
}
