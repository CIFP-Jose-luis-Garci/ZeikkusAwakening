using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCutSceneScript : MonoBehaviour
{
    public GameObject cameraChanger;
    private void OnTriggerEnter(Collider other)
    {
        cameraChanger.SetActive(true);
        cameraChanger.GetComponent<Cutscene2Manager>().player = other.transform;
        Destroy(gameObject);
    }
}
