using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaPausaManager : MonoBehaviour
{
    private InputManager inputManager;
    private GameManager gameManager;
    private Stats zeikkuStats;
    public GameObject pantallaInventario, pantallaEstado;
    public Button inventario, estado;
    public Text tiempo, maru, zeikkuVida, zeikkuMagia;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        inputManager.inPause = true;
        inventario.Select();
        zeikkuStats = inputManager.gameObject.GetComponent<Stats>();
        zeikkuVida.text = zeikkuStats.hp + "/" + zeikkuStats.maxHP;
        zeikkuMagia.text = zeikkuStats.mp + "/" + zeikkuStats.maxMP;
        maru.text = gameManager.maru.ToString();
    }

    private void OnDisable()
    {
        inputManager.inPause = false;
    }

    private void Start()
    {
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
