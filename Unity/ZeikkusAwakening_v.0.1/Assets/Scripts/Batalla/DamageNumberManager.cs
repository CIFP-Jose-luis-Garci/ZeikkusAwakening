using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    private TextMesh texto;
    private Color color;

    private float time;
    // Start is called before the first frame update
    void Start()
    {
        texto = GetComponent<TextMesh>();
        color = new Color(texto.color.r, texto.color.g, texto.color.b, 0);
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
