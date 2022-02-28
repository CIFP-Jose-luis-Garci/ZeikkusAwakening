using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfazMundoManager : MonoBehaviour
{
    private Animator animator;
    private GameManager gameManager;
    private InputManager inputManager;
    private float timeToShow = 2;
    private bool showing;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        inputManager = FindObjectOfType<InputManager>();
    }

    void Update()
    {
        if (inputManager.anyButtonPressed || inputManager.inDialogue || !gameManager.inWorld || GameManager.inPause)
        {
            timeToShow = 2;
            if (showing)
            {
                animator.SetBool("showed", false);
                showing = false;
            }
            return;
        }

        if (!showing)
        {
            timeToShow -= Time.deltaTime;
            if (timeToShow <= 0)
            {
                showing = true;
                animator.SetBool("showed", true);
            }
        }
    }
}
