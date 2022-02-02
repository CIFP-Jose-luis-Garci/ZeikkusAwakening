using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{
    public GameObject fire, meteorExplosion, smoke;
    public float speed;
    public bool forward;
    private int mpCost, damage;
    // Start is called before the first frame update
    void Start()
    {
        mpCost = 10;
        damage = 30;
        fire.SetActive(false);
        meteorExplosion.SetActive(false);
        smoke.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (forward)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void FireAndSmokeEvent()
    {
        fire.SetActive(true);
        smoke.SetActive(true);
        forward = true;
        GetComponent<Animator>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
            meteorExplosion.SetActive(true);
            foreach (DestroyAfter i in GetComponentsInChildren<DestroyAfter>())
            {
                i.Destruir();
            }
            meteorExplosion.transform.parent = null;
            fire.transform.parent = null;
            smoke.transform.parent = null;
            Destroy(gameObject);
    }
}
