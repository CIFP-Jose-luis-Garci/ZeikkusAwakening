using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private GameObject player;
    private Light[] lights;
    private GameObject[] antorchas;

    private void Start()
    {
        player = InputManager.Instance.gameObject;
        lights = GetComponentsInChildren<Light>();
        antorchas = new GameObject[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            antorchas[i] = lights[i].transform.parent.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float minDist = 30;
        foreach (GameObject antorcha in antorchas)
        {
            float dist = Vector3.Distance(antorcha.transform.position, player.transform.position);
            antorcha.SetActive(dist < minDist);
        }
    }
}
