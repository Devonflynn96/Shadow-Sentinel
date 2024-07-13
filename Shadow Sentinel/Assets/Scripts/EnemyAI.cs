using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour, ISaveData, IDamage
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for ID")]

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    [SerializeField] Renderer[] models;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [SerializeField] int HP;
    [SerializeField] int animTranSpeed;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;
    [SerializeField] int patrolTimer;

    [SerializeField] float shootRate;
    [SerializeField] int shootAngle;

    [SerializeField] GameObject bullet;

    [SerializeField] float hearingRange;  // Added hearing range
    [SerializeField] Transform[] patrolPoints;  // Patrol waypoints

    bool isShooting;
    bool isDead;
    bool playerInRange;
    bool destChosen;


    private bool isCrouched;
    [SerializeField] bool hasBeenSeen;

    Vector3 playerDir;
    Vector3 startingPos;

    float angleToPlayer;
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



    // Start is called before the first frame update
    void Start()
    {
        

        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        if (patrolPoints.Length > 0)
        {
            StartCoroutine(Patrol());
        }
        else
        {
            StartCoroutine(Roam());
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        //isCrouched bool is set to the isCrouched bool variable in playerScript. -Devon
        isCrouched = GameManager.instance.playerScript.GetCrouch();

        //These if statements will change the size of the enemy FOV depending on the state of the isCrouched bool. -Devon
        if (isCrouched)
        {
            viewAngle = 30;
        }

        if (!isCrouched)
        {
            viewAngle = 100;
        }


        float agentSpeed = agent.velocity.normalized.magnitude;

        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentSpeed, Time.deltaTime * animTranSpeed));

        //Checks to see if the player is seen. If player is seen, enemy will move towards player and start shooting. -Devon
        bool playerVisible = playerInRange && canSeePlayer();

        if (playerVisible)
        {
            

           

            StartCoroutine(Roam());

            if (patrolPoints.Length > 0)
            {
                StartCoroutine(Patrol());
            }
        }
        else if (!playerInRange)
        {
            StartCoroutine(Roam());

            if (patrolPoints.Length > 0)
            {
                StartCoroutine(Patrol());
            }
           
        }
        agent.stoppingDistance = playerVisible ? stoppingDistOrig : 0;
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            if (patrolPoints.Length == 0 || destChosen || agent == null || !agent.isOnNavMesh) yield break;
            {
                destChosen = true;
                int randomIndex = Random.Range(0, patrolPoints.Length);
                agent.SetDestination(patrolPoints[randomIndex].position);
                agent.stoppingDistance = 0;

                while (agent != null && agent.isOnNavMesh && agent.remainingDistance > 0.05f)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(patrolTimer);
                destChosen = false;
            }

        }
    }


    IEnumerator Roam()
    {
        if (destChosen || agent == null || !agent.isOnNavMesh || agent.remainingDistance >= 0.05f) yield break;
        {

          
            destChosen = true;
            yield return new WaitForSeconds(roamTimer);

            agent.stoppingDistance = 0;

            Vector3 ranPos = Random.insideUnitSphere * roamDist;
            ranPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destChosen = false;
            
        }
     

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasBeenSeen)
            {
                GameManager.instance.RemoveSeen();
                hasBeenSeen = false;
            }
            playerInRange = false;
        }
    }


    bool canSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y + 1, playerDir.z), transform.forward);

        // Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, new Vector3(playerDir.x, playerDir.y + 1, playerDir.z));

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            //can see the player!
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle && !GameManager.instance.playerScript.isInvisible)
            {
                if (!hasBeenSeen)
                {
                    GameManager.instance.AddSeen();
                    hasBeenSeen = true;
                }
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceTarget();
                }

                if (!isShooting && angleToPlayer <= shootAngle)
                    StartCoroutine(shoot());


                return true;
            }
        }

        if (hasBeenSeen)
        {
            GameManager.instance.RemoveSeen();
            hasBeenSeen = false;
        }

        agent.stoppingDistance = 0;
        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
    IEnumerator shoot()
    {
        isShooting = true;

        anim.SetTrigger("Shoot");


        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            isDead = true;
            hasBeenSeen = false;
            GameManager.instance.RemoveSeen();
            if (this.CompareTag("Target"))
            {

                GameManager.instance.gameGoalUpdate(-1);
                SaveDataManager.Instance.SaveGame("Autosave.Save");
            }
            Destroy(gameObject);
        }
    }
    IEnumerator flashDamage()
    {
        foreach (Renderer modelRenderer in models)
        {
            modelRenderer.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (Renderer modelRenderer in models)
        {
            modelRenderer.material.color = Color.white;
        }
    }
    public void OnPlayerShoot(Vector3 shootPosition)
    {
        float distanceToShoot = Vector3.Distance(transform.position, shootPosition);
        if (distanceToShoot <= hearingRange && !GameManager.instance.playerScript.gunSilenced)
        {
            Debug.Log("Enemy heard a sound and is moving towards it! (shoot)");
            agent.stoppingDistance = stoppingDistOrig;
            agent.SetDestination(shootPosition);
        }
    }



    public void OnHearSound(Vector3 soundPosition)
    {
        float distanceToSound = Vector3.Distance(transform.position, soundPosition);
        if (distanceToSound <= hearingRange && !GameManager.instance.playerScript.gunSilenced)
        {
            Debug.Log("Enemy heard a sound and is moving towards it!");
            // React to the sound (e.g., move towards it, become alert, etc.)
            agent.stoppingDistance = 0;
            agent.SetDestination(soundPosition);
            
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);

        Gizmos.color = Color.yellow;
        Vector3 viewAngleA = Quaternion.AngleAxis(-viewAngle / 2, Vector3.up) * transform.forward * hearingRange;
        Vector3 viewAngleB = Quaternion.AngleAxis(viewAngle / 2, Vector3.up) * transform.forward * hearingRange;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB);
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        Gizmos.color = Color.green;
        Vector3 shootAngleA = Quaternion.AngleAxis(-shootAngle / 2, Vector3.up) * transform.forward * hearingRange;
        Vector3 shootAngleB = Quaternion.AngleAxis(shootAngle / 2, Vector3.up) * transform.forward * hearingRange;
        Gizmos.DrawLine(transform.position, transform.position + shootAngleA);
        Gizmos.DrawLine(transform.position, transform.position + shootAngleB);

        //Hearing Range: A red wire sphere to visualize the enemy's hearing range.
        //Stopping Distance: A blue wire sphere to show the stopping distance.
       //View Angle: Two yellow lines to visualize the field of view.
       //Shoot Angle: Two green lines to visualize the shooting angle.
    }

    public void LoadData(GameData data)
    {
        if (data.livingEnemies.Count > 0)
        {
            data.livingEnemies.TryGetValue(id, out isDead);
            if (!isDead)
            {
                this.transform.rotation = data.enemyRots.GetValueOrDefault(id);
                this.transform.position = data.enemyLocations.GetValueOrDefault(id);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (data.livingEnemies.ContainsKey(id))
            {
                data.livingEnemies.Remove(id);
            }
            data.livingEnemies.Add(id, isDead);
            if (!isDead)
            {
                if (data.enemyLocations.ContainsKey(id))
                {
                    data.enemyLocations.Remove(id);
                }
                data.enemyLocations.Add(id, this.transform.position);
                if (data.enemyRots.ContainsKey(id))
                {
                    data.enemyRots.Remove(id);
                }
                data.enemyRots.Add(id, this.transform.rotation);
            }
        }
    }
}
