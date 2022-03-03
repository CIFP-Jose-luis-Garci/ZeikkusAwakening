using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashManager : MonoBehaviour
{
    private Text text;
    private Image background;
    private AudioSource source;
    public AudioClip enterBattleClip;
    public GameObject container, tutorialToSpawn;

    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        source = GameManager.Instance.source;
        GameManager.Instance.SpawnTutorial(container, tutorialToSpawn, null);
    }

    private void OnEnable()
    {
        background.CrossFadeAlpha(0, 0, true);
        text.CrossFadeAlpha(0, 0, true);
    }

    IEnumerator Animate()
    {
        source.PlayOneShot(enterBattleClip);
        background.CrossFadeAlpha(1,0.3f,true);
        yield return new WaitForSeconds(0.2f);
        text.CrossFadeAlpha(1, 0.2f, true);
        yield return new WaitForSeconds(0.8f);
        text.CrossFadeAlpha(0, 0.2f, true);
        yield return new WaitForSeconds(0.2f);
        background.CrossFadeAlpha(0,0.3f,true);
    }

    public void AnimateStart()
    {
        StartCoroutine(Animate());
    }
}
