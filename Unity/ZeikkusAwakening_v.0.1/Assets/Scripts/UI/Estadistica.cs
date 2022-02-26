using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estadistica
{
    public int oldValue;
    public int newValue;
    public int difference;

    public Estadistica(int oldValue, int difference)
    {
        this.oldValue = oldValue;
        this.difference = difference;
        newValue = oldValue + difference;
    }

    public int GetNewValue()
    {
        return newValue;
    }

    public int GetOldValue()
    {
        return oldValue;
    }

    public int GetDifference()
    {
        return difference;
    }
}
