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
    public bool isUsingRootMotion;
    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        GameManager.checkpoint = transform.position;
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        if (GameManager.inPause || GameManager.transitioning || GameManager.inCutscene) return;
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        isInteracting = animator.GetBool(IsInteracting);
        //isUsingRootMotion = animator.GetBool("isUsingRootMotion");
        if (gameManager.inWorld)
        {
            playerLocomotion.isJumping = animator.GetBool(IsJumping);
            animator.SetBool(IsGrounded, playerLocomotion.isGrounded);
        } 
    }
}
