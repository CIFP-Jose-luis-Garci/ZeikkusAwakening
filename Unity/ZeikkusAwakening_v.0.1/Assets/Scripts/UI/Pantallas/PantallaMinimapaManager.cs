using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantallaMinimapaManager : MonoBehaviour
{
    private BillboardSprites[] spritesConBillboard;
    private InputManager inputManager;
    private float scrollSpeed;
    public RectTransform minimapa;

    private void Awake()
    {
        spritesConBillboard = FindObjectsOfType<BillboardSprites>();
        inputManager = FindObjectOfType<InputManager>();
        scrollSpeed = 800;
    }

    void OnEnable()
    {
        foreach (BillboardSprites sprite in spritesConBillboard)
        {
            sprite.GetComponent<BillboardSprites>().enabled = false;
            sprite.transform.rotation = Quaternion.Euler(90, 0,0);
        }

        GameManager.inPause = true;
        GameManager.viewingMinimap = true;
    }

    private void Update()
    {
        Scroll();
        GoBack();
    }

    private void Scroll()
    {
        float input = inputManager.verticalInput;
        if (input != 0)
        {
            if (input > 0 && minimapa.anchoredPosition.y >= 15) scrollSpeed = 800;
            else if (input < 0 && minimapa.anchoredPosition.y <= 1065) scrollSpeed = 800;
            else scrollSpeed = 0;
                
                minimapa.localPosition += Vector3.down * input * Time.deltaTime * scrollSpeed;
                
        }
    }

    private void GoBack()
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
