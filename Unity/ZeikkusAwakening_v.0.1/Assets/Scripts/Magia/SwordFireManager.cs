using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SwordFireManager : Magic
{
    private AnimatorManager animatorManager;
    private GameManager gameManager;
    private float time;
    private ZagrantController zagrantController;
    void Start()
    {
        animatorManager = FindObjectOfType<AnimatorManager>();
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager.inWorld)
            animatorManager.PlayTargetAnimation("magicSelected firecast", true, true);
        else
            animatorManager.PlayTargetAnimation("magic firecast", true);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (!zagrantController)
            zagrantController = FindObjectOfType<ZagrantController>();
        if (gameManager.inWorld && time > 1.5f)
        {
            if (GameManager.transitioning) return;
            DestroyParticle dp = zagrantController.GetComponentInChildren<DestroyParticle>();
            zagrantController.onFire = false;
            if (dp)
                Destroy(dp.gameObject);
            zagrantController.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        if (!gameManager.inWorld && time > 20)
        {
            zagrantController.onFire = false;
            Destroy(gameObject);
        }
    }
}
