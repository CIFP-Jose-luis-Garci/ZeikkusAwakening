using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSpawnManager : MonoBehaviour
{
    public GameObject container, tutorialToSpawn;
    [SerializeField] private Sprite imageSprite;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Text contained = container.GetComponentInChildren<Text>();
            if (contained)
                Destroy(contained.transform.parent.gameObject);
            Instantiate(tutorialToSpawn, container.transform);
            Destroy(gameObject);
        }
    }
}
