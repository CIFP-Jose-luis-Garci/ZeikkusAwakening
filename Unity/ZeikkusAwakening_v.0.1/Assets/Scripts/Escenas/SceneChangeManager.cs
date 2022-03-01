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
        GameManager.transitioning = true;
        blackFade.CrossFadeAlpha(1, 0.3f, true);
        yield return new WaitForSeconds(0.3f);
        video.gameObject.SetActive(true);
        if (nivel1.activeSelf)
        {
            yield return ChangeLevels(nivel2,new Vector3(7.62f, 22.438f, -48.07f));
            nivel1.SetActive(false);
            GameManager.dungeonLevel = 1;
        } else if (nivel2.activeSelf)
        {
            yield return ChangeLevels(nivel1,new Vector3(2.75f, 0, -43.5f));
            nivel2.SetActive(false);
            GameManager.dungeonLevel = 0;
        }
        GameManager.checkpoint = player.transform.position;
        video.gameObject.SetActive(false);
        blackFade.CrossFadeAlpha(0, 1, true);
        GameManager.transitioning = false;
    }

    private IEnumerator ChangeLevels(GameObject next, Vector3 position)
    {
        next.SetActive(true);
        player.transform.position = position;
        yield return new WaitForSeconds(2f);
    }
}
