using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartItemsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Item[] items = GetComponentsInChildren<Item>();
        PlayerBagManager pbm = FindObjectOfType<PlayerBagManager>();
        foreach (Item item in items)
        {
            pbm.AddItem(item);
        }
    }
}
