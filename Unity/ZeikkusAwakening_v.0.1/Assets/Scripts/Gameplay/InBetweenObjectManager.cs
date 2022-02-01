using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBetweenObjectManager : MonoBehaviour
{
    Transform player;
    public Transform enemy;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckNearestEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = enemy.position + (player.position - enemy.position) / 2;
    }

    IEnumerator CheckNearestEnemy()
    {
        while (true)
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (GameObject t in GameObject.FindGameObjectsWithTag("Enemigo"))
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t.transform;
                    minDist = dist;
                }
            }
            enemy = tMin;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            yield return new WaitForSeconds(1f);
        }
    }
}
