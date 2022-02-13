using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{

    public Button newFile, continuee, options, exit;

    public GameObject opciones;
    // Start is called before the first frame update
    void Start()
    {
        newFile.onClick.AddListener(() =>
        {
            StartCoroutine(LoadYourAsyncScene());
        });
        continuee.interactable = false;
        options.onClick.AddListener(() =>
        {
            GameObject opcionesInstanciadas = Instantiate(opciones, GameObject.FindGameObjectWithTag("UI").transform);
            opcionesInstanciadas.GetComponentInChildren<Slider>().Select();
            Destroy(GameObject.FindGameObjectWithTag("GameLogo"));
            Destroy(gameObject);
        });
        exit.onClick.AddListener(() => Application.Quit(0));
    }
    
    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
