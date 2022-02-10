using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool inWorld;
    public GameObject pause;
    public int maru;
    public GameObject[] personajes;

    private InputManager inputManager;

    private void Update()
    {
        inputManager = FindObjectOfType<InputManager>();
        if (inputManager.start)
        {
            pause.SetActive(!pause.activeSelf);
        }
    }
}
