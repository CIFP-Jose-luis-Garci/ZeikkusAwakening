using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Magic
{
    // Start is called before the first frame update
    void Start()
    {
        mpCost = 10;
        damage = 30;
        StartCoroutine(SpawnAndLaunch());
    }

    IEnumerator SpawnAndLaunch()
    {
        yield return new WaitForSeconds(1);
    }
}
