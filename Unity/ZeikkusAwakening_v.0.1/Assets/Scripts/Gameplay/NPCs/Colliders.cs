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
            enemyManager.detectado = true;
            enemyManager.PararMovimiento();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyManager.detectado = false;
            enemyManager.IniciarMovimiento();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
