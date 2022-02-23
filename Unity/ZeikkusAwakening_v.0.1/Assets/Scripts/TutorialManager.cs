using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private float time;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 20) 
            Destroy(gameObject);
    }
}
