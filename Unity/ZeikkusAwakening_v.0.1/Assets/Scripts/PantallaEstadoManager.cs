using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaEstadoManager : MonoBehaviour
{
    private InputManager inputManager;
    private Stats stats;
    public Text vida, magia, experiencia, fuerza, defensa, poderMagico, resitencia, puntosDeTurno;
    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }
    private void OnEnable()
    {
        inputManager.inPause = true;
        stats = inputManager.gameObject.GetComponent<Stats>();
        vida.text = stats.hp + "/" + stats.maxHP;
        magia.text = stats.mp + "/" + stats.maxMP;
        experiencia.text = stats.experience + "/" + stats.nextLevelExperience;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
