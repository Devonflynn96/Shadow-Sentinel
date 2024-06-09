using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject enemyFOV;

    [SerializeField] EnemyFOVCheck enemyFOVScript;

    // Start is called before the first frame update
    void Start()
    {
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
}
