using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float secondsToDestruction;

    public void Destruir()
    {
        Destroy(gameObject, secondsToDestruction);
    }
}
