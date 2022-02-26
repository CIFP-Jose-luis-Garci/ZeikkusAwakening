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
            experience -= nextLevelExperience;
            experience
            Estadistica[] values = new Estadistica[7];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = new Estadistica();
            }
            level = values[0].PutValues(level, 1);
            experience = 0;
            nextLevelExperience = (int) (nextLevelExperience * 1.2f);
            
            hp = values[1].PutValues(hp, (int) (maxHP * 1.2f));
            maxHP = values[1].newValue;
            mp = values[2].PutValues(mp, (int) (maxMP * 1.2f));
            maxMP = values[2].newValue;
            strength = values[3].PutValues(strength, Random.Range(2, 6));
            defense = values[4].PutValues(defense, Random.Range(2, 4));
            magicPower = values[5].PutValues(magicPower, Random.Range(2, 5));
            resistance = values[6].PutValues(resistance, Random.Range(2, 4));
            
            return values;
        }
        return null;
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
