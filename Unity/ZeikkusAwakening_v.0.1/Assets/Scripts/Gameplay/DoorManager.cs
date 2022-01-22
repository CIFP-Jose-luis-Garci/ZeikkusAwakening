using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool hasKey;
    private bool doorOpen;
    public Transform player;
    
    void Awake()
    {
        
    }
    void Start()
    {
        hasKey = false;
        doorOpen = false;
    }

    void Update()
    {
        keyCheck();
    }

    //si el jugador tiene la llave, ejecuta liftDoor. si no, detecta si el jugador se acerca con la llave
    void keyCheck()
    {
        if(doorOpen)
        {
            liftDoor();
        }
        else if( hasKey || playerContact(5f) ) //de momento usa || en vez de && para testeo, cambiar después
        {
            doorOpen = true;
        }
    }

    //levanta la puerta una vez por ejecución hasta un límite
    void liftDoor()
    {
        if(transform.position.y < 2.3f)
        {
            transform.position += Vector3.up * 0.1f ;
            print("habrir puerta");
        }
    }

    //función que se usa para detectar el contacto con el jugador en el DoorManager y el KeyManager, no probada
    public bool playerContact(float detectRange)
    {
        if(Vector3.Distance(player.position, transform.position) <= detectRange )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
