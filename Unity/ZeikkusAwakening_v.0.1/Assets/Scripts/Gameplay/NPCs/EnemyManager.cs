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

    public bool detectado;
    public bool animatorCambiado = false;

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
            animator.SetBool("moving", true);
            rondaGoal = SetRandomGoal(transform.position, 5);
            agente.SetDestination(rondaGoal);
            yield return new WaitForSeconds(5f);
        }
    }

    public void PararMovimiento()
    {
        agente.speed = 4;
        StopCoroutine("IA");
    }

    public void IniciarMovimiento()
    {
        agente.speed = 1;
        StartCoroutine("IA");
    }

    // Update is called once per frame
    void Update()
    {
        if(animatorCambiado == false)
        {
            if (detectado == true)
            {
                animator.SetBool("detection", true);
                transform.LookAt(player.position);
                Quaternion rotation = transform.rotation;
                rotation.z = 0;
                rotation.x = 0;
                transform.rotation = rotation;
                agente.SetDestination(player.position);
            }
            else
            {
                animator.SetBool("detection", false);
            }
        }
        else
        {
            agente.SetDestination(player.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            agente.SetDestination(transform.position);
            agente.speed = 0;
            if(animatorCambiado == false)
            {
                animator.SetTrigger("caught");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Espada"))
        {
            if(animatorCambiado == false)
            {
                CambiarAnimator();
            }
            else
            {
                //Bajar vida
                agente.speed = -3;
                animator.SetTrigger("da�o");
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("alcance", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            agente.speed = 4;
            agente.SetDestination(player.position);
            if(animatorCambiado == false)
            {
                animator.SetBool("moving", true);
            }
            else
            {
                animator.SetBool("alcance", true);
            }
        }
    }
    public void ImTarget(bool set)
    {
        sprite.SetActive(set);
    }

    public void CambiarAnimator()
    {
        animatorCambiado = true;
        //Cambiar animator controller
    }
}
