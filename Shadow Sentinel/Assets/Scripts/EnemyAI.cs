using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform shootPos;
    [SerializeField] EnemyFOVCheck enemyFOVScript;

    [SerializeField] int HP;
    [SerializeField] int animTranSpeed;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;

    [SerializeField] float shootRate;

    [SerializeField] GameObject bullet;

    bool isShooting;
    bool playerInRange;
    bool destChosen;
    [SerializeField] bool hasBeenSeen;

    Vector3 playerDir;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.gameGoalUpdate(1);
      
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = GameManager.instance.player.transform.position - transform.position;

        float agentSpeed = agent.velocity.normalized.magnitude;

        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agentSpeed, Time.deltaTime * animTranSpeed));

        //Checks to see if the player is seen. If player is seen, enemy will move towards player and start shooting. -Devon

        if (enemyFOVScript.playerSeen)
        {
            if (!hasBeenSeen)
            {
                GameManager.instance.AddSeen();
                hasBeenSeen = true;
            }

            agent.SetDestination(GameManager.instance.player.transform.position);

            if (agent.remainingDistance < agent.stoppingDistance)
            {
                faceTarget();
            }



            if (!isShooting)
            {
                StartCoroutine(shoot());
            }
        }
       
        if(!enemyFOVScript.playerSeen)
        {
            hasBeenSeen = false;
        }
    }

    IEnumerator roam()
    {
        if (!destChosen && agent.remainingDistance < 0.05f)
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
            hasBeenSeen = false;
            GameManager.instance.RemoveSeen();
            GameManager.instance.gameGoalUpdate(-1);
            Destroy(gameObject);
        }
    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

}
