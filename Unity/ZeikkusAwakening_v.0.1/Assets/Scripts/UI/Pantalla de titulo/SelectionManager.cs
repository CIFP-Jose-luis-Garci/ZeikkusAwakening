using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public Button newFile, continuee, options, exit;
    public GameObject opciones, pantallaInicial;
    public Image blackBackground, loading;
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        newFile.onClick.AddListener(() =>
        {
            StartCoroutine(StartGame());
        });
        continuee.interactable = false;
        options.onClick.AddListener(() =>
        {
            opciones.SetActive(true);
            opciones.GetComponentInChildren<Slider>().Select();
            pantallaInicial.SetActive(false);
            gameObject.SetActive(false);
        });
        exit.onClick.AddListener(() => Application.Quit(0));
    }
    
    IEnumerator StartGame()
    {
        blackBackground.CrossFadeAlpha(1, 1, true);
        yield return GameManager.CrossFadeMusic(mixer, 2, true);
        loading.gameObject.SetActive(true);
        yield return GameManager.LoadScene(2);
    }
}
