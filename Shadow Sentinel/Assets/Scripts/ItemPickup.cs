using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] KeyStats key;
    private bool isPlayerNearby;

    void Start()
    {
        GameManager.instance.pickUpMessage.SetActive(false); // Ensure the message is hidden initially
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            GameManager.instance.pickUpMessage.SetActive(true); // Show the "Press E" message
        }
    }

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
            if (this.CompareTag("Key"))
            {
                PickUpKey();
            }
        }
    }

    void PickUpKey()
    {
        InventoryManager.instance.addKey(key);
        GameManager.instance.pickUpMessage.SetActive(false); // Hide the "Press E" message
        Destroy(gameObject);
    }
}
