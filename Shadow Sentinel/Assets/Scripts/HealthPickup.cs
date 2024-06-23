using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] int healthToAdd;

    private int healthMax;
    private int healthCurrent;


    private void OnTriggerEnter(Collider other)
    {
        healthMax = GameManager.instance.playerScript.GetHPMax();
        healthCurrent = GameManager.instance.playerScript.GetHPCurrent();

        if (other.CompareTag("Player") && healthCurrent != healthMax)
        {
            GameManager.instance.playerScript.AddHealth(healthToAdd);
            Destroy(gameObject);
        }
    }
}
