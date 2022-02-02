using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineFreeLook cmfl;
    private InputManager inputManager;
    private float originalRadius;
    private InBetweenObjectManager ibom;
    private GameManager gameManager;
    private void Awake()
    {
        cmfl = GetComponent<CinemachineFreeLook>();
        ibom = FindObjectOfType<InBetweenObjectManager>();
        gameManager = FindObjectOfType<GameManager>();
        Transform player = FindObjectOfType<PlayerManager>().transform;
        cmfl.m_Follow = player;
        cmfl.m_LookAt = player;
        inputManager = player.gameObject.GetComponent<InputManager>();
        originalRadius = cmfl.m_Orbits[1].m_Radius;
    }

    private void LateUpdate()
    {
        if (inputManager.lTrigger && !gameManager.inWorld)
        {
            if (!ibom.enemy) return;
            if (ibom.enemy.gameObject.CompareTag("Enemigo"))
            {
                cmfl.m_Orbits[1].m_Radius = ibom.distance;
            } else
            {
                cmfl.m_Orbits[1].m_Radius = originalRadius;
            }
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        cmfl.m_Follow = newTarget;
        cmfl.m_LookAt = newTarget;
    }
}
