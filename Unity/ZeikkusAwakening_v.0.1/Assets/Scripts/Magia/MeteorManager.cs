using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{
    public GameObject fire, meteorExplosion, smoke;
    public float speed;
    public bool forward;
    public GameObject damage;

    private int mpCost, strength;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = FindObjectOfType<AnimatorManager>().animator;
        mpCost = 10;
        strength = 30;
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
        if (other.gameObject.layer == 3) Animate();
        if (other.gameObject.CompareTag("Enemigo"))
        {
            EnemyBattleManager enemyBattleManager = other.gameObject.GetComponent<EnemyBattleManager>();
            Stats zeikkuStats = FindObjectOfType<PlayerLocomotion>().gameObject.GetComponent<Stats>();
            enemyBattleManager.RecieveDamage(zeikkuStats, animator.GetFloat("damage"), false);
            Animate();
        }
    }
    
    private void Animate(){
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
