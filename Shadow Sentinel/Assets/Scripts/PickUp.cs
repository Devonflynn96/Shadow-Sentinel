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
<<<<<<< HEAD

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            GameManager.instance.pickUpMessage.SetActive(false); // Hide the "Press E" message
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PickUpGun();
        }
    }

    void PickUpGun()
    {
        gun.ammoCur = gun.ammoMax;
        
        GameManager.instance.playerScript.GetGunStats(gun);
        GameManager.instance.pickUpMessage.SetActive(false); // Hide the "Press E" message
        Destroy(gameObject);
    }
=======
>>>>>>> parent of e097c95 (Merge branch 'main' of https://github.com/Devonflynn96/Shadow-Sentinel)
}
