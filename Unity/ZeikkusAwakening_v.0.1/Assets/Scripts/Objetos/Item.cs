using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string name;
    public Sprite itemSprite;
    public string description;

    public abstract void UseItem(GameObject player);
}
