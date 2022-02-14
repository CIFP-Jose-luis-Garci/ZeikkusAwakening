using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerLocomotion playerLocomotion;
    private AnimatorManager animatorManager;
    private GameManager gameManager;
    private CinemachineFreeLook freeLook;
    public ZagrantController zagrantController;
    

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;
    
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool bInput;
    public bool aInput;
    public bool yInput;
    public bool xInput;
    public bool start;
    public bool lTrigger;
    public bool rTrigger;
    public bool rBump;
    public bool lBump;

    public bool inDialogue;
    public bool inPause;

    public RuntimeAnimatorController inBattleController;
    public RuntimeAnimatorController inWorldController;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        gameManager = FindObjectOfType<GameManager>();
        freeLook = FindObjectOfType<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.B.performed += i => bInput = true;
            playerControls.PlayerActions.B.canceled += i => bInput = false;
            playerControls.PlayerActions.A.performed += i => aInput = true;
            playerControls.PlayerActions.A.canceled += i => aInput = false;
            playerControls.PlayerActions.Y.performed += i => yInput = true;
            playerControls.PlayerActions.Y.canceled += i => yInput = false;
            playerControls.PlayerActions.X.performed += i => xInput = true;
            playerControls.PlayerActions.X.canceled += i => xInput = false;
            playerControls.PlayerActions.Start.performed += i => start = true;
            playerControls.PlayerActions.Start.canceled += i => start = false;
            playerControls.PlayerActions.LTrigger.performed += i => lTrigger = true;
            playerControls.PlayerActions.LTrigger.canceled += i => lTrigger = false;
            playerControls.PlayerActions.RTrigger.performed += i => rTrigger = true;
            playerControls.PlayerActions.RTrigger.canceled += i => rTrigger = false;
            playerControls.PlayerActions.RBump.performed += i => rBump = true;
            playerControls.PlayerActions.RBump.canceled += i => rBump = false;
            playerControls.PlayerActions.LBump.performed += i => lBump = true;
            playerControls.PlayerActions.LBump.canceled += i => lBump = false;

        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleLeftTrigger();
        HandleRightTrigger();
        HandleAInput();
        HandleBInput();
        HandleXInput();
        HandleYInput();
        HandleStartInput();
        HandleLeftBump();
        HandleRightBump();
    }
    
    private void HandleMovementInput()
    {

        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        if (inPause) return;
        if (inDialogue) return;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
        
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        if (bInput) // if wants to walk
        {
            moveAmount = Mathf.Clamp(moveAmount, 0, 0.5f); // limit speed and blend tree transform
        }
        animatorManager.UpdateAnimatorValues(0, moveAmount);

    }

    private void HandleAInput()
    {
        if (aInput)
        {
            aInput = false;
            if (inDialogue || inPause) return;
            if (gameManager.inWorld) playerLocomotion.HandleJumping();
            else playerLocomotion.HandleMagic(0);
        }
    }
    private void HandleYInput()
    {
        if (yInput)
        {
            yInput = false;
            if (inDialogue || inPause) return;
            if (!gameManager.inWorld) playerLocomotion.HandleMagic(1);
            else playerLocomotion.HandleMagic(0);
        }
    }
    private void HandleXInput()
    {
        if (xInput)
        {
            if (inDialogue || inPause) return;
            if (!gameManager.inWorld)
            {
                xInput = false;
                playerLocomotion.HandleMagic(2);
            }
        }
    }

    private void HandleBInput()
    {
        if (bInput)
        {
            if (inDialogue || inPause) return;
            if (!gameManager.inWorld)
            {
                bInput = false;
                playerLocomotion.HandleAttack();
            }
        }
    }
    private void HandleStartInput()
    {
        if (start)
        {
            start = false;
            gameManager.Pause();
        }
    }

    private void HandleRightBump()
    {
        if (rBump)
        {
            if (inDialogue || inPause) return;
            if (!gameManager.inWorld)
            {
                rBump = false;
                playerLocomotion.HandleEvade();
            }
        }
    }

    private void HandleLeftBump()
    {
        if (lBump)
        {
            if (inDialogue || inPause) return;
            if (gameManager.inWorld) freeLook.m_RecenterToTargetHeading.m_enabled = true;
            else
            {
                playerLocomotion.HandleBlock();
            }
        }
        else
        {
            if (!gameManager.inWorld)
                animatorManager.animator.SetBool("blocking", false);
            else
                freeLook.m_RecenterToTargetHeading.m_enabled = false;
        }

    }

    private void HandleLeftTrigger()
    {
        if (inDialogue || inPause) return;
        if (gameManager.inWorld) return;
        playerLocomotion.HandleCameraChange(lTrigger);
    }

    private void HandleRightTrigger()
    {
        if (inDialogue || inPause) return;
        if (rTrigger)
        {
            rTrigger = false;
            if (gameManager.inWorld)
            {
                StartCoroutine(DrawSword());
            }
            else
            {
                StartCoroutine(Win());
            }
        }
    }

    private IEnumerator DrawSword()
    {
        animatorManager.animator.SetBool("isInteracting", true);
        animatorManager.animator.SetTrigger("toBattle");
        yield return new WaitForSeconds(1.9f);
        gameManager.inWorld = false;
        animatorManager.animator.runtimeAnimatorController = inBattleController;
    }

    private IEnumerator Win()
    {
        animatorManager.PlayTargetAnimation("win", true);
        yield return new WaitForSeconds(1f);
        gameManager.inWorld = true;
        animatorManager.animator.runtimeAnimatorController = inWorldController;
    }
}
