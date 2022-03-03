using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PantallaEstadoManager : MonoBehaviour
{
    private InputManager inputManager;
    private Stats stats;
    public GameObject pantallaPausa;
    public GameObject container, tutorialToSpawn;
    public Text nombre, titulo, nivel, vida, magia, experiencia, fuerza, defensa, poderMagico, resitencia, puntosDeTurno;
    public Image imagenPersonaje;
    private void Awake()
    {
        inputManager = InputManager.Instance;
        GameManager.Instance.SpawnTutorial(container, tutorialToSpawn, null);
    }
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        stats = inputManager.gameObject.GetComponent<Stats>();
        nombre.text = stats.actorName;
        titulo.text = stats.title;
        nivel.text = "Nv. " + stats.level;
        imagenPersonaje.sprite = stats.sprite; 
        vida.text = stats.hp + "/" + stats.maxHP;
        magia.text = stats.mp + "/" + stats.maxMP;
        experiencia.text = stats.experience + "/" + stats.nextLevelExperience;
        fuerza.text = stats.strength.ToString();
        defensa.text = stats.defense.ToString();
        poderMagico.text = stats.magicPower.ToString();
        resitencia.text = stats.resistance.ToString();
        puntosDeTurno.text = stats.turnPoints.ToString();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GoBack();
    }

    private void GoBack()
    {
        inputManager.GoBack(gameObject, pantallaPausa);
    }
}
