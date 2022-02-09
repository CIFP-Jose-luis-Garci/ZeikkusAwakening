using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaPausaManager : MonoBehaviour
{
    private InputManager inputManager;
    public GameObject pantallaInventario, pantallaEstado;
    public Button inventario, estado;
    public Text tiempo, maru, zeikkuVida, zeikkuMagia;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void OnEnable()
    {
        inventario.Select();
    }

    private void Start()
    {
        inputManager.inPause = true;
        inventario.onClick.AddListener(() =>
        {
            pantallaInventario.SetActive(true);
            gameObject.SetActive(false);
        });
        estado.onClick.AddListener(() =>
        {
            pantallaEstado.SetActive(true);
            gameObject.SetActive(false);
        });
        inventario.Select();

    }
}
