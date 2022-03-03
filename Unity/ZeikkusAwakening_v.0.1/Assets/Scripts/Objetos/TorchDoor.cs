using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class TorchDoor : MonoBehaviour
{
    private static int triggerCount;
    private AudioSource source;
    private VisualEffect fuego;
    private Light luz;
    private SpriteRenderer minimapSprite;
    private CinemachineFreeLook playerCmfl;
    private CinemachineBrain brain;
    public CinemachineVirtualCamera doorVcam;
    public GameObject door;
    public AudioClip abrirPuerta;

    private void Awake()
    {
        fuego = GetComponentInChildren<VisualEffect>();
        luz = GetComponentInChildren<Light>();
        brain = FindObjectOfType<CinemachineBrain>();
        playerCmfl = FindObjectOfType<CinemachineFreeLook>();
        source = GameManager.Instance.source;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (fuego.enabled) return;
        if (other.gameObject.CompareTag("Zagrant"))
        {
            if (!other.GetComponent<ZagrantController>().onFire) return;
            fuego.enabled = true;
            luz.enabled = true;
            Debug.Log(triggerCount);
            if (triggerCount < 2)
            {
                triggerCount++;
            }
            else
            {
                brain.m_DefaultBlend.m_Time = 0.5f;
                playerCmfl.gameObject.SetActive(false);
                doorVcam.gameObject.SetActive(true);
                GameManager.Instance.inPause = true;
                GameManager.Instance.transitioning = true;
                StartCoroutine(LiftDoor());
                Invoke(nameof(DisableCamera), 2f);
            }
        }
    }

    private void DisableCamera()
    {
        playerCmfl.gameObject.SetActive(true);
        doorVcam.gameObject.SetActive(false);
        brain.m_DefaultBlend.m_Time = 2f;
        GameManager.Instance.inPause = false;
        GameManager.Instance.transitioning = false;
    }

    IEnumerator LiftDoor()
    {
        source.PlayOneShot(abrirPuerta);
        while (door.transform.localPosition.y < 2.3f)
        {
            door.transform.localPosition += Vector3.up * 2f * Time.deltaTime;
            yield return null;
        }
    }
}
