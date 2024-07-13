
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemsInInventory : MonoBehaviour
{
    [SerializeField] string itemName;
    [SerializeField] int quantity;
    [SerializeField] MeshRenderer image;
    private InventoryManager inventoryManager;



    // Start is called before the first frame update
    void Start()
    {
        GameObject inventoryObject = GameObject.Find("Inventory");
        if (inventoryObject != null)
        {
            inventoryManager = inventoryObject.GetComponent<InventoryManager>();

        }
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (inventoryManager != null)
            {
                inventoryManager.addItem(itemName, quantity, image);
                Destroy(gameObject);
            }
        }
    }
}