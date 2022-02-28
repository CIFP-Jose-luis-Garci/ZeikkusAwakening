using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    public GameObject[] magics;
    public void MagicAttackLookupTable(int selected)
    {
        if (!FindObjectOfType<GameManager>().inWorld)
            GetComponent<Stats>().mp -= magics[selected].GetComponent<Magic>().mpCost;
        Instantiate(magics[selected], transform);
    }
}
