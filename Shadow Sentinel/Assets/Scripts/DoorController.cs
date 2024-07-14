using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Animator doorAnim;
    [SerializeField] bool doorLocked;
    [SerializeField] KeyStats keyNeeded;
    private InventoryManager inventoryManager;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !doorLocked)
        {
            doorAnim.SetBool("character_nearby", false);
        }
    }
}
