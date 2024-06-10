using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject enemyFOV;

    [SerializeField] EnemyFOVCheck enemyFOVScript;


    [SerializeField] int HP;

    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.gameGoalUpdate(1);
        enemyFOVScript = enemyFOV.GetComponent<EnemyFOVCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyFOVScript.playerSeen == true)
        { 
            agent.SetDestination(GameManager.instance.player.transform.position);
        }

    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
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
