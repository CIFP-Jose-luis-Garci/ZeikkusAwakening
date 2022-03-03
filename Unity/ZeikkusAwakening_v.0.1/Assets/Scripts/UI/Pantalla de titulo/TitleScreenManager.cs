using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public Image logobg, logo, titlebg, titlelogo, titlepress;
    public GameObject selecciones, zeikkuPrefab, zeikkuInstatiated;

    private AudioSource musicaTitulo;
    public AudioClip clipLogo;

    private void Start()
    {
        logobg.CrossFadeAlpha(0, 0, true);
        logo.CrossFadeAlpha(0, 0, true);
        titlebg.CrossFadeAlpha(0, 0, true);
        titlelogo.CrossFadeAlpha(0, 0, true);
        StartCoroutine(AnimateTitleScreen());
        musicaTitulo = GetComponent<AudioSource>();
    }

    IEnumerator AnimateTitleScreen()
    {
        logobg.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(1f);
        logo.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(1f);
        musicaTitulo.PlayOneShot(clipLogo);
        yield return new WaitForSeconds(1f);
        logo.CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1f);
        musicaTitulo.Play();
        logobg.CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1f);
        titlebg.CrossFadeAlpha(1, 1, true);
        titlelogo.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(2f);
        titlepress.GetComponent<Animator>().enabled = true;
        Button pressAnyButton = titlepress.GetComponent<Button>();
        pressAnyButton.onClick.AddListener(() =>
        {
            selecciones.SetActive(true);
            zeikkuInstatiated = Instantiate(zeikkuPrefab);
            selecciones.GetComponentInChildren<Button>().Select();
            Destroy(pressAnyButton.gameObject);
        });
        pressAnyButton.Select();
    }
}
