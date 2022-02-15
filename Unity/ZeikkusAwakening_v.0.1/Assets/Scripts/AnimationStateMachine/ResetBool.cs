using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string isInteractingBool;
    public bool interactingStatus;
    private GameManager gameManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        animator.SetBool(isInteractingBool, interactingStatus);
        animator.applyRootMotion = false;
        if (!gameManager.inWorld) animator.SetBool("isAttacking", false);
    }
}
