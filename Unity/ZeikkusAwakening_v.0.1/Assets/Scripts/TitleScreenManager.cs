using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public Image logobg, logo, titlebg, titlelogo, titlepress;
    public GameObject selecciones;

    private InputManager iMgr;
    private PlayerControls playerControls;
    private bool titlepressActive;
    private bool titlepressVisible;
    private Vector2 movementInput;
    private bool bInput;
    private bool aInput;
    
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

        }
        playerControls.Enable();
    }

    private void Start()
    {
        logobg.CrossFadeAlpha(0, 0, true);
        logo.CrossFadeAlpha(0, 0, true);
        titlebg.CrossFadeAlpha(0, 0, true);
        titlelogo.CrossFadeAlpha(0, 0, true);
        titlepress.CrossFadeAlpha(0, 0, true);
        StartCoroutine(AnimateTitleScreen());
    }

    IEnumerator AnimateTitleScreen()
    {
        logobg.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(1f);
        logo.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(2f);
        logo.CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1f);
        logobg.CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1f);
        titlebg.CrossFadeAlpha(1, 1, true);
        titlelogo.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(1f);
        titlepress.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(1f);
        titlepressActive = true;
        titlepressVisible = true;
        StartCoroutine(AnimatePressStart());
    }
    
    IEnumerator AnimatePressStart()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f);
            if (titlepressActive)
            {
                titlepress.CrossFadeAlpha(titlepressVisible ? 0 : 1, 2, true);
                titlepressVisible = !titlepressVisible;
            }
        }
    }

    private void Update()
    {
        if (titlepressActive)
            if (aInput)
            {
                aInput = false;
                titlepressActive = false;
                titlepressVisible = false;
                selecciones.SetActive(true);
                titlepress.enabled = false;
            }
                
    }
}
