using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinPickUp : MonoBehaviour
{
   
    public int coinPoints = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
            GameManager.instance.AddScore(coinPoints);
            Destroy(gameObject);
        }
    }

}
