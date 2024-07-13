using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
   

    [Header("Inventory UI")]
    [SerializeField] public GameObject inventoryMenu;

    private bool menuActivated;
  

    void Update()
    {
        if (Input.GetButtonDown("Inventory") && menuActivated)
        {
            Time.timeScale = 1;
            inventoryMenu.SetActive(false);
            menuActivated = false;
        }
        else if (Input.GetButtonDown("Inventory") && !menuActivated)
        {
            Time.timeScale = 0;
            inventoryMenu.SetActive(true);
            menuActivated= true;
        }
        


    }

    public void addItem(string itemName, int quantity, Sprite itemSprite)
    {
       
    }
}
