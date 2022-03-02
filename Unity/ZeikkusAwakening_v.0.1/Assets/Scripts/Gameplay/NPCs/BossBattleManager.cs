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
    
    [Header("Ataques")]
    private int currentAtack = 0;
    private bool cooldown;
    private bool completelyRandom;
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

            if (cooldown)
            {
                if (waitTime > 0.15f)
                    cooldown = false;
                waitTime += Time.deltaTime; 
                return;
            }
            if (isNearPlayer)
            {
                LookAtPlayer();
                DecideAction();
            }
            else
            {
                currentAtack = 0;
                agente.speed = 5;
                agente.SetDestination(player.position);
                print("holas");
            }
            SetNearPlayer();

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
        if (completelyRandom)
        {
            chance = Random.Range(0, chances.Length);
            DoAction(chances[(int) chance].name);
        }
        chance = Random.Range(0f, 1f);
        for (int i = 0; i < chances.Length; i++)
        {
            Chance currentChance = chances[0];
            if (chance < currentChance.chance)
            {
                DoAction(currentChance.name);
                return;
            }
        }

    }

    private void DoAction(string name)
    {
        switch (name)
        {
            case "Atacar":
                // parar enemigo
                agente.speed = 0;
                agente.SetDestination(transform.position);
                // hacer animacion de ataque
                animatorManager.PlayTargetAnimation(combo[currentAtack], true);
                currentAtack++;
                if (currentAtack >= combo.Length)
                    currentAtack = 0;
                // sigo con combo?
                break;
            case "Evadir":
                // parar enemigo
                // hacer animacion de evasion
                // marcar invencibilidad
                break;
            case "Bloquear":
                // parar enemigo
                // hacer animacion de bloqueo
                // marcar reduccion de dano
                break;
            case "Usar magia":
                // parar enemigo
                // que magia hago?
                // hacer animacion de magia
                break;
        }
    }

    private void CheckStatus()
    {
        if (stats.hp < stats.maxHP * 0.2f)
            SetChances(0.15f, 0.3f, 0.3f, 0.25f);
        else
            SetChances(0f,0f,0f,0f, true);
        
        
        
        if (stats.hp < stats.maxHP / 2)
        {
            // activar flama
        }

    }

    private void SetNearPlayer()
    {
        isNearPlayer = Vector3.Distance(player.position, transform.position) < 1f;
    }

    private void SetChances(float attackChance, float evadeChance, float blockChance, float useMagicChance, bool completelyRandom = false)
    {
        if (completelyRandom)
            this.completelyRandom = completelyRandom;
        chances[0].chance = attackChance;
        chances[1].chance = evadeChance;
        chances[2].chance = blockChance;
        chances[3].chance = useMagicChance;
        Array.Sort(chances);
    }
}
