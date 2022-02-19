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

    public void AddExp(int exp)
    {
        experience += exp;
        if (experience > nextLevelExperience)
        {
            level++;
            experience = 0;
            nextLevelExperience = (int) (nextLevelExperience * 1.2f);
            maxHP = (int) (maxHP * 1.2f);
            maxMP = (int) (maxMP * 1.2f);
            hp = maxHP;
            mp = maxMP;
            strength += Random.Range(1, 5);
            defense += Random.Range(1, 3);
            magicPower += Random.Range(1, 4);
            resistance += Random.Range(1, 3);
            strength += Random.Range(1, 3);
        }
    }
}
