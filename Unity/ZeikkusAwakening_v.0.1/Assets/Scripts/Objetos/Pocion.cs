using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocion : Item
{
    public float healingPercent;
    public AudioClip sonidoCuracion;
    private AudioSource source;

    private bool someoneIsHealed;

    private void Start()
    {
        source = GameManager.Instance.source;
    }

    public override bool UseItem()
    {
        GameObject[] party = source.GetComponent<GameManager>().personajes;
        foreach (GameObject partyMember in party)
        {
            if (partyMember == null) continue;
            Stats currentStats = partyMember.GetComponent<Stats>();
            float toHeal = currentStats.maxHP * healingPercent;
            if (currentStats.hp != currentStats.maxHP)
                someoneIsHealed = true;
            currentStats.hp += (int) toHeal;
            if (currentStats.hp > currentStats.maxHP)
                currentStats.hp = currentStats.maxHP;
        }

        GameManager.Instance.pause.UpdateValues();
        HUDManager.Instance.pantallaPausa.sliderVida.UpdateSliderValues();

        if (someoneIsHealed)
            source.PlayOneShot(sonidoCuracion);
        
        return someoneIsHealed;
    }
}
