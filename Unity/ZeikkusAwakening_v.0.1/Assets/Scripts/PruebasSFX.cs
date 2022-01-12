using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebasSFX : MonoBehaviour
{

    [SerializeField] AudioClip[] efectos;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //audioSource.clip = efectos[1];
        PlaySound(2);
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaySound(int key)
    {
        audioSource.PlayOneShot(efectos[0]);
    }

    
}
