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
    private InputManager inputManager;

    private int currentPage;
    private int maxPages;
    private int nextPage;
    private int startPoint;

    public Image[] categories;
    public Image selectedCategory;
    private int currentCategory;
    private bool pageChanged;
    private void Awake()
    {
        bag = FindObjectOfType<PlayerBagManager>();
        currentPage = 1;
        inputManager = FindObjectOfType<InputManager>();
    }

    void OnEnable()
    {
        inputManager.inPause = true;
        ReloadList();
    }

    private void OnDisable()
    {

        inputManager.inPause = false;
    }

    private void Update()
    {
        ChangePage();
        ChangeCategory();
    }

    private void ChangeCategory()
    {

        if (inputManager.rBump)
        {
            currentCategory++;
            if (currentCategory >= categories.Length)
            {
                currentCategory = 0;
            }
            selectedCategory.rectTransform.position = categories[currentCategory].rectTransform.position;
            ReloadList();
            inputManager.rBump = false;
        }
        else if (inputManager.lBump)
        {
            currentCategory--;
            if (currentCategory < 0)
            {
                currentCategory = categories.Length - 1;
            }
            selectedCategory.rectTransform.position = categories[currentCategory].rectTransform.position;
            ReloadList();
            inputManager.lBump = false;
        }
    }

    private void ChangePage()
    {
        float x = inputManager.horizontalInput;
        if (x != 0)
        {
            if (pageChanged) return;
            if (x > 0)
            {
                currentPage++;
                if (currentPage > maxPages)
                    currentPage = 1;
                ReloadList();
            }
            else if (x < 0)
            {
                currentPage--;
                if (currentPage < 1)
                    currentPage = maxPages;
                ReloadList();
            }
            pageChanged = true;
        } else
        {
            pageChanged = false;
        }

    }

    private void ReloadList()
    {
        ClearList();
        GetItems();
        SetPages();
    }

    private void ClearList()
    {
        foreach (GameObject item in items)
        {
            item.GetComponentInChildren<Text>().text = "";
        }
    }

    private void GetItems()
    {
        nextPage = currentPage;
        nextPage *= 10;
        startPoint = nextPage - 10;
        currentItems = bag.GetBagContents(currentCategory);
        for (int i = startPoint; i < nextPage; i++)
        {
            if (i >= currentItems.Length) break;
            string name = currentItems[i].name;
            Text itemName = items[i - startPoint].GetComponentInChildren<Text>();
            itemName.text = name;
        }
        items[0].GetComponent<Button>().Select();
    }

    private void SetPages()
    {
        if (currentItems.Length < 10)
        {
            paginas.text = "1/1";
            maxPages = 1;
        }
        else
        {
            int count = Mathf.FloorToInt((float) currentItems.Length / 10) + 1;
            paginas.text = currentPage + "/" + count;
            maxPages = count;
        }
    }
}
