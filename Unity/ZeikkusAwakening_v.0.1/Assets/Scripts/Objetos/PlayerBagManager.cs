using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBagManager : MonoBehaviour
{
    private ArrayList bag;
    // Start is called before the first frame update
    void Start()
    {
        bag = new ArrayList();
    }

    public void AddItem(Item item)
    {
        bag.Add(item);
    }

    public Item GetItem(int slot)
    {
        bag.Remove(slot);
        return bag[slot] as Item;
    }

    public Item[] GetBagContents(int category)
    {
        object[] objects = bag.ToArray();
        Item[] items = Array.ConvertAll(objects, new Converter<object, Item>(ObjectToItem));
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
            Item[] filteredItems = Array.ConvertAll(filteredList.ToArray(), new Converter<object, Item>(ObjectToItem));
            return filteredItems;
        }
        return items;
    }

    private Item ObjectToItem(object input)
    {
        return input as Item;
    }
}
