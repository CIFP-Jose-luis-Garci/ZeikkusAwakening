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
    [Header("Zagrant")]
    public GameObject zagrant;
    
    public GameObject appearParticles, disappearParticles;
    [Header("Sound")]
    public AudioSource source;
    public AudioClip[] stepSounds;
    public AudioClip[] swordSounds;
    public AudioClip[] magicSounds;
    public AudioClip[] zeikkuSounds;
    public AudioClip[] jumpSounds;
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

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool useRootMotion = false, float transitionDuration = 0.2f)
    {
        animator.SetBool("isInteracting", isInteracting);
        if (useRootMotion)
            animator.applyRootMotion = true;
        animator.CrossFade(targetAnimation, transitionDuration);
        
    }

    public int GetAnimationLength()
    {
        return animator.GetCurrentAnimatorClipInfo(1).Length;
    }

    public void ChangeWorld(bool value)
    {
        GameManager.Instance.inWorld = value;
        animator.SetBool("inWorld", value);
    }
    
    public void Step(AnimationEvent animationEvent)
    {
        float weight = animationEvent.animatorClipInfo.weight;
        if(weight > 0.8f && !animator.GetBool("isInteracting"))
        {
            source.PlayOneShot(stepSounds[Mathf.FloorToInt(Random.Range(0, stepSounds.Length))]);
        }
    }

    public void ZeikkuCrySound(AnimationEvent animationEvent)
    {
        source.PlayOneShot(zeikkuSounds[animationEvent.intParameter]);
    }
    
    public void SwordSwingSound(AnimationEvent animationEvent)
    {
        source.PlayOneShot(swordSounds[animationEvent.intParameter]);
    }

    public void IsAttackingSet(AnimationEvent animationEvent)
    {
        animator.SetBool("isAttacking", animationEvent.intParameter == 1);
    }

    public void CanStartBattle(AnimationEvent animationEvent)
    {
        animator.SetBool("canStartBattle", animationEvent.intParameter == 1);
    }

    public void MagicSound(AnimationEvent animationEvent)
    {
        source.PlayOneShot(magicSounds[animationEvent.intParameter]);
    }

    public void BlockSound()
    {
        source.PlayOneShot(swordSounds[4]);
    }

    public void JumpSound(AnimationEvent animationEvent)
    {
        source.PlayOneShot(jumpSounds[animationEvent.intParameter]);
    }

    public void DrawSwordParticle()
    {
        Transform zagranPos = zagrant.transform;
        Instantiate(appearParticles, zagranPos.position, zagranPos.rotation).GetComponent<ParticleSystem>().Play();
    }

    public void HideSwordParticle()
    {
        Transform zagranPos = zagrant.transform;
        Instantiate(disappearParticles, zagranPos.position, zagranPos.rotation).GetComponent<ParticleSystem>().Play();
    }

    public void HideSword()
    {
        zagrant.SetActive(false);
        source.PlayOneShot(swordSounds[6]);
    }

    public void DrawSword()
    {
        zagrant.SetActive(true);
        source.PlayOneShot(swordSounds[5]);
    }

    public void SpawnFire(AnimationEvent animationEvent)
    {
        zagrant.GetComponent<ZagrantController>().onFire = true;
        DestroyParticle oldFire = zagrant.GetComponentInChildren<DestroyParticle>();
        if (oldFire)
            Destroy(oldFire.gameObject);
        Instantiate(animationEvent.objectReferenceParameter, zagrant.transform.position, zagrant.transform.rotation, 
            zagrant.transform);
    }
}
