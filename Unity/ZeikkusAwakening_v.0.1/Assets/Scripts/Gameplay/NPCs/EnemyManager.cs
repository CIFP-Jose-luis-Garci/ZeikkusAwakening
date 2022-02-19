using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [NonSerialized]public Transform player;
    public bool detectado;
    public GameObject enemyToSpawn;
    
    private Animator animator;
    private NavMeshAgent agente;
    private AudioSource source;
    public bool isWalking;
    public bool isRunning;
    private bool isAttacking;
    private float waitTime;
    private float hitLength;
    private float idleLength;

    public AudioClip[] stepSounds;
    public AudioClip[] crySounds;
    public AudioClip[] attackSounds;

    private void Start()
    {
        detectado = false;
        animator = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();
        ClipLength();
    }

    private void Update()
    {
        if (detectado)
        {
            Run();
        }
        else 
        {
            Walk();
        }
    }

    private void Walk()
    {
        if (isWalking && Vector3.Distance(agente.destination, transform.position) < 0.2)
        {
            if (waitTime > idleLength)
            {
                waitTime = 0;
                isWalking = false;
                return;
            }
            if (waitTime <= 0)
            {
                animator.SetBool("moving", false);
                agente.speed = 0;
            }
            waitTime += Time.deltaTime;
        }
        else
        {
            if (!isWalking)
            {
                waitTime = 0;
                animator.SetBool("moving", true);
                isWalking = true;
                agente.speed = 2.5f;
                agente.SetDestination(WalkTo(transform.position, 4));
            }
        }
    }

    private void Run()
    {
        if (isAttacking)
        {
            if (waitTime <= 0)
            {
                animator.SetTrigger("caught");
                agente.speed = 0;
            }
            if (waitTime > hitLength)
            {
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
            if (distanceFromPlayer < 10)
            {
                if (!isRunning)
                {
                    waitTime = 0;
                    animator.SetBool("detection", true);
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
            else
            {
                animator.SetBool("detection", false);
                detectado = false;
                isRunning = false;
            }
        }
    }

    public void StepSound()
    {
        source.PlayOneShot(stepSounds[(Random.Range(0, stepSounds.Length))]);
    }

    public void CrySound()
    {
        source.PlayOneShot(crySounds[Random.Range(0, crySounds.Length)]);
    }

    public void AttackSound(AnimationEvent animationEvent)
    {
        source.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Length)]);
    }
    
    private void ClipLength()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "Attack":
                    hitLength = clip.length;
                    break;
                case "Idle":
                    idleLength = clip.length;
                    break;
            }
        }
    }

    private Vector3 WalkTo(Vector3 from, float randomValue)
    {
        float newX = from.x + Random.Range(-randomValue, randomValue);
        float newZ = from.z + Random.Range(-randomValue, randomValue);
        Vector3 direccion = new Vector3(newX, 0, newZ);
        return direccion;
    }
}