using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    public GameObject[] magics;
    public void MagicAttackLookupTable(int selected)
    {
        Vector3 pos = transform.position;
        pos.y += 1;
        Vector3 zPos = (-0.6f * transform.forward);
        pos.x -= zPos.x;
        pos.z -= zPos.z;
        GetComponent<Stats>().mp -= magics[selected].GetComponent<Magic>().mpCost;
        Instantiate(magics[selected], pos, transform.rotation, transform);
    }
}
