using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressAnyButtonController : MonoBehaviour
{

    public GameObject selecciones;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject seleccionesInstanciadas = Instantiate(selecciones, GameObject.FindGameObjectWithTag("UI").transform);
            seleccionesInstanciadas.GetComponentInChildren<Button>().Select();
            Destroy(gameObject);
        });
    }
}
