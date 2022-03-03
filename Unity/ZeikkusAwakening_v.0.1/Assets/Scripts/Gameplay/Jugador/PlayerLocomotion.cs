using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public bool isGrounded;

    public LayerMask groundLayer;
    public bool isJumping;
    private float jumpForce = 50;

    [Header("Movement")] 
    public float runningSpeed = 7;
    private float rotationSpeed = 15;

    [Header("Textures")] 
    public SkinnedMeshRenderer body;
    public Texture faceEyesOpen;
    public Texture faceEyesClosed;

    [Header("Attacking")] 
    private string[] animaciones;
    private Coroutine coroutine;

    [Header("Z targeting")] 
    public bool isZTargeting;
    internal Transform enemyObject;

    [Header("Battle")] 
    private bool striking;
    private bool invincible;
    public int[] magicSlots;
    private Transform lookInBetween;
    public CameraManager cameraManager;
    public bool blocking;
    private Stats stats;
    public Slider lifebar;
    public GameObject damage, deathVolume; 
    public AudioClip playerDeath;
    private static readonly int Blocking = Animator.StringToHash("blocking");


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        rb = GetComponent<Rigidbody>();
        gameManager = GameManager.Instance;
        cameraObject = Camera.main.transform;
        stats = GetComponent<Stats>();
        lookInBetween = FindObjectOfType<InBetweenObjectManager>().transform;
        isGrounded = true;
        animaciones = new string[stats.turnPoints];
        animaciones[0] = "final_slash";
        animaciones[1] = "hard_slash";
        animaciones[2] = "second_slash";
        animaciones[3] = "basic_slash";
        StartCoroutine(Blink());
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    internal void GravitySet(bool set)
    {
        rb.useGravity = set;
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
        Quaternion playerRotation =
            Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        if (rb.velocity.y <= 0)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, 10f, groundLayer);
            if ((hit.distance < 0.5f || rb.velocity.y == 0) && !isJumping && !isGrounded)
            {
                Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
                Debug.Log(hit.distance);
                Vector3 fix = rb.velocity;
                fix.x = 0;
                fix.z = 0;
                rb.velocity = fix;
                animatorManager.PlayTargetAnimation("Land", true);
                isGrounded = true;
            }
        }

    }

    public void HandleJumping()
    {
        if (isGrounded && !playerManager.isInteracting)
        {
            animatorManager.PlayTargetAnimation("Jump", true);
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
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
        ResetRigidbody();
        if (GetComponent<PlayerMagic>().MagicAttackLookupTable(magicSlots[slot]))
            LookAtEnemy();
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
            enemyObject = null;
            cameraManager.ChangeTarget(transform);
        }

        this.isZTargeting = isZTargeting;
    }

    public void HandleEvade()
    {
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
        blocking = animatorManager.animator.GetBool(Blocking);
        if (!blocking)
        {
            animatorManager.PlayTargetAnimation("block", true);
            animatorManager.animator.SetBool(Blocking, true);
        }
    }

    public IEnumerator HandleFirstStrike(GameObject zagrant)
    {
        if (playerManager.isInteracting) yield break;
        ResetRigidbody();
        animatorManager.PlayTargetAnimation("FirstStrikeDraw", true, true);
        striking = true;
        yield return new WaitForSeconds(1f);
        striking = false;
        zagrant.SetActive(false);
    }

    public void RecieveDamage(Stats playerStats, float power, bool isPhysical, bool forceCrit = false)
    {
        if (gameManager.inPause || invincible || !stats.alive) return;
        ResetRigidbody();
        if (!playerManager.isInteracting) animatorManager.PlayTargetAnimation("recoil", true);
        int resultado;
        if (isPhysical)
            resultado = gameManager.CalcPhysDamage(playerStats, stats, power, forceCrit);
        else
            resultado = gameManager.CalcSpecDamage(playerStats, stats, power, forceCrit);
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
            animatorManager.PlayTargetAnimation("Die", true, true);
            Time.timeScale = 0.5f;
            deathVolume.SetActive(true);
            gameManager.source.PlayOneShot(playerDeath);
            GameManager.Instance.transitioning = true;
            HUDManager.Instance.ToDieInBattle(transform, stats, deathVolume);
        }
    }

    public void ResetRigidbody()
    {
        rb.velocity = Vector3.zero;
    }

    private IEnumerator Blink()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (!inputManager.inDialogue)
            {
                Material material = body.materials[1];
                material.mainTexture = faceEyesClosed;
                yield return new WaitForSeconds(0.1f);
                material.mainTexture = faceEyesOpen;
            }

            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (striking) return;
        if (collision.gameObject.CompareTag("EnemigoWorld"))
        {
            HUDManager.Instance.StartBattle(collision.gameObject, false);
        }
    }
}