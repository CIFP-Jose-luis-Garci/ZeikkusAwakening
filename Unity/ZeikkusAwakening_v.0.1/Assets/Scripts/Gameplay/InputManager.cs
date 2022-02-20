using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerLocomotion playerLocomotion;
    private AnimatorManager animatorManager;
    private GameManager gameManager;
    private CinemachineFreeLook freeLook;
    public DialogueManager dialogue;
    public ZagrantController zagrantController;
    

    public Vector2 movementInput;
    public Vector2 cameraInput;
    
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
    public CutsceneManager cutsceneManager;

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

        if (GameManager.inPause) return;
        if (inDialogue) return;
        
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        
        animatorManager.UpdateAnimatorValues(0, moveAmount);

    }

    private void HandleAInput()
    {
        if (aInput)
        {
            aInput = false;
            if (GameManager.inPause) return;
            if (inDialogue)
            {
                if (dialogue.currentEvent == GameManager.currentEvent || dialogue.showingPhrase)
                {
                    if (dialogue.NextDialogue())
                    {
                        cutsceneManager.dialogueCount++;
                        cutsceneManager.DoStuff();
                    }
                }
                else
                {
                    cutsceneManager.dialogueCount++;
                    cutsceneManager.DoStuff();
                    dialogue.gameObject.SetActive(false);
                }

                return;
            }

            if (gameManager.inWorld)
            {
                PantallaResultadosManager results = FindObjectOfType<PantallaResultadosManager>();
                if (results) results.fade = true;
                else playerLocomotion.HandleJumping();
            }
            else playerLocomotion.HandleMagic(0);
        }
    }
    private void HandleYInput()
    {
        if (yInput)
        {
            yInput = false;
            if (inDialogue || GameManager.inPause) return;
            if (!gameManager.inWorld) playerLocomotion.HandleMagic(1);
            else playerLocomotion.HandleMagic(0);
        }
    }
    private void HandleXInput()
    {
        if (xInput)
        {
            if (inDialogue || GameManager.inPause) return;
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
            if (inDialogue || GameManager.inPause) return;
            bInput = false;
            if (!gameManager.inWorld)
                playerLocomotion.HandleAttack();
            else
                StartCoroutine(playerLocomotion.HandleFirstStrike(zagrantController.gameObject));
        }
    }
    private void HandleStartInput()
    {
        if (start)
        {
            if (inDialogue) return;
            start = false;
            gameManager.Pause();
        }
    }

    private void HandleRightBump()
    {
        if (rBump)
        {
            if (inDialogue || GameManager.inPause) return;
            if (!gameManager.inWorld)
            {
                rBump = false;
                playerLocomotion.HandleEvade();
            }
        }
    }

    private void HandleLeftBump()
    {
        if (inDialogue || GameManager.inPause) return;
        if (!freeLook)
            freeLook = FindObjectOfType<CinemachineFreeLook>();
        if (lBump)
        {
            if (gameManager.inWorld)
                freeLook.m_RecenterToTargetHeading.m_enabled = true;
            else
                playerLocomotion.HandleBlock();
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
        if (inDialogue || GameManager.inPause) return;
        if (gameManager.inWorld) return;
        playerLocomotion.HandleCameraChange(lTrigger);
    }

    private void HandleRightTrigger()
    {
        if (inDialogue || GameManager.inPause) return;
        if (rTrigger)
        {
            rTrigger = false;
        }
    }

    public IEnumerator StartBattle()
    {
        animatorManager.PlayTargetAnimation("DrawSword", true);
        playerLocomotion.ResetRigidbody();
        yield return new WaitForSeconds(1.9f);
        animatorManager.animator.runtimeAnimatorController = inBattleController;
        gameManager.inWorld = false;
    }

    public IEnumerator WinBattle()
    {
        animatorManager.PlayTargetAnimation("WinBattle", true);
        playerLocomotion.ResetRigidbody();
        yield return new WaitForSeconds(1f);
        animatorManager.animator.runtimeAnimatorController = inWorldController;
        gameManager.inWorld = true;
    }
}
