using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditoManager : MonoBehaviour
{
    [SerializeField] private float speed = 100;
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
    }
}
