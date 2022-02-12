using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Item : MonoBehaviour
{
    [NonSerialized] public int slot;
    [FormerlySerializedAs("name")] public string itemName;
    public string category;
    public int type;
    public Sprite itemSprite;
    public string description;
    public bool usable;
    public bool tossable;

    public abstract bool UseItem();
}
