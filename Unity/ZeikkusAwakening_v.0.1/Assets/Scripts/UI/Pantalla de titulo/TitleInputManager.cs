using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TitleInputManager : MonoBehaviour
{
    private PlayerControls playerControls;

    [NonSerialized] public bool bInput;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerActions.B.performed += i => bInput = true;
            playerControls.PlayerActions.B.canceled += i => bInput = false;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        HandleBInput();
    }


    private void HandleBInput()
    {
        if (bInput)
        {
            bInput = false;
            OptionsManager options = FindObjectOfType<OptionsManager>();
            if (options)
                options.gameObject.SetActive(false);
        }
    }
}
