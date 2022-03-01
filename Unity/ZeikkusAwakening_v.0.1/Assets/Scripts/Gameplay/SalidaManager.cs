using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalidaManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<HUDManager>().FinishLevel();
    }
}
