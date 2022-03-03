using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : EnemyManager
{
    private bool animated;
    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>().transform;
    }

    private void Update()
    {
        // override method to block movement
        
        
        return;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.transitioning = false;
            HUDManager.Instance.StartBattle(gameObject, true, 1);
        }
    }
}
