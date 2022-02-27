using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotEnemigoManager : MonoBehaviour
{
    [SerializeField] private Text nombre, nivel;

    public void SetNameAndLevel(EnemyStats enemyStats)
    {
        if (!enemyStats)
        {
            gameObject.SetActive(false);
            return;
        }
        nombre.text = enemyStats.actorName;
        nivel.text = enemyStats.level.ToString();
    }
}
