using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyBattleManager : MonoBehaviour
{
    private Transform player;
    public GameObject sprite;
    public bool recoiled;
    public bool battleStarted;
    public Slider lifebar;
    public GameObject damage;
    
    private Animator animator;
    private NavMeshAgent agente;
    private AudioSource source;
    private Stats stats;
    public bool isRunning;
    private bool isAttacking;
    private float waitTime;
    private float hitLength;

    public AudioClip[] stepSounds;
    public AudioClip[] crySounds;
    public AudioClip[] attackSounds;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerManager>().transform;
        agente = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();
        stats = GetComponent<Stats>();
        lifebar.minValue = 0;
        lifebar.maxValue = stats.maxHP;
        lifebar.value = stats.hp;
    }

    private void Update()
    {
        if (GameManager.inPause || !stats.alive || !battleStarted)
        {
            animator.SetBool("alcance", false);
            isRunning = false;
            waitTime = 0;
            isAttacking = false;
            agente.SetDestination(transform.position);
            return;
        }
        if (recoiled)
            Recoil();
        else
            Run();
    }

    private void Run()
    {
        if (isAttacking)
        {
            if (waitTime <= 0)
            {
                float randomAttack = Random.Range(0f, 1f);
                animator.SetFloat("tipoAtaque", randomAttack);
                animator.SetBool("alcance", true);
                ClipLength();
                agente.speed = 0;
            }
            if (waitTime > hitLength)
            {
                animator.SetBool("alcance", false);
                isRunning = false;
                waitTime = 0;
                isAttacking = false;
                return;
            }
            waitTime += Time.deltaTime;
            agente.SetDestination(transform.position);
        }
        else
        {
            float distanceFromPlayer = Vector3.Distance(player.position, transform.position);
            if (!isRunning)
            { 
                waitTime = 0;
                isRunning = true;
                agente.speed = 4;
            }

            if (distanceFromPlayer < 1)
            {
                isAttacking = true;
                return;
            }
            agente.SetDestination(player.position);
        }
    }

    private void Recoil()
    {
        animator.SetTrigger("daño");
        recoiled = false;
    }

    public void RecieveDamage(Stats playerStats, float power, bool isPhysical, bool forceCrit = false)
    {
        recoiled = true;
        int resultado;
        if (isPhysical)
            resultado = GameManager.CalcPhysDamage(playerStats, stats, power, forceCrit);
        else
            resultado = GameManager.CalcSpecDamage(playerStats, stats, power, forceCrit);
        stats.hp -= resultado;
        lifebar.value = stats.hp;
        FindObjectOfType<EscenaBatallaManager>().danoTotal += resultado;
        GameObject instantiated = Instantiate(damage, transform.position, Quaternion.identity, transform);
        instantiated.GetComponent<TextMesh>().text = resultado.ToString();
        if (stats.hp < 0 && stats.alive)
        {
            stats.alive = false;
            isAttacking = false;
            animator.applyRootMotion = true;
            animator.SetTrigger("muerte");
            Destroy(gameObject, 2f);
        }
    }

    private void OnDestroy()
    {
        CheckAlive();
    }

    private void CheckAlive()
    {
        bool alive = false;
        foreach (EnemyBattleManager enemy in FindObjectsOfType<EnemyBattleManager>())
        {
            if (enemy.stats.alive)
            {
                alive = true;
            }
                
        }

        if (!alive)
        {
            player.GetComponent<PlayerLocomotion>().HandleWinBattle();
        }
    }

    public void StepSound()
    {
        source.PlayOneShot(stepSounds[Mathf.FloorToInt(Random.Range(0, stepSounds.Length))]);
    }

    public void CrySound()
    {
        source.PlayOneShot(crySounds[Mathf.FloorToInt(Random.Range(0, crySounds.Length))]);
    }

    public void AttackSound()
    {
        source.PlayOneShot(attackSounds[Mathf.FloorToInt(Random.Range(0, attackSounds.Length))]);
    }

    public void IsAttackingSet(AnimationEvent animationEvent)
    {
        animator.SetBool("isAttacking", animationEvent.intParameter == 1);
    }
    
    private void ClipLength()
    {
        hitLength = animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void ImTarget(bool set)
    {
        sprite.SetActive(set);
    }
}