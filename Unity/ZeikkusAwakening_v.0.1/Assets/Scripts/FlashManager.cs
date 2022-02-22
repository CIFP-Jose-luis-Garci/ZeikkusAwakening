using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashManager : MonoBehaviour
{
    private Text text;
    private Image background;
    void Start()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        background.CrossFadeAlpha(0,0,true);
        text.CrossFadeAlpha(0, 0, true);
        StartCoroutine(Animate());
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
}
