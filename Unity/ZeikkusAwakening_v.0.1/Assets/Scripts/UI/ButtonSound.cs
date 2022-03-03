using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    public AudioClip sonidoSeleccionado;
    private static AudioSource gameManagerAudio;

    private void Awake()
    {
        if (!gameManagerAudio)
            gameManagerAudio = GameManager.Instance.source;
    }

    public void OnSelect(BaseEventData eventData)
    {
        gameManagerAudio.PlayOneShot(sonidoSeleccionado);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        
    }
}
