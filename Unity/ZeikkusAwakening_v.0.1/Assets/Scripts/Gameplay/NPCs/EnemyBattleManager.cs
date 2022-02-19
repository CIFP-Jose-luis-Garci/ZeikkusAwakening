using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class EnemyBattleManager : MonoBehaviour
{
    private Transform player;
    public GameObject sprite;
    public bool recoiled;
    
    private Animator animator;
    private NavMeshAgent agente;
    private AudioSource source;
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
    }

    private void Update()
    {
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

    public void StepSound()
    {
        source.PlayOneShot(stepSounds[Mathf.FloorToInt(Random.Range(0, stepSounds.Length))]);
    }

    public void CrySound()
    {
        source.PlayOneShot(crySounds[Mathf.FloorToInt(Random.Range(0, crySounds.Length))]);
    }

    public void AttackSound(AnimationEvent animationEvent)
    {
        source.PlayOneShot(attackSounds[Mathf.FloorToInt(Random.Range(0, attackSounds.Length))]);
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