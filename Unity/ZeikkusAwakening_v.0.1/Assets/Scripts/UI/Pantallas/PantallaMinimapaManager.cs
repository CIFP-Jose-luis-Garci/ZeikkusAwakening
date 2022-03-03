using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantallaMinimapaManager : MonoBehaviour
{
    private BillboardSprites[] spritesConBillboard;
    private InputManager inputManager;
    private GameManager gameManager;
    private Animator animator;
    private PlayerLocomotion playerLocomotion;
    private float scrollSpeed;
    private int currentDungeonlevel = -1;
    public RectTransform minimapa;
    public GameObject[] minimapZones;
    public AudioClip sonidoMapa;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        playerLocomotion = inputManager.GetComponent<PlayerLocomotion>();
        scrollSpeed = 800;
    }

    void OnEnable()
    {
        animator.SetBool("retract", false);
        if (currentDungeonlevel != GameManager.dungeonLevel)
        {
            spritesConBillboard = FindObjectsOfType<BillboardSprites>();
            currentDungeonlevel = GameManager.dungeonLevel;
        }
        foreach (BillboardSprites sprite in spritesConBillboard)
        {
            sprite.GetComponent<BillboardSprites>().enabled = false;
            sprite.transform.rotation = Quaternion.Euler(90, -180,0);
        }

        minimapZones ? [GameManager.dungeonLevel].SetActive(true);

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
            playerLocomotion.GravitySet(true);
            inputManager.select = false;
            gameManager.source.PlayOneShot(sonidoMapa);
            StartCoroutine(Retract());
        }
    }

    private IEnumerator Retract()
    {
        animator.SetBool("retract", true);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        
        foreach (BillboardSprites sprite in spritesConBillboard)
        {
            sprite.GetComponent<BillboardSprites>().enabled = true;
        }
        
        minimapZones ? [GameManager.dungeonLevel].SetActive(false);
        GameManager.inPause = false;
        GameManager.viewingMinimap = false;
    }
}
