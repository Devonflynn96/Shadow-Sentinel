using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invisibilityPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.instance.addInvisibility();
            Destroy(gameObject);
        }
    }

}
