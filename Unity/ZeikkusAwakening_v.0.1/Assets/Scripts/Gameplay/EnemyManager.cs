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

    public bool noVeo;
    public bool detectado;

    public Vector3 direccion;
    Vector3 rondaGoal;

    private void Start()
    {
        noVeo = true;
        detectado = false;
        animator = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();
        StartCoroutine("IA");
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
        while (noVeo)
        {
            rondaGoal = SetRandomGoal(transform.position, 5);
            agente.SetDestination(rondaGoal);
            animator.SetBool("moving", true);
            yield return new WaitForSeconds(Random.Range(1,4));
        }
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
        }
    }

    public void ImTarget(bool set)
    {
        sprite.SetActive(set);
    }
}
