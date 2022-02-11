using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public Transform player;
    public GameObject sprite;

    Animator animator;
    NavMeshAgent agente;


    public float velNPC = 10;

    public bool detectado;
    public Coroutine corrutina;

    public Vector3 direccion;
    Vector3 rondaGoal;

    private void Start()
    {
        detectado = false;
        animator = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();
        IniciarMovimiento();
    }
    Vector3 SetRandomGoal(Vector3 npcPos, float randomValue)
    {
        float newX = npcPos.x + Random.Range(-randomValue, randomValue);
        float newZ = npcPos.z + Random.Range(-randomValue, randomValue);
        direccion = new Vector3(newX, 0, newZ);
        return direccion;
    }

    IEnumerator IA()
    {
        while (!detectado)
        {
            rondaGoal = SetRandomGoal(transform.position, 5);
            agente.SetDestination(rondaGoal);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void PararMovimiento()
    {
        StopCoroutine(corrutina);
    }

    public void IniciarMovimiento()
    {
        corrutina = StartCoroutine(IA());
    }


    // Update is called once per frame
    void Update()
    {
        if (detectado)
        {
            transform.LookAt(player);
            Quaternion rotation = transform.rotation;
            rotation.z = 0;
            rotation.x = 0;
            transform.rotation = rotation;
        } else
        {
            if (Vector3.Distance(player.transform.position, transform.position) > 2)
                agente.SetDestination(player.position);
            else
                agente.SetDestination(transform.position);
        }

        if (agente.remainingDistance <= (detectado ? 2 : 0.1f))
        {
            animator.SetBool("moving", false);
        }
        else
        {
            animator.SetBool("moving", true);
        }
    }

    public void ImTarget(bool set)
    {
        sprite.SetActive(set);
    }
}
