using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estadística
{
    public int oldValue;
    public int newValue;
    public int difference;

    public Estadística(int oldValue, int difference)
    {
        this.oldValue = oldValue;
        this.difference = difference;
        newValue = oldValue + difference;
    }

    public int GetNewValue()
    {
        return newValue;
    }
}
