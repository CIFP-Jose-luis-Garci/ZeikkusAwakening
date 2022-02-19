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
    public Rigidbody rb;
    
    [Header("Falling and Landing")]
    public bool isGrounded;
    public LayerMask groundLayer;
    public float raycastHeightOffset = 0.5f;

    public bool isJumping;
    public float jumpForce = 3;

    [Header("Movement")]
    public float runningSpeed = 7;
    public float rotationSpeed = 15;
    
    [Header("Attacking")]
    public int puntosDeTurno = 4;
    public string[] animaciones;
    private Coroutine coroutine;

    [Header("Z targeting")] 
    public bool isZTargeting;
    public LayerMask enemyLayer;
    private Transform enemyObject;

    [Header("Battle")]
    public bool invincible;
    public int[] magicSlots;
    public Transform lookInBetween;
    public CameraManager cameraManager;
    public bool blocking;
    private Stats stats;
    public Slider lifebar;
    public GameObject damage;

    
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        animatorManager = GetComponent<AnimatorManager>();
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        cameraObject = Camera.main.transform;
        stats = GetComponent<Stats>();
        animaciones = new string[puntosDeTurno];
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
                animatorManager.PlayTargetAnimation("Land", true);
            
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
            coroutine = StartCoroutine(ReloadTurnPoints(animatorManager.GetAnimationLength() * 2));
        }
    }

    public void HandleMagic(int slot)
    {
        if (playerManager.isInteracting) return;
        LookAtEnemy();
        ResetRigidbody();
        animatorManager.PlayTargetAnimation("magic", true);
        GetComponent<Magic>().MagicAttackLookupTable(magicSlots[slot]);
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

    public void HandleCameraChange(bool isZTargeting = false)
    {
        InBetweenObjectManager ibom = lookInBetween.gameObject.GetComponent<InBetweenObjectManager>();
        ibom.FindEnemy();
        if (isZTargeting)
        {
            enemyObject = ibom.enemy;
            ibom.enemyManager.ImTarget(true);
            cameraManager.ChangeTarget(lookInBetween);
            
        }
        else
        {
            cameraManager.ChangeTarget(transform);
            if (ibom.enemyFound)
                ibom.enemyManager.ImTarget(false);
        }
        this.isZTargeting = isZTargeting;
    }

    public void HandleEvade()
    {
        if (!invincible)
        {
            animatorManager.PlayTargetAnimation("evade", true, true);
            invincible = true;
            StartCoroutine(Invincible(1));
        }
    }

    IEnumerator Invincible(float time)
    {
        yield return new WaitForSeconds(time);
        invincible = false;
    }

    public void HandleBlock()
    {
        if (!animatorManager.animator.GetBool("blocking"))
        {
            animatorManager.PlayTargetAnimation("block", true);
            animatorManager.animator.SetBool("blocking", true);
        }
    }
    
    public IEnumerator HandleFirstStrike(GameObject zagrant)
    {
        if (playerManager.isInteracting) yield break;
        ResetRigidbody();
        animatorManager.PlayTargetAnimation("FirstStrikeDraw", true);
        yield return new WaitForSeconds(1.2f);
        zagrant.SetActive(false);
    }

    public void RecieveDamage(Stats playerStats, float power, bool isPhysical)
    {
        if (invincible) return;
        animatorManager.PlayTargetAnimation("recoil", true);
        int resultado;
        if (isPhysical)
            resultado = GameManager.CalcPhysDamage(playerStats, stats, power);
        else
            resultado = GameManager.CalcSpecDamage(playerStats, stats, power);
        stats.hp -= resultado;
        lifebar.value = stats.hp;
        GameObject instantiated = Instantiate(damage, transform.position, Quaternion.identity, transform);
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

}
