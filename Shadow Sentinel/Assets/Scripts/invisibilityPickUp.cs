using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invisibilityPickUp : MonoBehaviour
{
    bool isPlayerNear;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           isPlayerNear = true;
            GameManager.instance.activateAbilityTxt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            GameManager.instance.activateAbilityTxt.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Alpha1))
        {
            InventoryManager.instance.addInvisibility();
            GameManager.instance.activateAbilityTxt.SetActive(false);
            Destroy(gameObject);
        }
    }



}
