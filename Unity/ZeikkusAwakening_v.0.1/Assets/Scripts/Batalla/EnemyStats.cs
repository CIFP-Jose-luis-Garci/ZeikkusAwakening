using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyStats : Stats
{
    public int expBase;
    public int marubase;

    public void SetLevel(int level)
    {
        if (level < 1)
        {
            level = 1;
        }

        Debug.Log(level);
        int levelAdded = 1;
        Estadistica[] values = new Estadistica[7];
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = new Estadistica();
        }
        while (levelAdded < level)
        {
            CalcValues(values);
            levelAdded++;
        }
        if (level > 1)
            AddValues(values);
    }
}
