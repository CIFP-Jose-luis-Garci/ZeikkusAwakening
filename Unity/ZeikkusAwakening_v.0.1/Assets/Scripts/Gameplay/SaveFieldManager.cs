using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFieldManager : MonoBehaviour
{
    private HUDManager hudManager;
    private bool showedTutorial;
    public GameObject container, tutorialCheckpoint;

    private void Start()
    {
        hudManager = FindObjectOfType<HUDManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.checkpoint = transform.position;
        if (!showedTutorial)
        {
            showedTutorial = true;
            GameManager.SpawnTutorial(container, tutorialCheckpoint, null);
        }
        hudManager.ShowCheckpointArrived();
    }
}