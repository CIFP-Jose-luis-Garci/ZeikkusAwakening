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
    
    void OnEnable()
    {
        bag = FindObjectOfType<PlayerBagManager>();
        GetItems();
    }

    private void GetItems()
    {
        currentItems = bag.GetBagContents();
        if (currentItems.Length < 10)
            paginas.text = "1/1";
        else
        {
            float count = Mathf.Floor((float) currentItems.Length / 10) + 1;
            paginas.text = "1/" + count;
        }
        for (int i = 0; i < currentItems.Length; i++)
        {
            if (i > 9) break;
            string name = currentItems[i].name;
            Text itemName = items[i].GetComponentInChildren<Text>();
            itemName.text = name;
        }
        items[0].GetComponent<Button>().Select();
    }
}
