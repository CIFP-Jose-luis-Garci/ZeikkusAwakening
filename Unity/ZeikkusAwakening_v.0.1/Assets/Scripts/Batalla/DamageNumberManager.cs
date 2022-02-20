using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    public TextMesh texto;
    private Color color;

    private float time;
    // Start is called before the first frame update
    void Start()
    {
        texto = GetComponent<TextMesh>();
        color = new Color(255, 255, 255, 0);
        Destroy(gameObject,2f);
    }

    private void Update()
    {
        texto.color = Color.Lerp(texto.color, color, time);
        transform.Translate(Vector3.up * Time.deltaTime);
        time += Time.deltaTime / 10;
        transform.LookAt(Camera.main.transform);
    }
}
