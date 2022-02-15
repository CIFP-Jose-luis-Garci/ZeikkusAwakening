using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaEstadoManager : MonoBehaviour
{
    private InputManager inputManager;
    private Stats stats;
    public GameObject pantallaPausa;
    public Text nombre, titulo, nivel, vida, magia, experiencia, fuerza, defensa, poderMagico, resitencia, puntosDeTurno;
    public Image imagenPersonaje;
    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }
    private void OnEnable()
    {
        inputManager.inPause = true;
        stats = inputManager.gameObject.GetComponent<Stats>();
        nombre.text = stats.actorName;
        titulo.text = stats.title;
        nivel.text = "Nv. " + stats.level;
        imagenPersonaje.sprite = stats.sprite; 
        vida.text = stats.hp + "/" + stats.maxHP;
        magia.text = stats.mp + "/" + stats.maxMP;
        experiencia.text = stats.experience + "/" + stats.nextLevelExperience;
        fuerza.text = stats.strength.ToString();
        poderMagico.text = stats.defense.ToString();
        resitencia.text = stats.magicPower.ToString();
        puntosDeTurno.text = stats.turnPoints.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        GoBack();
    }

    private void GoBack()
    {
        if (inputManager.bInput)
        {
            gameObject.SetActive(false);
            pantallaPausa.SetActive(true);
        }
    }
}
