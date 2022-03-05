using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFieldManager : MonoBehaviour
{
    private static bool showedTutorial;
    public GameObject container, tutorialCheckpoint;

    

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.checkpoint = transform.position;
        if (!showedTutorial)
        {
            showedTutorial = true;
            GameManager.Instance.SpawnTutorial(container, tutorialCheckpoint, null);
        }
        HUDManager.Instance.ShowCheckpointArrived();
    }
}