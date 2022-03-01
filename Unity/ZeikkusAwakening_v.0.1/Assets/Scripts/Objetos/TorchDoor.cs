using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchDoor : MonoBehaviour
{
    int triggerCount;
    private AudioSource source;
    public SpriteRenderer minimapSprite;
    public AudioClip abrirPuerta;

    private void Awake()
    {
        source = FindObjectOfType<GameManager>().GetComponent<AudioSource>();
    }
    void Start()
    {

    }
    public void doorTrigger()
    {
        if(triggerCount < 2)
        {
            triggerCount++;
        }
        else if(triggerCount >= 2)
        {
            StartCoroutine("LiftDoor");
        }
    }

    IEnumerator LiftDoor()
    {
        source.PlayOneShot(abrirPuerta);
        Debug.Log(transform.position);
        while (transform.localPosition.y < 2.3f)
        {
            transform.localPosition += Vector3.up * 2f * Time.deltaTime;
            Debug.Log("hi");
            yield return null;
        }
    }
}
