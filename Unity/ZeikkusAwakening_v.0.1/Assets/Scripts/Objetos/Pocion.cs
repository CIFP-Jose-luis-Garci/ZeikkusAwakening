using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocion : Item
{
    public float healingPercent;

    private bool someoneIsHealed;
    public override bool UseItem()
    {
        // play sound
        GameObject[] party = FindObjectOfType<GameManager>().personajes;
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

        FindObjectOfType<PantallaPausaManager>().UpdateValues();
        FindObjectOfType<VidaSliderManager>().UpdateSliderValues();
        
        return someoneIsHealed;
    }
}
