using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] gunStats gun;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gun.ammoCur = gun.ammoMax;
            //InventoryManager.instance.AddToInventory(gun);
            GameManager.instance.playerScript.GetGunStats(gun);
            Destroy(gameObject);
        }
    }
}
