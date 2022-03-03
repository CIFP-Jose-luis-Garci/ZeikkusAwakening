using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MemorySceneManager : MonoBehaviour
{
    public Image flash, loading;
    public DialogueManager dialogo;

    public IEnumerator FadeOutFlash(float timeToLoad)
    {
        flash.CrossFadeAlpha(0, 2, true);
        // sonido de deasaparecer
        yield return new WaitForSeconds(3);
        loading.gameObject.SetActive(true);
        yield return GameManager.Instance.LoadScene(timeToLoad);
    }

    public void ShowDialogue()
    {
        dialogo.gameObject.SetActive(true);
    }
}
