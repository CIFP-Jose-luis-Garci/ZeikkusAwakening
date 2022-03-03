using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfazBatallaManager : MonoBehaviour
{
    private Animator animator;
    private PlayerLocomotion playerLocomotion;
    private GameObject[] magics;
    public SlotEnemigoManager[] slotsEnemigos;
    public Text magiaA, magiaX, magiaY;

    private void Awake()
    {
        playerLocomotion = InputManager.Instance.GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        magics = playerLocomotion.GetComponent<PlayerMagic>().magics;
        magiaA.text = magics[playerLocomotion.magicSlots[0]].GetComponent<Magic>().magicName;
        magiaY.text = magics[playerLocomotion.magicSlots[1]].GetComponent<Magic>().magicName;
        magiaX.text = magics[playerLocomotion.magicSlots[2]].GetComponent<Magic>().magicName;
    }

    public void ActivateSlots()
    {
        foreach (SlotEnemigoManager slotEnemigo in slotsEnemigos)
        {
            slotEnemigo.gameObject.SetActive(true);
            slotEnemigo.animator.enabled = false;
        }
    }

    public void Retract()
    {
        StartCoroutine(RetractAnimate());
    }

    private IEnumerator RetractAnimate()
    {
        animator.SetBool("retract", true);
        yield return new WaitForSeconds(0.5f);
        animator.CrossFade("InterfazBatalla", 0);
        animator.SetBool("retract", false);
        gameObject.SetActive(false);
    }
}
