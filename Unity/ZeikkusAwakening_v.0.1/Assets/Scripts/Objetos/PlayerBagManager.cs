using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBagManager : MonoBehaviour
{
    private ArrayList bag;
    // Start is called before the first frame update
    void Awake()
    {
        bag = new ArrayList();
    }

    public void AddItem(Item item)
    {
        bag.Add(item);
    }

    public void RemoveItem(int slot)
    {
        bag.RemoveAt(slot);
    }

    //añadido por Ignacio
    public int ItemSlot(Item item)
    {
        return bag.IndexOf(item);
    }

    public Item[] GetBagContents(int category)
    {
        object[] objects = bag.ToArray();
        Item[] items = Array.ConvertAll(objects, ObjectToItem);
        if (category != 0)
        {
            ArrayList filteredList = new ArrayList();
            foreach (Item item in items)
            {
                if (item.type == category)
                {
                    filteredList.Add(item);
                }
            }
            Item[] filteredItems = Array.ConvertAll(filteredList.ToArray(), ObjectToItem);
            return filteredItems;
        }
        return items;
    }

    private Item ObjectToItem(object input)
    {
        return input as Item;
    }
}
