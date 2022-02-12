using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private GameObject player;
    private Light[] lights;
    private GameObject[] antorchas;

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>().gameObject;
        lights = GetComponentsInChildren<Light>();
        antorchas = new GameObject[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            antorchas[i] = lights[i].gameObject.transform.parent.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float minDist = 30;
        foreach (GameObject putaAntorcha in antorchas)
        {
            float dist = Vector3.Distance(putaAntorcha.transform.position, player.transform.position);
            putaAntorcha.SetActive(dist < minDist);
        }
    }
}
