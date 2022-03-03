using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyBattleManager : MonoBehaviour
{
    public GameObject sprite;
    public GameObject worldEnemy;
    protected bool recoiled;
    public bool battleStarted;
    [SerializeField] protected Slider lifebar;
    [SerializeField] private GameObject damage;
    public bool isRunning;

    protected Transform player;
    protected Animator animator;
    protected NavMeshAgent agente;
    private AudioSource source;
    protected EnemyStats stats;
    protected EscenaBatallaManager escenaBatalla;
    protected GameManager gameManager;
    private bool isAttacking;
    protected bool isRecoiling;
    protected float waitTime;
    private float hit1Length;
    private float hit2Length;
    protected float dieLength;
    protected float recoilLength;
    private float randomAttack;
    protected float time;
    private bool anyoneAlive = true;

    public AudioClip[] stepSounds;
    public AudioClip[] crySounds;
    public AudioClip[] attackSounds;
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int Muerte = Animator.StringToHash("muerte");

    private void Start()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
        player = InputManager.Instance.transform;
        agente = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();
        stats = GetComponent<EnemyStats>();
        escenaBatalla = FindObjectOfType<EscenaBatallaManager>();
        lifebar.minValue = 0;
        lifebar.maxValue = stats.maxHP;
        lifebar.value = stats.hp;
        ClipLength();
    }

    private void Update()
    {
        if (gameManager.inPause || !stats.alive || !battleStarted)
        {
            if (!stats.alive)
            {
                if (time <= 0)
                    CheckAlive();
                time += Time.deltaTime;
                if (time > dieLength)
                    CheckWinning();
                return;
            }
            animator.SetBool("alcance", false);
            isRunning = false;
            waitTime = 0;
            isAttacking = false;
            agente.SetDestination(transform.position);
            return;
        }

        if (recoiled)
        {
            if (!isRecoiling)
                StartCoroutine(Recoil());
        } else
            Run();
    }

    private void Run()
    {
        float distanceFromPlayer = Vector3.Distance(player.position, transform.position);
        if (isAttacking)
        {
            if (waitTime <= 0)
            {
                randomAttack = Random.Range(0f, 1f);
                animator.SetFloat("tipoAtaque", randomAttack);
                LookAtPlayer();
                animator.SetTrigger("alcance");
                agente.speed = 0;
            }
            if (waitTime > (randomAttack < 0.5f ? hit2Length : hit1Length))
            {
                waitTime = 0;
                if (distanceFromPlayer > 1)
                {
                    isRunning = false;
                    isAttacking = false;
                }
                return;
            }
            waitTime += Time.deltaTime;
            agente.SetDestination(transform.position);
        }
        else
        {
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

    private IEnumerator Recoil()
    {
        isRecoiling = true;
        animator.SetTrigger("daño");
        animator.SetBool("isAttacking", false);
        isAttacking = false;
        agente.SetDestination(transform.position);
        waitTime = 0;
        yield return new WaitForSeconds(recoilLength);
        isRecoiling = false;
        recoiled = false;
        agente.SetDestination(player.position);
        agente.speed = 4;
    }

    protected void LookAtPlayer()
    {
        transform.LookAt(player.position);
        Quaternion fix = transform.rotation;
        fix.x = 0;
        fix.z = 0;
        transform.rotation = fix;
    }

    public void RecieveDamage(Stats playerStats, float power, bool isPhysical, bool forceCrit = false)
    {
        if (gameManager.inPause) return;
        recoiled = true;
        int resultado;
        if (isPhysical)
            resultado = gameManager.CalcPhysDamage(playerStats, stats, power, forceCrit);
        else
            resultado = gameManager.CalcSpecDamage(playerStats, stats, power, forceCrit);
        stats.hp -= resultado;
        lifebar.value = stats.hp;
        escenaBatalla.danoTotal += resultado;
        GameObject instantiated = Instantiate(damage, transform.position, Quaternion.identity);
        instantiated.GetComponent<TextMesh>().text = resultado.ToString();
        if (stats.hp < 0 && stats.alive)
        {
            stats.alive = false;
            isAttacking = false;
            animator.applyRootMotion = true;
            animator.SetTrigger(Muerte);
            GetComponent<Collider>().enabled = false;
            ImTarget(false);
        }
    }

    protected void CheckAlive()
    {
        if (gameManager.transitioning) return;
        stats.slotEnemigo.Retract();
        agente.SetDestination(transform.position);
        anyoneAlive = false;
        foreach (EnemyBattleManager enemy in FindObjectsOfType<EnemyBattleManager>())
        {
            if (enemy.stats.alive)
            {
                anyoneAlive = true;
            }
                
        }

        if (anyoneAlive) return;
        gameManager.transitioning = true;
        gameManager.inPause = true;
    }
    protected void CheckWinning()
    {
        if (!anyoneAlive)
        {
            HUDManager.Instance.ToWinBattle();
        }
        Destroy(gameObject);
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
        animator.SetBool(IsAttacking, animationEvent.intParameter == 1);
    }

    protected void ClipLength()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "Attack":
                    hit1Length = clip.length;
                    break;
                case "Attack2":
                    hit2Length = clip.length;
                    break;
                case "Recoil":
                    recoilLength = clip.length;
                    break;
                case "Die":
                    Debug.Log(clip.length);
                    dieLength = clip.length;
                    break;
            }
        }
    }

    public void ImTarget(bool set)
    {
        sprite.SetActive(set);
    }
}