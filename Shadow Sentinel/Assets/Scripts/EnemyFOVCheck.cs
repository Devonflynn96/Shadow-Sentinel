using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOVCheck : MonoBehaviour
{
    public bool playerSeen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerSeen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerSeen = false;
        }
    }
}
