using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnedObject;
    [SerializeField] int spawnLimit;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform[] spawnPos;

    //private variables
    int spawnCount;
    bool isSpawning;
    bool startSpawning; 


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.gameGoalUpdate(spawnLimit);
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < spawnLimit && !isSpawning)
        {
            StartCoroutine(spawn());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;
        int arrayPos = Random.Range(0, spawnPos.Length);
        Instantiate(spawnedObject, spawnPos[arrayPos].position, spawnPos[arrayPos].rotation);
        spawnCount++;
        yield return new WaitForSeconds(spawnTimer);
        isSpawning =false;
    }
}
