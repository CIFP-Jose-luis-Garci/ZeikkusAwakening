using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PantallaPausaManager : MonoBehaviour
{
    private InputManager inputManager;
    private GameManager gameManager;
    private Stats zeikkuStats;
    public GameObject pantallaInventario, pantallaEstado, pantallaOpciones;
    public Button inventario, estado, opciones;
    public Text tiempo, maru, zeikkuVida, zeikkuMagia;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        GameManager.inPause = true;
        inventario.Select();
        zeikkuStats = inputManager.gameObject.GetComponent<Stats>();
        zeikkuVida.text = zeikkuStats.hp + "/" + zeikkuStats.maxHP;
        zeikkuMagia.text = zeikkuStats.mp + "/" + zeikkuStats.maxMP;
        maru.text = gameManager.maru.ToString();
    }

    private void OnDisable()
    {
        GameManager.inPause = false;
    }

    private void Start()
    {
        inventario.onClick.AddListener(() =>
        {
            pantallaInventario.SetActive(true);
        });
        estado.onClick.AddListener(() =>
        {
            pantallaEstado.SetActive(true);
        });

        opciones.onClick.AddListener(() =>
        {
            pantallaOpciones.SetActive(true);
        });
        inventario.Select();

    }
}
