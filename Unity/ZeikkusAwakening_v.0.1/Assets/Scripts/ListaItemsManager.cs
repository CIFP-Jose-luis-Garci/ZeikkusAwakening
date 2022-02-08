using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListaItemsManager : MonoBehaviour
{
    public GameObject[] items;
    private PlayerBagManager bag;
    private Item[] currentItems;
    
    void OnEnable()
    {
        bag = FindObjectOfType<PlayerBagManager>();

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
