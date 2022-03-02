using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BossBattleManager : MonoBehaviour
{
    [NonSerialized] public Transform player;
    private bool isNearPlayer, isAttacking, isEvading, isBlocking, isUsingMagic, isInteracting;
    private Chance[] chances;
    private string[] combo;
    private AnimatorManager animatorManager;
    private NavMeshAgent agente;
    private EnemyStats stats;
    
    [Header("Attacks")]
    private int currentAtack = 0;
    private bool cooldown;
    private float waitTime;
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animatorManager = GetComponent<AnimatorManager>();
        stats = GetComponent<EnemyStats>();
        agente = GetComponent<NavMeshAgent>();
        chances = new Chance[4];
        chances[0] = new Chance("Atacar");
        chances[1] = new Chance("Evadir");
        chances[2] = new Chance("Bloquear");
        chances[3] = new Chance("Usar magia");
        combo = new [] {"basic_slash", "second_slash", "hard_slash", "final_slash" };
    }

    private void Update()
    {
        if (stats.alive)
        {
            if (isInteracting)
            {
                cooldown = true;
                waitTime = 0;
                return;
            }
            SetNearPlayer();

            if (cooldown)
            {
                if (waitTime > 0.15f)
                {
                    cooldown = false;
                    if (!isNearPlayer) isAttacking = false;
                    isEvading = false;
                    isBlocking = false;
                }
                waitTime += Time.deltaTime; 
                return;
            }
            if (isNearPlayer)
            {
                DecideAction();
            }
            else
            {
                currentAtack = 0;
                agente.speed = 5;
                agente.SetDestination(player.position);
            }

        }
    }

    private void LookAtPlayer()
    {
        transform.LookAt(player.position);
        Quaternion fix = transform.rotation;
        fix.x = 0;
        fix.z = 0;
        transform.rotation = fix;

    }

    private void LateUpdate()
    {
        isInteracting = animatorManager.animator.GetBool(IsInteracting);
    }

    private void DecideAction()
    {
        CheckStatus();
        SelectAction();
    }

    private void SelectAction()
    {
        float chance;
        if (WereYouAttacking()) return;
        chance = Random.Range(0f, 1f);
        for (int i = 0; i < chances.Length; i++)
        {
            Chance currentChance = chances[i];
            if (chance < currentChance.chance)
            {
                Debug.Log(chance + " es menor que " + currentChance.chance + " de " + currentChance.name);
                DoAction(currentChance.name);
                return;
            }
        }

    }

    private bool WereYouAttacking()
    {
        if (isAttacking)
        {
            DoAction("Atacar");
        }
        
        return isAttacking;
    }

    private void DoAction(string name)
    {
        switch (name)
        {
            case "Atacar":
                LookAtPlayer();
                StopAIandAnimate(combo[currentAtack]);
                currentAtack++;
                if (currentAtack >= combo.Length)
                    currentAtack = 0;
                isAttacking = true;
                break;
            case "Evadir":
                StopAIandAnimate("evade", true);
                isEvading = true;
                break;
            case "Bloquear":
                LookAtPlayer();
                StopAIandAnimate("block", true);
                isBlocking = true;
                break;
            case "Usar magia":
                LookAtPlayer();
                StopAIandAnimate("magic fireball");
                // parar enemigo
                // que magia hago?
                // hacer animacion de magia
                break;
        }
    }

    private void StopAIandAnimate(string targetAnimation, bool useRootMotion = false)
    {
        agente.speed = 0;
        agente.SetDestination(transform.position);
        animatorManager.PlayTargetAnimation(targetAnimation, true, useRootMotion);
    }

    private void CheckStatus()
    {
        if (stats.hp < stats.maxHP * 0.2f)
            SetChances(0.15f, 0.3f, 0.3f, 0.25f);
        else if (stats.hp < stats.maxHP * 0.5f)
            SetChances(0.5f, 0.15f, 0.2f, 0.15f);
        else if (stats.hp < stats.maxHP * 0.8f)
            SetChances(0.6f, 0.15f, 0f, 0.25f);
        else 
            SetChances(0.7f, 0f, 0f, 0.3f);

    }

    private void SetNearPlayer()
    {
        isNearPlayer = Vector3.Distance(player.position, transform.position) < 1f;
    }

    private void SetChances(float attackChance, float evadeChance, float blockChance, float useMagicChance)
    {
        for (int i = 0; i < chances.Length; i++)
        {
            switch (chances[i].name)
            {
                case "Atacar":
                    chances[i].chance = attackChance;
                    break;
                case "Evadir":
                    chances[i].chance = evadeChance;
                    break;
                case "Bloquear":
                    chances[i].chance = blockChance;
                    break;
                case "Usar magia":
                    chances[i].chance = useMagicChance;
                    break;
            }
        }
        Array.Sort(chances);

    }
}
