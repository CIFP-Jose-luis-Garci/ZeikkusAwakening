using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListaItemsManager : MonoBehaviour
{
    public GameObject[] items;
    public Text paginas;
    private PlayerBagManager bag;
    private Item[] currentItems;
    private int currentPage;
    private InputManager inputManager;
    private void Start()
    {
        bag = FindObjectOfType<PlayerBagManager>();
        currentPage = 1;
        inputManager = FindObjectOfType<InputManager>();
    }

    void OnEnable()
    {
        GetItems();
        SetPages();
    }

    private void Update()
    {
        
    }

    private void GetItems()
    {
        currentItems = bag.GetBagContents();
        for (int i = 0; i < currentItems.Length; i++)
        {
            if (i > 9) break;
            string name = currentItems[i].name;
            Text itemName = items[i].GetComponentInChildren<Text>();
            itemName.text = name;
        }
        items[0].GetComponent<Button>().Select();
    }

    private void SetPages()
    {
        if (currentItems.Length < 10)
            paginas.text = "1/1";
        else
        {
            float count = Mathf.Floor((float) currentItems.Length / 10) + 1;
            paginas.text = currentPage + "/" + count;
        }
    }
}
