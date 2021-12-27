using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform player;
    public GameObject sprite;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        Quaternion rotation = transform.rotation;
        rotation.z = 0;
        rotation.x = 0;
        transform.rotation = rotation;
    }

    public void ImTarget(bool set)
    {
        sprite.SetActive(set);
    }
}
