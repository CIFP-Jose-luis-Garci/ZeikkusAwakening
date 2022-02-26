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
        oldValue = 0;
        difference = 0;
        newValue = 0;
    }

    public void PutValues(int oldValue, int difference)
    {
        this.oldValue = oldValue;
        this.difference += difference;
        newValue = oldValue + difference;
    }
}
