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
    public PantallaResultadosManager results;
    public ZagrantController zagrantController;
    public AudioClip sonidoCerrarPopUp;
    

    private Vector2 movementInput;
    
    [NonSerialized] public float moveAmount;
    [NonSerialized] public float verticalInput;
    [NonSerialized] public float horizontalInput;

    [NonSerialized] public bool bInput;
    [NonSerialized] public bool aInput;
    [NonSerialized] public bool yInput;
    [NonSerialized] public bool xInput;
    [NonSerialized] public bool start;
    [NonSerialized] public bool select;
    [NonSerialized] public bool lTrigger;
    [NonSerialized] public bool rTrigger;
    [NonSerialized] public bool rBump;
    [NonSerialized] public bool lBump;
    [NonSerialized] public bool anyButtonPressed;

    public bool inDialogue;
    public CutsceneManager cutsceneManager;
    public Image minimap;
    public GameObject container, magicTutorial, buttonTutorial, lockOnTutorial, evadeBlockTutorial;
    private bool showedMagicTutorial, showedButtonTutorial, showedLockOnTutorial, showedEvadeBlockTutorial;


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
            playerControls.PlayerActions.Select.performed += i => select = true;
            playerControls.PlayerActions.Select.canceled += i => select = false;
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
        HandleSelectInput();
        HandleLeftBump();
        HandleRightBump();
        AnyActionsPressed();
    }

    private void AnyActionsPressed()
    {
        var actions = playerControls.PlayerActions;

        anyButtonPressed = verticalInput != 0 || horizontalInput != 0 ||
                           actions.A.WasPressedThisFrame() ||
                           actions.B.WasPressedThisFrame() ||
                           actions.X.WasPressedThisFrame() ||
                           actions.Y.WasPressedThisFrame() ||
                           actions.RBump.WasPressedThisFrame() ||
                           actions.LBump.WasPressedThisFrame() ||
                           actions.RTrigger.WasPressedThisFrame() ||
                           actions.LTrigger.WasPressedThisFrame() ||
                           actions.Start.WasPressedThisFrame() ||
                           actions.Select.WasPressedThisFrame();
    }

    private void HandleMovementInput()
    {
        if (inDialogue) return;
        
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        if (GameManager.inPause)
        {
            moveAmount = 0; 
            animatorManager.UpdateAnimatorValues(0, 0);
            playerLocomotion.ResetRigidbody();
            return;
        }
        
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }

    private void HandleAInput()
    {
        if (aInput)
        {
            aInput = false;
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
                    if (dialogue.gameObject.activeSelf)
                    {
                        cutsceneManager.dialogueCount++;
                        cutsceneManager.DoStuff();
                    }
                    dialogue.gameObject.SetActive(false);
                }

                return;
            }

            if (gameManager.inWorld)
            {
                if (GameManager.transitioning)
                    results.End();
                else
                {
                    if (GameManager.inPause) return;
                    playerLocomotion.HandleJumping();
                }
            }
            else
            {
                if (GameManager.transitioning || GameManager.inPause) return;
                showedButtonTutorial = ShowTutorial(buttonTutorial, showedButtonTutorial);
                playerLocomotion.HandleMagic(0);
            }
        }
    }
    private void HandleYInput()
    {
        if (yInput)
        {
            yInput = false;
            if (AnyInteraction()) return;
            if (!gameManager.inWorld)
                showedButtonTutorial = ShowTutorial(buttonTutorial, showedButtonTutorial);
            else
                showedMagicTutorial = ShowTutorial(magicTutorial, showedMagicTutorial);
            playerLocomotion.HandleMagic(1);
        }
    }
    private void HandleXInput()
    {
        if (xInput)
        {
            if (AnyInteraction()) return;
            if (!gameManager.inWorld)
            {
                xInput = false;
                showedButtonTutorial = ShowTutorial(buttonTutorial, showedButtonTutorial);
                playerLocomotion.HandleMagic(2);
            }
        }
    }

    private void HandleBInput()
    {
        if (bInput)
        {
            if (inDialogue) return;
            if (GameManager.inPause)
            {
                if (GameManager.transitioning) return;
                if (gameManager.Pause())
                {
                    bInput = false;
                }
                return;
            }
            bInput = false;
            if (!gameManager.inWorld)
            {
                if (GameManager.transitioning) return;
                showedButtonTutorial = ShowTutorial(buttonTutorial, showedButtonTutorial);
                playerLocomotion.HandleAttack();
            }
            else
                StartCoroutine(playerLocomotion.HandleFirstStrike(zagrantController.gameObject));
        }
    }

    private bool ShowTutorial(GameObject tutorial, bool condition)
    {
        if (condition) return true;
        GameManager.SpawnTutorial(container, tutorial, null);
        return true;
    }
    private void HandleStartInput()
    {
        if (start)
        {
            start = false;
            if (inDialogue)
            {
                cutsceneManager.EndCutScene();
                return;
            }
            gameManager.Pause();
        }
    }

    private void HandleSelectInput()
    {
        if (select)
        {
            select = false;
            if (AnyInteraction()) return;
            bool activeSelf = minimap.gameObject.activeSelf;
            minimap.gameObject.SetActive(!activeSelf);
        }
    }

    private void HandleRightBump()
    {
        if (rBump)
        {
            if (AnyInteraction()) return;
            if (!gameManager.inWorld)
            {
                rBump = false;
                showedEvadeBlockTutorial = ShowTutorial(evadeBlockTutorial, showedEvadeBlockTutorial);
                playerLocomotion.HandleEvade();
            }
        }
    }

    private void HandleLeftBump()
    {
        if (AnyInteraction()) return;
        if (!freeLook)
            freeLook = FindObjectOfType<CinemachineFreeLook>();
        if (lBump)
        {
            if (gameManager.inWorld)
                freeLook.m_RecenterToTargetHeading.m_enabled = true;
            else
            {
                showedEvadeBlockTutorial = ShowTutorial(evadeBlockTutorial, showedEvadeBlockTutorial);
                playerLocomotion.HandleBlock();
            }
        }
        else
        {
            if (!gameManager.inWorld)
            {
                animatorManager.animator.SetBool("blocking", false);
                playerLocomotion.blocking = false;
            }
            else
                freeLook.m_RecenterToTargetHeading.m_enabled = false;
        }

    }

    private void HandleLeftTrigger()
    {
        if (inDialogue || GameManager.inPause) return;
        if (gameManager.inWorld) return;
        if (lTrigger)
        {
            showedLockOnTutorial = ShowTutorial(lockOnTutorial, showedLockOnTutorial);
        }
        playerLocomotion.HandleCameraChange(lTrigger);
    }

    private void HandleRightTrigger()
    {
        if (rTrigger)
        {
            rTrigger = false;
            TutorialManager tutorialManager = container.GetComponentInChildren<TutorialManager>();
            if (tutorialManager)
                tutorialManager.ReverseAndDestroy();
            
            if (inDialogue || GameManager.inPause) return;
        }
    }

    private bool AnyInteraction()
    {
        return inDialogue || GameManager.inPause || GameManager.transitioning;
    }
    
    public void GoBack(GameObject toDisable, GameObject pantallaPausa)
    {
        if (bInput)
        {
            gameManager.source.PlayOneShot(sonidoCerrarPopUp);
            toDisable.SetActive(false);
            pantallaPausa.SetActive(true);
            pantallaPausa.GetComponentInChildren<Button>().Select();
            bInput = false;
        }
    }
}
