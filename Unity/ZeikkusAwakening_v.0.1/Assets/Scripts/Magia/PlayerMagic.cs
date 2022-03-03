using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    public GameObject[] magics;
    public bool MagicAttackLookupTable(int selected)
    {
        if (!GameManager.Instance.inWorld)
        {
            Stats playerStats = GetComponent<Stats>();
            int mpCost = magics[selected].GetComponent<Magic>().mpCost;
            if (playerStats.mp <= 0 || playerStats.mp < mpCost) return false;
            playerStats.mp -= mpCost;
        }
        Instantiate(magics[selected]);
        return true; 
    }
}
