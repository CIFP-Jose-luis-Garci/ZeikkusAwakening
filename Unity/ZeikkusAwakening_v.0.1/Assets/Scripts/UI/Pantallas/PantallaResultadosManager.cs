using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaResultadosManager : MonoBehaviour
{
    public Text exp, maru, danoTotal, tiempoBatalla;

    private EscenaBatallaManager escenaBatallaManager;
    private GameManager gameManager;
    public bool fade;
    public Image blackFade;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        escenaBatallaManager = FindObjectOfType<EscenaBatallaManager>();
        gameManager = FindObjectOfType<GameManager>();
        exp.text = GameManager.CalcExp(escenaBatallaManager.enemies);
        maru.text = GameManager.CalcMaru(escenaBatallaManager.enemies);
        danoTotal.text = escenaBatallaManager.danoTotal.ToString();
        tiempoBatalla.text = escenaBatallaManager.TiempoBatalla();
    }

    private void Update()
    {
        if (!fade) return;
        if (fade)
        {
            fade = false;
            StartCoroutine(FadeOutIn());
        }
    }

    private IEnumerator FadeOutIn()
    {
        gameObject.SetActive(false);
        // press a, goto transition fade in black
        blackFade.CrossFadeAlpha(1, 1, true);
        HUDManager hudManager = FindObjectOfType<Canvas>().GetComponent<HUDManager>();
        yield return GameManager.CrossFadeMusic(hudManager.mixer, 1, true);
        AudioSource musicSource = hudManager.GetComponent<AudioSource>();
        musicSource.Stop();
        musicSource.clip = gameManager.worldMusic;
        musicSource.Play();
        yield return GameManager.CrossFadeMusic(hudManager.mixer, 1, false);
        escenaBatallaManager.gameObject.SetActive(false);
        escenaBatallaManager.ResetPlayer();
        // fade out black
        blackFade.CrossFadeAlpha(0, 1, true);
    }
}
