using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubesDeNivelManager : MonoBehaviour
{
    private Animator animator;
    private Estadistica[] estadisticas;
    public Text[] textos;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        for (int i = 0; i < estadisticas.Length; i++)
        {
            textos[i].text = estadisticas[i].GetOldValue().ToString();;
        }
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

    public void SetEstadísticas(Estadistica[] estadisticas)
    {
        this.estadisticas = estadisticas;
    }
}
