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
        pos.z -= 0.8f;
        Instantiate(magics[selected], pos, transform.rotation);
    }
}
