using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfazBatallaManager : MonoBehaviour
{
    private Animator animator;

    public SlotEnemigoManager enemigo1, enemigo2, enemigo3;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
