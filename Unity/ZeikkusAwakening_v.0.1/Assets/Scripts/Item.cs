using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private string name { get; set; }
    private string description { get; set; }

    public abstract void UseItem(GameObject player);
}
