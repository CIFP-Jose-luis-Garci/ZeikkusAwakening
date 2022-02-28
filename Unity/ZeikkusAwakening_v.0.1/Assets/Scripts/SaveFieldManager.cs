using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFieldManager : MonoBehaviour
{
    private HUDManager hudManager;

    private void Start()
    {
        hudManager = FindObjectOfType<HUDManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.checkpoint = transform.position;
        hudManager.ShowCheckpointArrived();
    }
}