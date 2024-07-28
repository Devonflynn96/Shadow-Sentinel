using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Animator doorAnim;
    [SerializeField] bool doorLocked;
    [SerializeField] KeyStats keyNeeded;
    private InventoryManager inventoryManager;
    [SerializeField] TMP_Text keyCheckText;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        keyCheckText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (inventoryManager.CheckKey(keyNeeded))
        {
            doorLocked = false;
        }
        if (other.CompareTag("Player") && !doorLocked)
        {
            doorAnim.SetBool("character_nearby", true);
        }
        else if (other.CompareTag("Player") && doorLocked)
        {
            keyCheckText.text = "Key Not Found!";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        keyCheckText.text = string.Empty;
        if (other.CompareTag("Player") && !doorLocked)
        {
            doorAnim.SetBool("character_nearby", false);
        }
    }
}
