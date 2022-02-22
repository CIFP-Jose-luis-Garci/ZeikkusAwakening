using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemPopupManager : MonoBehaviour
{

    public Button usar, tirar, salir;
    [NonSerialized] public GameObject botonesBolsa, botonesItem;
    private Item selectedItem;

    private InputManager inputManager;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        usar.onClick.AddListener(() =>
        {
            if (selectedItem.usable)
            {
                if (selectedItem.UseItem())
                {
                    TossItem();
                    Exit();
                }
                else
                {
                    // TODO: play sound
                }
            }
        });
        tirar.onClick.AddListener(() =>
        {
            if (selectedItem.tossable)
            {
                TossItem();
                Exit();
            }
        });

        salir.onClick.AddListener(() => Exit());
    }

    private void Exit()
    {
        botonesBolsa.SetActive(true);
        botonesItem.SetActive(false);
        ListaItemsManager lista = FindObjectOfType<ListaItemsManager>();
        lista.ReloadList();
        lista.itemSelected = false;
        Destroy(gameObject);
    }

    private void TossItem()
    {
        FindObjectOfType<PlayerBagManager>().RemoveItem(selectedItem.slot);
    }

    // Update is called once per frame
    void Update()
    {
        inputManager.horizontalInput = 0;
        if (inputManager.bInput)
        {
            inputManager.bInput = false;
            Exit();
        }
    }

    public void SetItem(Item item)
    {
        selectedItem = item;
    }
}
