using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineFreeLook cmfl;
    private void Awake()
    {
        cmfl = GetComponent<CinemachineFreeLook>();
        Transform player = FindObjectOfType<PlayerManager>().transform;
        cmfl.m_Follow = player;
        cmfl.m_LookAt = player;
    }

    public void ChangeTarget(Transform newTarget)
    {
        cmfl.m_Follow = newTarget;
        cmfl.m_LookAt = newTarget;
    }
}
