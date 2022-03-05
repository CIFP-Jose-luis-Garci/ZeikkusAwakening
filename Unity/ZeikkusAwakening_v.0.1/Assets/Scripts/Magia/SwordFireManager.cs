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
        animatorManager = InputManager.Instance.GetComponent<AnimatorManager>();
        gameManager = GameManager.Instance;
        if (gameManager.inWorld)
            animatorManager.PlayTargetAnimation("magicSelected firecast", true, true);
        else
            animatorManager.PlayTargetAnimation("magic firecast", true);
    }

    private void Update()
    {
        if (!gameManager.inWorld && gameManager.inPause) return;
        time += Time.deltaTime;
        if (!zagrantController)
            zagrantController = animatorManager.zagrant.GetComponent<ZagrantController>();
        if (gameManager.inWorld && time > 1.5f || !gameManager.inWorld && time > 20)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        DestroyParticle dp = zagrantController.GetComponentInChildren<DestroyParticle>();
        zagrantController.onFire = false;
        if (dp)
            Destroy(dp.gameObject);
        if (gameManager.inWorld && !gameManager.transitioning)
            zagrantController.gameObject.SetActive(false);
        Destroy(gameObject);
        
    }
}
