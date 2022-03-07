using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BossBattleManager : EnemyBattleManager
{
    private bool isNearPlayer, isAttacking, isEvading, isBlocking, isUsingMagic, isInteracting;
    private Chance[] chances;
    private string[] combo;
    private AnimatorManager animatorManager;
    private PlayerMagic magic;
    public int[] magicSlots;
    public GameObject pistaJefe;

    [Header("Attacks")] 
    private int currentAtack = 0;
    private bool cooldown;
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

    private void Awake()
    {
        gameManager = GameManager.Instance;
        player = InputManager.Instance.transform;
        gameManager.SpawnTutorial(HUDManager.Instance.tutorialContainer, pistaJefe, null);
        animatorManager = GetComponent<AnimatorManager>();
        stats = GetComponent<EnemyStats>();
        agente = GetComponent<NavMeshAgent>();
        magic = GetComponent<PlayerMagic>();
        escenaBatalla = FindObjectOfType<EscenaBatallaManager>();
        lifebar.maxValue = stats.hp;
        lifebar.value = stats.hp;
        chances = new Chance[4];
        chances[0] = new Chance("Atacar");
        chances[1] = new Chance("Evadir");
        chances[2] = new Chance("Bloquear");
        chances[3] = new Chance("Usar magia");
        combo = new[] {"basic_slash", "second_slash", "hard_slash", "final_slash"};
    }

    void Start()
    {
        animator = animatorManager.animator;
        ClipLength();
    }

    private void Update()
    {
        if (GameManager.Instance.inPause || !stats.alive || !battleStarted)
        {
            agente.speed = 0;
            agente.SetDestination(transform.position);
            if (stats.alive) return;
            gameManager.bossDefeated = true;
            if (time <= 0)
                CheckAlive();
            time += Time.deltaTime;
            if (time > dieLength)
                CheckWinning();
            return;
        }

        SetNearPlayer();

        if (Recoiling() || Interacting() || CoolingDown()) return;

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

    private void LateUpdate()
    {
        isInteracting = animator.GetBool(IsInteracting);
    }

    private bool CoolingDown()
    {
        if (cooldown)
        {
            if (waitTime > 0.35f)
            {
                cooldown = false;
                if (!isNearPlayer) isAttacking = false;
                isEvading = false;
                isBlocking = false;
                return false;
            }

            waitTime += Time.deltaTime;
            return true;
        }

        return false;
    }

    private bool Interacting()
    {
        if (isInteracting)
        {
            cooldown = true;
            waitTime = 0;
            return true;
        }

        return false;
    }

    private bool Recoiling()
    {
        if (recoiled)
        {
            if (isRecoiling) return false;
            isRecoiling = true;
            StartCoroutine(Recoil());
            return true;
        }

        return false;
    }

    private IEnumerator Recoil()
    {
        isRecoiling = true;
        animatorManager.PlayTargetAnimation("recoil", true);
        animator.SetBool(IsAttacking, false);
        agente.SetDestination(transform.position);
        waitTime = 0;
        yield return new WaitForSeconds(recoilLength);
        isRecoiling = false;
        recoiled = false;
        isAttacking = false;
        agente.SetDestination(player.position);
        agente.speed = 5;
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
                if (magic.MagicAttackLookupTable(magicSlots[Random.Range(0, magicSlots.Length)]))
                {
                    StopAIandAnimate("magic fireball");
                    LookAtPlayer();
                }
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