using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : Magic
{
    private AnimatorManager animatorManager;
    private GameManager gameManager;
    private Animator animator;
    private Transform player;
    public GameObject fire, meteorExplosion, smoke;
    public AudioClip cry, woosh;
    public float speed;
    public bool forward;
    private static readonly int Damage = Animator.StringToHash("damage");

    // Start is called before the first frame update
    void Start()
    {
        player = InputManager.Instance.transform;
        gameManager = GameManager.Instance;
        transform.rotation = player.rotation;
        Vector3 pos = player.position;
        pos.y += 1;
        Vector3 zPos = (-0.6f * player.forward);
        pos.x -= zPos.x;
        pos.z -= zPos.z;
        transform.position = pos;
        animatorManager = player.GetComponent<AnimatorManager>();
        if (gameManager.inWorld)
            animatorManager.PlayTargetAnimation("magicSelected fireball", true);
        else
            animatorManager.PlayTargetAnimation("magic fireball", true);
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
        if (other.gameObject.CompareTag("Enemigo") || other.gameObject.CompareTag("Boss"))
        {
            EnemyBattleManager enemyBattleManager = other.gameObject.GetComponent<EnemyBattleManager>();
            Stats zeikkuStats = player.GetComponent<Stats>();
            enemyBattleManager.RecieveDamage(zeikkuStats, animator.GetFloat(Damage), false);
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
