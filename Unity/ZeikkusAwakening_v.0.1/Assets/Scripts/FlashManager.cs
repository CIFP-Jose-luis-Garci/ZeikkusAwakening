using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashManager : MonoBehaviour
{
    private Text text;
    private Image background;
    public GameObject container, tutorialToSpawn;

    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        GameManager.SpawnTutorial(container, tutorialToSpawn, null);
    }

    private void OnEnable()
    {
        background.CrossFadeAlpha(0, 0, true);
        text.CrossFadeAlpha(0, 0, true);
    }

    IEnumerator Animate()
    {
        background.CrossFadeAlpha(1,0.5f,true);
        yield return new WaitForSeconds(0.2f);
        text.CrossFadeAlpha(1, 0.3f, true);
        yield return new WaitForSeconds(1f);
        text.CrossFadeAlpha(0, 0.3f, true);
        yield return new WaitForSeconds(0.2f);
        background.CrossFadeAlpha(0,0.5f,true);
    }

    public void AnimateStart()
    {
        StartCoroutine(Animate());
    }
}
