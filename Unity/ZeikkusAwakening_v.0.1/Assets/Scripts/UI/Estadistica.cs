using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estadistica
{
    public int oldValue { get; set; }
    public int newValue { get; set; }
    public int difference { get; set; }

    public Estadistica()
    {
        
    }

    public int PutValues(int oldValue, int difference)
    {
        this.oldValue = oldValue;
        this.difference = difference;
        newValue = oldValue + difference;
        return newValue;
    }
}
