using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : EnemyManager
{
    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.transitioning = false;
            FindObjectOfType<HUDManager>().StartBattle(gameObject, true, 1);
        }
    }
}
