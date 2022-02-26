using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Stats : MonoBehaviour
{
    [FormerlySerializedAs("name")] public string actorName;
    public string title;
    public Sprite sprite;
    public int level;
    public int hp;
    public int maxHP;
    public int mp;
    public int maxMP;
    public int strength;
    public int defense;
    public int magicPower;
    public int resistance;
    public int experience;
    public int nextLevelExperience;
    public int turnPoints;
    public int expBase;
    public int marubase;
    public bool alive;

    private void Awake()
    {
        maxHP = hp;
        maxMP = mp;
    }

    public Estadistica[] AddExp(int exp)
    {
        experience += exp;
        if (experience > nextLevelExperience)
        {
            
            Estadistica[] values = new Estadistica[7];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = new Estadistica();
            }

            do
            {
                CalcAndAdd(values);
            } while (experience > nextLevelExperience);

            level = values[0].newValue;
            hp = values[1].newValue;
            mp = values[2].newValue;
            strength = values[3].newValue;
            defense = values[4].newValue;
            magicPower = values[5].newValue;
            resistance = values[6].newValue;
            
            return values;
        }
        return null;
    }

    private void CalcAndAdd(Estadistica[] values)
    {
        experience -= nextLevelExperience;
        values[0].PutValues(level, 1);
        nextLevelExperience = (int) Mathf.Pow(values[0].newValue, 3);
        values[1].PutValues(hp, Mathf.FloorToInt(Mathf.Sqrt(100 * level)));
        maxHP = values[1].newValue;
        values[2].PutValues(mp, Mathf.FloorToInt(Mathf.Sqrt(80 * level)));
        maxMP = values[2].newValue;
        values[3].PutValues(strength, 10 + ( level / 100 * ( (120 * 2) ) ));
        values[4].PutValues(defense, 10 + ( level / 100 * ( (90 * 2) ) ));
        values[5].PutValues(magicPower, 10 + ( level / 100 * ( (100 * 2) ) ));
        values[6].PutValues(resistance, 10 + ( level / 100 * ( (95 * 2) ) ));
    }

    public void SetLevel(int level)
    {
        if (level < 1)
        {
            level = 1;
        }
        this.level = level;
    }
}
