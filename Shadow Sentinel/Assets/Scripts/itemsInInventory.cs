using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemsInInventory : MonoBehaviour
{
    [SerializeField] string itemName;
    [SerializeField] int quantity;
    [SerializeField] Sprite image;
     private InventoryManager inventoryManager;



    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("Inventory") .GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inventoryManager.addItem(itemName, quantity, image);
            Destroy(gameObject);
        }
    }
}
