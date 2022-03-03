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
    public AudioClip sonidoNo, sonidoPopUp, sonidoCerrarPopUp;
    private AudioSource source;
    private Item selectedItem;

    private InputManager inputManager;

    void Start()
    {
        inputManager = InputManager.Instance;
        source = GameManager.Instance.source;
        source.PlayOneShot(sonidoPopUp);
        usar.onClick.AddListener(() =>
        {
            if (selectedItem && selectedItem.usable)
            {
                if (selectedItem.UseItem())
                {
                    TossItem();
                    Exit();
                }
                else
                {
                    source.PlayOneShot(sonidoNo);
                }
            }
            else
            {
                source.PlayOneShot(sonidoNo);
            }
        });
        tirar.onClick.AddListener(() =>
        {
            if (selectedItem && selectedItem.tossable)
            {
                TossItem();
                Exit();
            }
            else
            {
                source.PlayOneShot(sonidoNo);
            }
        });

        salir.onClick.AddListener(() =>
        {
            source.PlayOneShot(sonidoCerrarPopUp);
            Exit();
        });
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
        GameManager.Instance.bag.RemoveItem(selectedItem.slot);
    }

    // Update is called once per frame
    void Update()
    {
        inputManager.horizontalInput = 0;
        if (inputManager.bInput)
        {
            inputManager.bInput = false;
            source.PlayOneShot(sonidoCerrarPopUp);
            Exit();
        }
    }

    public void SetItem(Item item)
    {
        selectedItem = item;
    }
}
