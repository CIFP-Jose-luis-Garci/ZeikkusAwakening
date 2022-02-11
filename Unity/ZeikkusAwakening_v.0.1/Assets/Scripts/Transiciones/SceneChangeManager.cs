using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChangeManager : MonoBehaviour
{
    

    public Canvas HUD;
    

    void Start()
    {
        // Create a temporary reference to the current scene.
        //Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        //string sceneName = currentScene.name;

    }
    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player") && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("escenario_p2"))
        {
            GameObject.Find("HUD").GetComponent<Canvas>().transform.GetChild(4).gameObject.SetActive(true);
            
            Invoke("CargarNivel1", 2f);
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("escenario_p1"))
        {
            Invoke("CargarNivel2", 2f);
        }
    }
    
    
    private void CargarNivel1()
    {
        GameObject.Find("HUD").GetComponent<Canvas>().transform.GetChild(4).gameObject.SetActive(false);
        SceneManager.LoadScene(0); 

    }

    private void CargarNivel2()
    {
        GameObject.Find("HUD").GetComponent<Canvas>().transform.GetChild(4).gameObject.SetActive(false);
        SceneManager.LoadScene(1);

    }
}
