using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditosManager : MonoBehaviour
{
    public Titulo[] creditos;
    public GameObject container, titulo, subtitulo;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MostrarCreditos());
    }

    private IEnumerator MostrarCreditos()
    {
        for (int i = 0; i < creditos.Length; i++)
        {
            Titulo credito = Instantiate(creditos[i].gameObject).GetComponent<Titulo>();
            GameObject tituloInstance = Instantiate(titulo, container.transform);
            tituloInstance.GetComponent<Text>().text = credito.titulo;
            Destroy(tituloInstance, 20f);
            yield return new WaitForSeconds(2);
            for (int j = 0; j < credito.subtitulos.Length; j++)
            {
                GameObject subtituloInstance = Instantiate(subtitulo, container.transform);
                subtituloInstance.GetComponent<Text>().text = credito.subtitulos[j];
                Destroy(subtituloInstance, 20f);
                yield return new WaitForSeconds(1.25f);
            }
            Destroy(credito.gameObject);
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(17);
        Application.Quit(0);
    }
}
