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
            values[0] = new Estadistica(level, 1);
            level = values[0].GetNewValue();
            experience = 0;
            nextLevelExperience = (int) (nextLevelExperience * 1.2f);
            values[1] = new Estadistica(hp, (int) (maxHP * 1.2f));
            values[2] = new Estadistica(mp, (int) (maxMP * 1.2f));
            values[3] = new Estadistica(strength, Random.Range(2, 6));
            values[4] = new Estadistica(defense, Random.Range(2, 4));
            values[5] = new Estadistica(magicPower, Random.Range(2, 5));
            values[6] = new Estadistica(resistance, Random.Range(2, 4));
            hp = values[1].GetNewValue();
            maxHP = values[1].GetNewValue();
            mp = values[2].GetNewValue();
            maxMP = values[2].GetNewValue();
            strength = values[3].GetNewValue();
            defense = values[4].GetNewValue();
            magicPower = values[5].GetNewValue();
            resistance = values[6].GetNewValue();
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
