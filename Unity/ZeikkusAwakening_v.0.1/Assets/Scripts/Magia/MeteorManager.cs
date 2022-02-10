using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{
    public GameObject fire, meteorExplosion, smoke;
    public float speed;
    public bool forward;

    private int mpCost, damage;
    private AnimatorManager animatorManager;
    // Start is called before the first frame update
    void Start()
    {
        animatorManager = GameObject.FindObjectOfType<AnimatorManager>();
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
        if (other.gameObject.CompareTag("Enemigo"))
        {

            Stats enemyStats = other.gameObject.GetComponent<Stats>();
            Stats zeikkuStats = FindObjectOfType<PlayerLocomotion>().gameObject.GetComponent<Stats>();
            float resultado = 0.2f * 2;
            resultado += 1;
            resultado *= zeikkuStats.magicPower;
            resultado *= animatorManager.animator.GetFloat("damage");
            resultado /= (25 * enemyStats.resistance);
            resultado += 2;
            float random = Random.Range(85, 100);
            resultado *= random;
            resultado *= 0.01f;
            resultado *= 5;
            enemyStats.hp -= (int)resultado;
            if (enemyStats.hp < 0)
            {
                Destroy(other.gameObject);

            }

            // animacion
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
}
