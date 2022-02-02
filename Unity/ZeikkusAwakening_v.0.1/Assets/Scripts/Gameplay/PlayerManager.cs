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
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting");
        //isUsingRootMotion = animator.GetBool("isUsingRootMotion");
        if (gameManager.inWorld)
        {
            playerLocomotion.isJumping = animator.GetBool("isJumping");
            animator.SetBool("isGrounded", playerLocomotion.isGrounded);
        } 
    }
}
