using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    public string name { get; set; }
    public Image itemSprite;
    public string description { get; set; }

    public abstract void UseItem(GameObject player);
}
