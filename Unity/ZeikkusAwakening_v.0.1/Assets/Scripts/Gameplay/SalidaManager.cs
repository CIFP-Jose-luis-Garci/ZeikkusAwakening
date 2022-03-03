using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalidaManager : MonoBehaviour
{
    public AudioClip sonidoPortal;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.source.PlayOneShot(sonidoPortal);
        HUDManager.Instance.FinishLevel();
    }
}
