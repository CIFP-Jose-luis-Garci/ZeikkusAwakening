using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeteorManager : Magic
{
    private AnimatorManager animatorManager;
    private GameManager gameManager;
    private Animator animator;
    private Transform user;
    public GameObject fire, meteorExplosion, smoke;
    public AudioClip cry, woosh;
    private float speed = 15;
    private bool forward;
    private static readonly int Damage = Animator.StringToHash("damage");

    // Start is called before the first frame update
    void Start()
    {
        user = FindObjectOfType<EnemyBattleManager>().transform;
        transform.rotation = user.rotation;
        Vector3 pos = user.position;
        pos.y += 1;
        Vector3 zPos = (-0.6f * user.forward);
        pos.x -= zPos.x;
        pos.z -= zPos.z;
        transform.position = pos;
        animatorManager = user.GetComponent<AnimatorManager>();
        gameManager = FindObjectOfType<GameManager>();
        animator = animatorManager.animator;
        fire.SetActive(false);
        meteorExplosion.SetActive(false);
        smoke.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (forward)
        {
            transform.parent = null;
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void MagicSound()
    {
        gameManager.source.PlayOneShot(cry);
        gameManager.source.PlayOneShot(woosh);
    }
    
    public void FireAndSmokeEvent()
    {
        fire.SetActive(true);
        smoke.SetActive(true);
        forward = true;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<Animator>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!forward) return;
        if (other.gameObject.layer == 3) Animate();
        if (other.gameObject.CompareTag("Player"))
        {
            BossBattleManager bossBattleManager = user.gameObject.GetComponent<BossBattleManager>();
            PlayerLocomotion playerLocomotion = other.gameObject.GetComponent<PlayerLocomotion>();
            playerLocomotion.RecieveDamage(bossBattleManager.GetComponent<Stats>(), animator.GetFloat(Damage), false);
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
