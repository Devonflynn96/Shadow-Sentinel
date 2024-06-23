using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickUp : MonoBehaviour
{
    [SerializeField] gunStats gun;
    [SerializeField] int ammoAmount = 100; // Set the amount of ammo the box will give

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gun.ammoCur += ammoAmount; // Add ammo to the current ammo count

            // Check if the current ammo exceeds the max ammo
            if (gun.ammoCur > gun.ammoMax)
            {
                gun.ammoCur = gun.ammoMax; // Set current ammo to max ammo if it exceeds
            }
            GameManager.instance.playerScript.GetGunStats(gun);

        }
    }
}
