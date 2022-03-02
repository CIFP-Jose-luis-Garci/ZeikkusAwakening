using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocionMagica : Item
{
    public float healingPercent;
    public AudioClip sonidoCuracion;
    private AudioSource source;

    private bool someoneIsHealed;

    private void Start()
    {
        source = FindObjectOfType<GameManager>().GetComponent<AudioSource>();
    }

    public override bool UseItem()
    {
        GameObject[] party = source.GetComponent<GameManager>().personajes;
        foreach (GameObject partyMember in party)
        {
            if (partyMember == null) continue;
            Stats currentStats = partyMember.GetComponent<Stats>();
            float toHeal = currentStats.maxMP * healingPercent;
            if (currentStats.mp != currentStats.maxMP)
                someoneIsHealed = true;
            currentStats.mp += (int) toHeal;
            if (currentStats.mp > currentStats.maxMP)
                currentStats.mp = currentStats.maxMP;
        }

        FindObjectOfType<PantallaPausaManager>().UpdateValues();
        FindObjectOfType<MagiaSliderManager>().UpdateSliderValues();

        if (someoneIsHealed)
            source.PlayOneShot(sonidoCuracion);
        
        return someoneIsHealed;
    }
}
