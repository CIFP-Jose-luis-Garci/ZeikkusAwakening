using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool inWorld;
    public GameObject pause;
    private InputManager inputManager;
    public int maru;

    private void Update()
    {
        inputManager = FindObjectOfType<InputManager>();
        if (inputManager.start)
        {
            pause.SetActive(!pause.activeSelf);
        }
    }
}
