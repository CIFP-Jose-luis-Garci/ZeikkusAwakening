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
        blackFade.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(1f);
        video.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        video.gameObject.SetActive(false);
        if (nivel1.activeSelf){
            nivel2.SetActive(true);
            nivel1.SetActive(false);
            player.transform.position = new Vector3(7.44f, 22.28f, -48.19f);
        } else if (nivel2.activeSelf)
        {
            nivel2.SetActive(false);
            nivel1.SetActive(true);
            player.transform.position = new Vector3(2.75f, 0, -43.5f);
        }
        blackFade.CrossFadeAlpha(0, 1, true);
    }
}
