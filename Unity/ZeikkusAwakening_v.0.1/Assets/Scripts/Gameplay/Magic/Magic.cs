using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public GameObject[] magics;
    public void MagicAttackLookupTable(int selected)
    {
        Vector3 pos = transform.position;
        pos.y += 1;
        Vector3 xzPos = (-0.8f * transform.forward);
        pos.z -= xzPos.z;
        pos.x -= xzPos.x;
        Instantiate(magics[selected], pos, transform.rotation);
    }
}
