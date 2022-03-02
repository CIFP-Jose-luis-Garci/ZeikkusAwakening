using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BossBattleManager : MonoBehaviour
{
    [NonSerialized] public Transform player;
    private bool isNearPlayer, isAttacking, isEvading, isBlocking, isUsingMagic;
    private Chance[] chances;
    private string[] combo;
    private AnimatorManager animatorManager;
    private NavMeshAgent agente;
    private EnemyStats stats;
    private int currentAtack = 0;

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
        combo = new string[] {"basic_slash", "second_slash", "hard_slash", "final_slash" };
    }

    private void Update()
    {
        if (stats.alive)
        {
            if (isNearPlayer)
            {
                DecideAction();
            }
            else
            {
                currentAtack = 0;
                agente.SetDestination(player.position);
                print("holas");
                SetNearPlayer();
            }

        }
    }

    private void DecideAction()
    {
        CheckStatus();
        SelectAction();
    }

    private void SelectAction()
    {
        float chance = Random.Range(0f, 1f);
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
                agente.SetDestination(transform.position);
                // hacer animacion de ataque
                animatorManager.PlayTargetAnimation(combo[currentAtack], true, true);
                currentAtack++;
                SetNearPlayer();
                // sigo con combo?
                break;
            case "Evadir":
                // parar enemigo
                // hacer animacion de evasión
                // marcar invencibilidad
                break;
            case "Bloquear":
                // parar enemigo
                // hacer animacion de bloqueo
                // marcar reducción de daño
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
        {
            SetChances(0.15f, 0.3f, 0.3f, 0.25f);
        }
        
        
        
        if (stats.hp < stats.maxHP / 2)
        {
            // activar flama
        }

    }

    private void SetNearPlayer()
    {
        isNearPlayer = Vector3.Distance(player.position, transform.position) < 1;
    }

    private void SetChances(float attackChance, float evadeChance, float blockChance, float useMagicChance)
    {
        chances[0].chance = attackChance;
        chances[1].chance = evadeChance;
        chances[2].chance = blockChance;
        chances[3].chance = useMagicChance;
        Array.Sort(chances);
    }
}
