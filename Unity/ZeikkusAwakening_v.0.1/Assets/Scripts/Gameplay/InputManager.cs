using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerLocomotion playerLocomotion;
    private AnimatorManager animatorManager;
    private GameManager gameManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;
    
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool bInput;
    public bool aInput;
    public bool lTrigger;
    public bool rBump;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        gameManager = FindObjectOfType<GameManager>();
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
            playerControls.PlayerActions.LTrigger.performed += i => lTrigger = true;
            playerControls.PlayerActions.LTrigger.canceled += i => lTrigger = false;
            playerControls.PlayerActions.RBump.performed += i => rBump = true;
            playerControls.PlayerActions.RBump.canceled += i => rBump = false;

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
        if (gameManager.inWorld) HandleJumpingInput();
        if (!gameManager.inWorld) HandleAttackInput();
        if (!gameManager.inWorld) HandleCameraTargetingInput();
        HandleRightBump();
    }
    
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
        
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        if (bInput) // if wants to walk
        {
            moveAmount = Mathf.Clamp(moveAmount, 0, 0.5f); // limit speed and blend tree transform
        }
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }

    private void HandleJumpingInput()
    {
        if (aInput)
        {
            aInput = false;
            playerLocomotion.HandleJumping();
        }
    }

    private void HandleAttackInput()
    {
        if (bInput)
        {
            bInput = false;
            playerLocomotion.HandleAttack();
        }
    }

    private void HandleRightBump()
    {
        if (rBump && !gameManager.inWorld)
        {
            rBump = false;
            playerLocomotion.HandleEvade();
        }
    }

    private void HandleCameraTargetingInput()
    {
        if (lTrigger)
        {
            lTrigger = false;
            playerLocomotion.HandleCameraChange();
        }
    }
}
