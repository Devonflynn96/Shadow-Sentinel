using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] int animTranSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float hearingRange;
    [SerializeField] float alertRange; // New field for alert range
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] int roamDist;
    [SerializeField] float minWaitTime = 5f;  // Minimum wait time in seconds
    [SerializeField] float maxWaitTime = 10f;
    [SerializeField] float fleeDistance = 20f;

    //bool isRunning;
    //bool isDead;
    bool destChosen;
    

    Vector3 startingPos;
    float stoppingDistOrig;


    void OnEnable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnPlayerShoot += OnPlayerShoot;
        }
        else
        {
            Debug.LogError("GameManager instance is null. Make sure the GameManager is properly initialized.");
        }
    }

    void OnDisable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnPlayerShoot -= OnPlayerShoot;
        }
    }

    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        StartCoroutine(Roam());
    }

    void Update()
    {
       
        
            float agentSpeed = agent.velocity.normalized.magnitude;

            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentSpeed, Time.deltaTime * animTranSpeed));

            StartCoroutine(Roam());
    }

    IEnumerator Roam()
    {
        if (destChosen || agent == null || !agent.isOnNavMesh || agent.remainingDistance >= 0.05f) yield break;
        {


            destChosen = true;
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            agent.stoppingDistance = 0;

            Vector3 ranPos = Random.insideUnitSphere * roamDist;
            ranPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destChosen = false;
            
        }
    }
        void OnPlayerShoot(Vector3 shootPosition)
    {
        float distanceToShoot = Vector3.Distance(transform.position, shootPosition);
        if (distanceToShoot <= hearingRange)
        {
            AlertEnemies();
            Debug.Log($"{gameObject.name} heard a sound at  shootPosition: {shootPosition} and is fleeing.");
            agent.stoppingDistance = stoppingDistOrig;
            // agent.SetDestination(shootPosition);
            FleeFromSound(shootPosition);

        }
    }
    public void OnHearSound(Vector3 soundPosition)
    {
        float distanceToSound = Vector3.Distance(transform.position, soundPosition);
        if (distanceToSound <= hearingRange)
        {
            Debug.Log($"{gameObject.name} heard a sound at soundPosition: {soundPosition} and is fleeing.");
            // React to the sound (e.g., move towards it, become alert, etc.)
            agent.stoppingDistance = 0;
            // agent.SetDestination(soundPosition);
            FleeFromSound(soundPosition);
           
        }
    }

    public void takeDamage(int amount)
    {
        //isDead = true;
        StopAllCoroutines();
        StartCoroutine(FlashDamage());
        AlertEnemies();
        if (this.CompareTag("Target"))
        {
            GameManager.instance.gameGoalUpdate(-1);
        }
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



    public void AlertEnemies()
    {
        Debug.Log($"{gameObject.name} is alerting enemies within {alertRange} units.");
        Collider[] colliders = Physics.OverlapSphere(transform.position, alertRange, enemyLayerMask);
        foreach (Collider collider in colliders)
        {
            GameObject enemy = collider.gameObject;
            if (enemy != null)
            {
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    // Alert the enemy regardless of its own hearing range
                    enemyAI.OnHearSound(transform.position);
                    enemyAI.OnPlayerShoot(transform.position);
                    Debug.Log($"{gameObject.name} alerted {enemy.name} at {enemy.transform.position}.");
                }
            }
        }
    }


    void FleeFromSound(Vector3 soundPosition)
    {
        // Find a direction away from the sound
        Vector3 fleeDirection = (transform.position - soundPosition).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * fleeDistance;

        // Find nearest valid position on the NavMesh
        NavMeshHit hit;
        NavMesh.SamplePosition(fleePosition, out hit, fleeDistance, NavMesh.AllAreas);

        // Set destination to the found position
        agent.speed = runSpeed;
        agent.SetDestination(hit.position);
        //isRunning = true;

        Debug.Log($"{gameObject.name} is fleeing to {hit.position}");
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, alertRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, fleeDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, roamDist);
        
    }
}
