using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    private DoorManager doorManager;
    public bool testGetKey = false;

    void Awake()
    {
        doorManager = GameObject.Find("p1_puerta").GetComponent<DoorManager>();
    }

    void Start()
    {
        
    }
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            doorManager.hasKey = true;
            print("llave conseguida");
        }
    }

}
