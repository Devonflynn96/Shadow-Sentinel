using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;

    [SerializeField] float runSpeed;
    [SerializeField] float hearingRange;
    [SerializeField] float alertRange; // New field for alert range
    [SerializeField] LayerMask enemyLayerMask;

    bool isRunning;
    bool isDead;

    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
        StartCoroutine(Roam());
    }

    void Update()
    {
        if (isRunning)
        {
            float agentSpeed = agent.velocity.magnitude / runSpeed;
            anim.SetFloat("Speed", agentSpeed);
        }
    }

    IEnumerator Roam()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));

            if (!isDead)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 10f;
                randomDirection += startingPos;

                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);

                agent.SetDestination(hit.position);
            }
        }
    }

    public void OnHearSound(Vector3 soundPosition)
    {
        float distanceToSound = Vector3.Distance(transform.position, soundPosition);
        if (distanceToSound <= hearingRange)
        {
            if (!isRunning && !isDead)
            {
                agent.speed = runSpeed;
                agent.SetDestination(soundPosition);
                isRunning = true;
            }
        }
    }
    
    public void takeDamage(int amount)
    {
        isDead = true;
        StopAllCoroutines();
        StartCoroutine(FlashDamage());
        AlertEnemies();
        Destroy(gameObject);
    }

    IEnumerator FlashDamage()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = Color.white;
        }
    }



    void AlertEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, alertRange, enemyLayerMask);
        foreach (Collider collider in colliders)
        {
            GameObject enemy = collider.gameObject;
            if (enemy != null)
            {
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.OnHearSound(transform.position); // Notify enemies about the civilian's death
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, alertRange);
    }
}
