using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZagrantController : MonoBehaviour
{

    private AnimatorManager animatorManager;
    private GameManager gameManager;
    private AudioSource source
    public bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (!gameManager.inWorld)
            isAttacking = animatorManager.animator.GetBool("isAttacking");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            if (other.gameObject.CompareTag("Enemigo"))
            {
                Stats enemyStats = other.gameObject.GetComponent<Stats>();
                Stats zeikkuStats = FindObjectOfType<PlayerLocomotion>().gameObject.GetComponent<Stats>();
                float resultado = 0.2f * 2;
                resultado += 1;
                resultado *= zeikkuStats.strength;
                resultado *= animatorManager.animator.GetFloat("damage");
                resultado /= (25 * enemyStats.defense);
                resultado += 2;
                float random = Random.Range(85 ,100);
                resultado *= random;
                resultado *= 0.01f;
                resultado *= 5;
                enemyStats.hp -= (int) resultado;
                if (enemyStats.hp < 0)
                {
                    enemyStats.alive = false;
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
