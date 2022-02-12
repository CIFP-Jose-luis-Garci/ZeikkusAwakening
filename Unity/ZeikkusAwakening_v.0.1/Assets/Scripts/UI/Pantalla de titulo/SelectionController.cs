using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{

    public Button newFile, continuee, options, exit;

    public GameObject opciones;
    // Start is called before the first frame update
    void Start()
    {
        newFile.onClick.AddListener(() =>
        {
            //iniciar juego
        });
        continuee.interactable = false;
        options.onClick.AddListener(() =>
        {
            GameObject opcionesInstanciadas = Instantiate(opciones, GameObject.FindGameObjectWithTag("UI").transform);
            opcionesInstanciadas.GetComponentInChildren<Slider>().Select();
            Destroy(GameObject.FindGameObjectWithTag("GameLogo"));
            Destroy(gameObject);
        });
        exit.onClick.AddListener(() => Application.Quit(0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
