using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SceneChangeManager : MonoBehaviour
{
    public GameObject nivel1, nivel2;
    public VideoPlayer video;
    public Image blackFade;
    private GameObject player;

    private void Start()
    {
        player = FindObjectOfType<PlayerManager>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        blackFade.CrossFadeAlpha(1, 0.3f, true);
        yield return new WaitForSeconds(0.3f);
        video.gameObject.SetActive(true);
        Debug.Log("Hola");
        yield return new WaitForSeconds(2f);
        if (nivel1.activeSelf){
            nivel2.SetActive(true);
            nivel1.SetActive(false);
            player.transform.position = new Vector3(7.62f, 22.438f, -48.07f);
        } else if (nivel2.activeSelf)
        {
            nivel2.SetActive(false);
            nivel1.SetActive(true);
            player.transform.position = new Vector3(2.75f, 0, -43.5f);
        }
        GameManager.checkpoint = player.transform.position;
        video.gameObject.SetActive(false);
        blackFade.CrossFadeAlpha(0, 1, true);
    }
}
