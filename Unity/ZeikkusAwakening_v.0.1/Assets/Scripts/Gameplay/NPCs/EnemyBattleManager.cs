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
        Run();
    }

    private void Run()
    {
        if (isAttacking)
        {
            if (waitTime <= 0)
            {
                animator.SetBool("alcance", true);
                animator.SetFloat("tipoAtaque", Random.Range(0, 1));
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
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "Attack1":
                    hitLength = clip.length;
                    break;
                case "Attack2":
                    hitLength = clip.length;
                    break;
            }
        }
    }

    public void ImTarget(bool set)
    {
        sprite.SetActive(set);
    }
}