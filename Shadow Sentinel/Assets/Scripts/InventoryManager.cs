using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
   

    [Header("Inventory UI")]
    [SerializeField] public GameObject inventoryMenu;

    private bool menuActivated;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            ToggleInventory();
        }
      

    }

    public void ToggleInventory()
    {
        menuActivated = !menuActivated;
        inventoryMenu.SetActive(menuActivated);

        if (menuActivated)
        {
            if (gameManager != null)
            { 
                gameManager.statePause();
            }
        }
        else
        {
            if(gameManager != null)
            {
                gameManager.stateUnpause();
            }
            
        }
    }

    public void addItem(string itemName, int quantity, MeshRenderer itemMesh)
    {
       
    }
}
