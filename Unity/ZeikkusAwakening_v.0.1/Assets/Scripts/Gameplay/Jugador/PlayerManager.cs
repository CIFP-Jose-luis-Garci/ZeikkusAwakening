using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    
    private Animator animator;
    private InputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    
    private GameManager gameManager;
    public bool isInteracting;
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        playerLocomotion = GetComponent<PlayerLocomotion>();
        gameManager.checkpoint = transform.position;
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        if (gameManager.inPause || gameManager.transitioning || gameManager.inCutscene) return;
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        isInteracting = animator.GetBool(IsInteracting);
        if (gameManager.inWorld)
        {
            playerLocomotion.isJumping = animator.GetBool(IsJumping);
            animator.SetBool(IsGrounded, playerLocomotion.isGrounded);
        } 
    }
}
