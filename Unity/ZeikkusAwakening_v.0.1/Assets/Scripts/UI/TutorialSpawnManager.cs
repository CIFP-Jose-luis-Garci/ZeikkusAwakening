using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSpawnManager : MonoBehaviour
{
    public GameObject container, tutorialToSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.SpawnTutorial(container, tutorialToSpawn, gameObject);
        }
    }
}
