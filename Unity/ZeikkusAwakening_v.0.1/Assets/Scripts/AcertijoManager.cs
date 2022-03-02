using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcertijoManager : MonoBehaviour
{
    public GameObject container, popupAcertijo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.SpawnTutorial(container, popupAcertijo, null);
        }
    }
}
