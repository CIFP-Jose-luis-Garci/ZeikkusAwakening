using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    public float secondsToDestruction;

    public void Start()
    {
        Destroy(gameObject, secondsToDestruction);
    }
}
