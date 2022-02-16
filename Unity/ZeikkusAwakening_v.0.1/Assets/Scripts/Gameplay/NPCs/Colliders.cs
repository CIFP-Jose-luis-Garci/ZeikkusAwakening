using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    EnemyManager enemyManager;
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = transform.parent.gameObject.GetComponent<EnemyManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyManager.isWalking = false;
            enemyManager.detectado = true;
            enemyManager.player = other.transform;
        }
    }
}
