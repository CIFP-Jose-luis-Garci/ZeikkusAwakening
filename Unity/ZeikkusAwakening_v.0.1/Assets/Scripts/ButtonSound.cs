using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IDeselectHandler
{
    public AudioClip sonidoSeleccionado;
    private AudioSource gameManagerAudio;

    private void Start()
    {
        gameManagerAudio = FindObjectOfType<GameManager>().GetComponent<AudioSource>();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameManagerAudio.clip = sonidoSeleccionado;
        gameManagerAudio.Play();
    }
}
