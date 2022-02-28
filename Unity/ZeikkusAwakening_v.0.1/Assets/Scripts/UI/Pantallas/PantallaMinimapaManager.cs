using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantallaMinimapaManager : MonoBehaviour
{
    private BillboardSprites[] spritesConBillboard;
    private InputManager inputManager;

    private void Awake()
    {
        spritesConBillboard = FindObjectsOfType<BillboardSprites>();
        inputManager = FindObjectOfType<InputManager>();
    }

    void OnEnable()
    {
        foreach (BillboardSprites sprite in spritesConBillboard)
        {
            sprite.GetComponent<BillboardSprites>().enabled = false;
            sprite.transform.rotation = Quaternion.Euler(90, 0,0);
        }

        GameManager.inPause = true;
        GameManager.viewingMinimap = false;
    }

    private void Update()
    {
        if (inputManager.select)
        {
            inputManager.select = false;
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        
        foreach (BillboardSprites sprite in spritesConBillboard)
        {
            sprite.GetComponent<BillboardSprites>().enabled = true;
        }
        
        GameManager.inPause = false;
        GameManager.viewingMinimap = false;
    }
}
