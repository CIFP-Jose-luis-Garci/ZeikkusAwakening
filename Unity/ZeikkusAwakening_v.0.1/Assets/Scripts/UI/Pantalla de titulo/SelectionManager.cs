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
    private AudioSource source;
    public AudioMixer mixer;
    public AudioClip sonidoSeleccionar;

    private bool started;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        source = transform.root.GetComponent<AudioSource>();
        zeikku = GetComponentInParent<TitleScreenManager>().zeikkuInstatiated;
        newFile.onClick.AddListener(() =>
        {
            if (!started)
            {
                started = true;
                StartCoroutine(StartGame());
                source.PlayOneShot(sonidoSeleccionar);
            }
        });
        continuee.interactable = false;
        options.onClick.AddListener(() =>
        {
            zeikku.SetActive(false);
            opciones.SetActive(true);
            opciones.GetComponentInChildren<Slider>().Select();
            logo.SetActive(false);
            source.PlayOneShot(sonidoSeleccionar);
            gameObject.SetActive(false);
        });
        exit.onClick.AddListener(() =>
        {
            source.PlayOneShot(sonidoSeleccionar);
            Application.Quit(0);
        });
    }
    
    IEnumerator StartGame()
    {
        zeikku.SetActive(false);
        background.CrossFadeAlpha(0, 1, true);
        logo.GetComponent<Image>().CrossFadeAlpha(0, 1, true);
        animator.SetBool("start", true);
        yield return GameManager.Instance.CrossFadeMusic(mixer, 2, true);
        loading.gameObject.SetActive(true);
        yield return GameManager.Instance.LoadScene(2);
    }
}
