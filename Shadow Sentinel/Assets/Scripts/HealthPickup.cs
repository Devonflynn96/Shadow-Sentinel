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
        if (other.CompareTag("Player"))
        { 
          healthMax = GameManager.instance.playerScript.GetHPMax();
          healthCurrent = GameManager.instance.playerScript.GetHPCurrent();

            if(healthCurrent < healthMax)
            {
                GameManager.instance.playerScript.AddHealth(healthToAdd);
            }
            else
            {
                InventoryManager inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
                if(inventoryManager != null)
                {
                    inventoryManager.addItem(gameObject.name, 1, GetComponent<MeshRenderer>());
                }
                Destroy(gameObject);
            }
     
           
        }
    }

   
}
