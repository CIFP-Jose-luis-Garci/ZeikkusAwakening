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
        values[1].PutValues(hp, 10 + ( level / 10 * ( (100 * 2) ) ));
        maxHP = values[1].newValue;
        values[2].PutValues(mp, 10 + ( level / 10 * ( (80 * 2) ) ));
        maxMP = values[2].newValue;
        values[3].PutValues(strength, Random.Range(2, 6));
        values[4].PutValues(defense, Random.Range(2, 4));
        values[5].PutValues(magicPower, Random.Range(2, 5));
        values[6].PutValues(resistance, Random.Range(2, 4));
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
