using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string name;
    public string category;
    public int type;
    public Sprite itemSprite;
    public string description;

    public abstract void UseItem(GameObject player);
}
