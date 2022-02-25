using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Cutscene1FreekLookTurn : MonoBehaviour
{
    private CinemachineFreeLook freeLook;

    private void Start()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        freeLook.m_XAxis.Value += 10 * Time.deltaTime;
    }
}
