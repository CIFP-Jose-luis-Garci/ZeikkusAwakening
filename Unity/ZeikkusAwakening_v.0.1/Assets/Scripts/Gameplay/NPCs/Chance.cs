using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chance : IComparable<Chance>
{
    public string name;
    public float chance;

    public Chance(string name)
    {
        this.name = name;
        chance = 0;
    }

    public int CompareTo(Chance other)
    {
        return chance.CompareTo(other.chance);
    }
}
