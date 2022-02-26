using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubesDeNivelManager : MonoBehaviour
{
    private Animator animator;
    private Estadistica[] estadisticas;
    public Text[] textos;
    public Text[] nuevosValores;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        for (int i = 0; i < estadisticas.Length; i++)
        {
            textos[i].text = estadisticas[i].oldValue.ToString();;
        }
    }

    private void PopUpValuesEvent()
    {
        for (int i = 0; i < nuevosValores.Length; i++)
        {
            Estadistica estadistica = estadisticas[i];
            nuevosValores[i].text = "+" + estadistica.difference;
            textos[i].text = estadistica.newValue.ToString();
        }
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
