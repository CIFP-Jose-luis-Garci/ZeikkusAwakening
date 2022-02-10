using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListaItemsManager : MonoBehaviour
{
    public GameObject pantallaPausa;

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
    public Text categoryText;
    private int currentCategory;
    private bool pageChanged;

    public Text itemName, itemCategory, itemDescription;
    public Image itemSprite;
    private void Awake()
    {
        bag = FindObjectOfType<PlayerBagManager>();
        currentPage = 1;
        inputManager = FindObjectOfType<InputManager>();
    }

    void OnEnable()
    {
        inputManager.inPause = true;
        Debug.Log("hola");
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
        ChangeItemInfo();
        GoBack();
    }

    private void ChangeItemInfo()
    {
        ItemSlotManager currentObject = EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlotManager>();
        if (currentObject == null) return;
        Item current = currentObject.item;
        if (current == null) return;

        itemName.text = current.name;
        itemCategory.text = current.category;
        itemDescription.text = current.description;
        itemSprite.sprite = current.itemSprite;
    }

    private void GoBack()
    {
        if (inputManager.bInput)
        {
            gameObject.SetActive(false);
            pantallaPausa.SetActive(true);
        }
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
            _ChangeCategory();
            inputManager.rBump = false;
        }
        else if (inputManager.lBump)
        {
            currentCategory--;
            if (currentCategory < 0)
            {
                currentCategory = categories.Length - 1;
            }
            _ChangeCategory();
            inputManager.lBump = false;
        }
    }

    private void _ChangeCategory()
    {
        selectedCategory.rectTransform.position = categories[currentCategory].rectTransform.position;
        ReloadList();
        string category = "";
        switch (currentCategory)
        {
            case 0:
                category = "Todo";
                break;
            case 1:
                category = "Armas";
                break;
            case 2:
                category = "Armaduras";
                break;
            case 3:
                category = "Pantalones";
                break;
            case 4:
                category = "Pociones";
                break;
            case 5:
                category = "Llaves";
                break;
            case 6:
                category = "Tesoros";
                break;
            default:
                break;
        }
        categoryText.text = "Inventario: " + category;
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
            items[i - startPoint].GetComponent<ItemSlotManager>().item = currentItems[i];
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
