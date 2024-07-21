using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class invisibilityPickUp : MonoBehaviour
{
    private bool isPlayerNearby;

    void Start()
    {
        GameManager.instance.pickUpMessage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            GameManager.instance.pickUpMessage.gameObject.SetActive(true); // Show the "Press 1 to Activate" message
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            GameManager.instance.pickUpMessage.gameObject.SetActive(false); // Hide the "Press 1 to Activate" message
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateInvisibility();
        }
    }

    void ActivateInvisibility()
    {
        
        GameManager.instance.pickUpMessage.gameObject.SetActive(false); // Hide the "Press 1 to Activate" message
        Destroy(gameObject);
        GameManager.instance.playerScript.StartCoroutine(GameManager.instance.playerScript.goInvisible());
        
    }






}
