using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaPausaManager : MonoBehaviour
{
    private InputManager inputManager;
    private Stats zeikkuStats;
    public GameObject pantallaInventario, pantallaEstado;
    public Button inventario, estado;
    public Text tiempo, maru, zeikkuVida, zeikkuMagia;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void OnEnable()
    {
        inputManager.inPause = true;
        inventario.Select();
        zeikkuStats = inputManager.gameObject.GetComponent<Stats>();
    }

    private void OnDisable()
    {
        inputManager.inPause = false;
    }

    private void Start()
    {
        zeikkuVida.text = zeikkuStats.hp + "/" + zeikkuStats.maxHP;
        zeikkuMagia.text = zeikkuStats.mp + "/" + zeikkuStats.maxMP;
        inventario.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            pantallaInventario.SetActive(true);
        });
        estado.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            pantallaEstado.SetActive(true);
        });
        inventario.Select();

    }
}
