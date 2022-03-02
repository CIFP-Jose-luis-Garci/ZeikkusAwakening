using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chance : IComparer<Chance>
{
    public string name;
    public float chance;

    public Chance(string name)
    {
        this.name = name;
        chance = 0;
    }

    public int Compare(Chance x, Chance y)
    {
        if (x.chance > y.chance)
            return 1;
        else if (x.chance < y.chance)
            return -1;
        else 
            return 0;
    }
}
