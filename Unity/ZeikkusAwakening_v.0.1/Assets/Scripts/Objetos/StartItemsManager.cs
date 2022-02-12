using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartItemsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Item[] items = GetComponentsInChildren<Item>();
        FindObjectOfType<PlayerBagManager>().AddItem(items[0]);
        FindObjectOfType<PlayerBagManager>().AddItem(items[1]);
        FindObjectOfType<PlayerBagManager>().AddItem(items[2]);
        FindObjectOfType<PlayerBagManager>().AddItem(items[3]);
    }
}
