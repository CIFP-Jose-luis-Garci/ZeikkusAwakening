using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLocomotion : MonoBehaviour
{
    private PlayerManager playerManager;
    private InputManager inputManager;
    private AnimatorManager animatorManager;
    private GameManager gameManager;
    
    private Vector3 moveDirection;
    private Transform cameraObject;
    private Rigidbody rb;
    
    [Header("Falling and Landing")]
    [NonSerialized] public bool isGrounded;
    public LayerMask groundLayer;
    private float raycastHeightOffset = 0.5f;

    [NonSerialized] public bool isJumping;
    private float jumpForce = 50;

    [Header("Movement")]
    private float runningSpeed = 7;
    private float rotationSpeed = 15;
    
    [Header("Attacking")]
    private string[] animaciones;
    private Coroutine coroutine;

    [Header("Z targeting")] 
    public bool isZTargeting;
    private Transform enemyObject;

    [Header("Battle")]
    private bool invincible;
    public int[] magicSlots;
    private Transform lookInBetween;
    public CameraManager cameraManager;
    public bool blocking;
    private Stats stats;
    public Slider lifebar;
    public GameObject damage;
    public GameObject resultScreen;

    
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        animatorManager = GetComponent<AnimatorManager>();
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        cameraObject = Camera.main.transform;
        stats = GetComponent<Stats>();
        lookInBetween = FindObjectOfType<InBetweenObjectManager>().transform;
        isGrounded = true;
        animaciones = new string[stats.turnPoints];
        animaciones[0] = "final_slash";
        animaciones[1] = "hard_slash";
        animaciones[2] = "second_slash";
        animaciones[3] = "basic_slash";
    }

    public void HandleAllMovement()
    {
        //if (!stats.alive) return;
        if (gameManager.inWorld) HandleFallingAndLanding();
        if (playerManager.isInteracting) return;
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        
        moveDirection *= runningSpeed * inputManager.moveAmount;
        moveDirection.y = rb.velocity.y;
        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;
        
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y += raycastHeightOffset;
        if (!isGrounded && !isJumping)
            if (!playerManager.isInteracting)
                animatorManager.PlayTargetAnimation("Falling", true);

        if (Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, 0.6f, groundLayer))
        {
            if (!isGrounded)
            {
                Vector3 fix = rb.velocity;
                fix.x = 0;
                fix.z = 0;
                rb.velocity = fix;
                animatorManager.PlayTargetAnimation("Land", true);
            }
            
            isGrounded = true;
        }
        else
            isGrounded = false;
    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.PlayTargetAnimation("Jump", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void HandleAttack()
    {
        if (playerManager.isInteracting) return;
        if (stats.turnPoints > 0)
        {
            stats.turnPoints--;
            LookAtEnemy();
            animatorManager.PlayTargetAnimation(animaciones[stats.turnPoints], true, true);
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(ReloadTurnPoints(animatorManager.GetAnimationLength() * 3));
        }
    }

    public void HandleMagic(int slot)
    {
        if (playerManager.isInteracting) return;
        LookAtEnemy();
        ResetRigidbody();
        GetComponent<PlayerMagic>().MagicAttackLookupTable(magicSlots[slot]);
    }

    private void LookAtEnemy()
    {
        if (isZTargeting)
        {
            transform.LookAt(enemyObject);
            Quaternion rotation = transform.rotation;
            rotation.z = 0;
            rotation.x = 0;
            transform.rotation = rotation;
        }
    }

    private IEnumerator ReloadTurnPoints(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        stats.turnPoints = 4;
        coroutine = null;
    }

    public void HandleCameraChange(bool isZTargeting)
    {
        InBetweenObjectManager ibom = lookInBetween.GetComponent<InBetweenObjectManager>();
        ibom.FindEnemy(isZTargeting);
        if (isZTargeting)
        {
            enemyObject = ibom.enemy;
            cameraManager.ChangeTarget(lookInBetween);
        }
        else
        {
            cameraManager.ChangeTarget(transform);
        }
        this.isZTargeting = isZTargeting;
    }

    public void HandleEvade()
    {
        if (GameManager.win) return;
        if (!invincible)
        {
            rotationSpeed = 1000;
            HandleRotation();
            rotationSpeed = 15;
            animatorManager.PlayTargetAnimation("evade", true, true);
            invincible = true;
            StartCoroutine(Invincible(1.2f));
        }
    }

    IEnumerator Invincible(float time)
    {
        yield return new WaitForSeconds(time);
        invincible = false;
    }

    public void HandleBlock()
    {
        if (GameManager.win) return;
        blocking = animatorManager.animator.GetBool("blocking");
        if (!blocking)
        {
            animatorManager.PlayTargetAnimation("block", true);
            animatorManager.animator.SetBool("blocking", true);
        }
    }
    
    public IEnumerator HandleFirstStrike(GameObject zagrant)
    {
        if (playerManager.isInteracting) yield break;
        ResetRigidbody();
        animatorManager.PlayTargetAnimation("FirstStrikeDraw", true, true);
        yield return new WaitForSeconds(1.2f);
        zagrant.SetActive(false);
    }

    public void RecieveDamage(Stats playerStats, float power, bool isPhysical, bool forceCrit = false)
    {
        if (invincible) return;
        if (!playerManager.isInteracting) animatorManager.PlayTargetAnimation("recoil", true);
        int resultado;
        if (isPhysical)
            resultado = GameManager.CalcPhysDamage(playerStats, stats, power, forceCrit);
        else
            resultado = GameManager.CalcSpecDamage(playerStats, stats, power, forceCrit);
        if (blocking)
            resultado -= (int) (resultado * 0.5f);
        
        stats.hp -= resultado;
        lifebar.value = stats.hp;
        Vector3 damageLocation = new Vector3(transform.position.x, 1.75f, transform.position.z);
        GameObject instantiated = Instantiate(damage, damageLocation, Quaternion.identity, transform);
        instantiated.GetComponent<TextMesh>().text = resultado.ToString();
        if (stats.hp < 0)
        {
            stats.alive = false;
        }
    }

    public void ResetRigidbody()
    {
        rb.velocity = Vector3.zero;
    }

    public void HandleWinBattle()
    {
        StartCoroutine(_HandleWinBattle());
    }

    private IEnumerator _HandleWinBattle()
    {
        // win battle anim
        inputManager.WinBattle();
        GameManager.win = true;
        AudioSource source = lifebar.transform.root.GetComponent<AudioSource>();
        source.Stop();
        source.clip = gameManager.fanfare;
        source.Play();
        source.loop = false;
        yield return new WaitForSeconds(1.2f);
        // result screen
        GameManager.inPause = true;
        cameraManager.ChangeTarget(transform);
        cameraManager.ResetRaidus();
        resultScreen.SetActive(true);
    }
}
