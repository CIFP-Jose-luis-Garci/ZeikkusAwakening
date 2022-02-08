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

    public Item[] GetBagContents()
    {
        object[] objects = bag.ToArray();
        Item[] items = Array.ConvertAll(objects, new Converter<object, Item>(ObjectToItem));
        return items;
    }

    private Item ObjectToItem(object input)
    {
        return input as Item;
    }
}
