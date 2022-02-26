using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubesDeNivelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retract()
    {
        animator.SetBool("retract", true);
        Destroy(gameObject, 2f);
    }
}
