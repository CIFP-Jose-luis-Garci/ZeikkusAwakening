using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private void Awake()
    {
        CinemachineFreeLook cmfl = GetComponent<CinemachineFreeLook>();
        Transform player = FindObjectOfType<PlayerManager>().transform;
        cmfl.m_Follow = player;
        cmfl.m_LookAt = player;
    }
}
