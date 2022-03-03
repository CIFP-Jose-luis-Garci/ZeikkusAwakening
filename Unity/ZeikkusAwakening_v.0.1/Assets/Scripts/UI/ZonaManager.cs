using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HUDManager.Instance.ChangeZoneName(name);
    }
}
