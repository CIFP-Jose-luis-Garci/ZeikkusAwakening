using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AnimatorManager : MonoBehaviour
{
    [NonSerialized] public Animator animator;
    private int horizontal;
    private int vertical;
    public AudioSource source;
    public AudioClip[] stepSounds;
    public AudioClip[] swordSounds;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        // Snap to animations, not blend to make it look good
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            snappedHorizontal = 0.5f;
        else if (horizontalMovement > 0.55f)
            snappedHorizontal = 1;
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            snappedHorizontal = 0.5f;
        else if (horizontalMovement < -0.55f)
            snappedHorizontal = -1;
        else
            snappedHorizontal = 0;
        #endregion
        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
            snappedVertical = 0.5f;
        else if (verticalMovement > 0.55f)
            snappedVertical = 1;
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
            snappedVertical = 0.5f;
        else if (verticalMovement < -0.55f)
            snappedVertical = -1;
        else
            snappedVertical = 0;
        #endregion
        
        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool useRootMotion = false)
    {
        animator.SetBool("isInteracting", isInteracting);
        if (useRootMotion)
            animator.applyRootMotion = true;
        animator.CrossFade(targetAnimation, 0.2f);
        
    }

    public int GetAnimationLength()
    {
        return animator.GetCurrentAnimatorClipInfo(1).Length;
    }
    
    public void Step(AnimationEvent animationEvent)
    {
        float weight = animationEvent.animatorClipInfo.weight;
        if(weight > 0.8f)
        {
            source.PlayOneShot(stepSounds[Mathf.FloorToInt(Random.Range(0, stepSounds.Length - 0.1f))]);
        }
    }

}
