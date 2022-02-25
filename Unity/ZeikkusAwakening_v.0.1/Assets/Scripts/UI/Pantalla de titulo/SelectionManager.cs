using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public Button newFile, continuee, options, exit;
    public GameObject opciones, logo;
    public Image background, loading;
    private Animator animator;
    private GameObject zeikku;
    public AudioMixer mixer;

    private bool started;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        zeikku = GetComponentInParent<TitleScreenManager>().zeikkuInstatiated;
        newFile.onClick.AddListener(() =>
        {
            if (!started)
            {
                started = true;
                StartCoroutine(StartGame());
            }
        });
        continuee.interactable = false;
        options.onClick.AddListener(() =>
        {
            zeikku.SetActive(false);
            opciones.SetActive(true);
            opciones.GetComponentInChildren<Slider>().Select();
            logo.SetActive(false);
            gameObject.SetActive(false);
        });
        exit.onClick.AddListener(() => Application.Quit(0));
    }
    
    IEnumerator StartGame()
    {
        zeikku.SetActive(false);
        background.CrossFadeAlpha(0, 1, true);
        logo.GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        animator.SetBool("start", true);
        yield return GameManager.CrossFadeMusic(mixer, 2, true);
        loading.gameObject.SetActive(true);
        yield return GameManager.LoadScene(2);
    }
}
