using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes

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

<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
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
            if (gameManager != null)
            {
                gameManager.stateUnpause();
            }

        }
    }

    public void addItem(string itemName, int quantity, MeshRenderer itemMesh)
    {
<<<<<<< Updated upstream
        
=======

>>>>>>> Stashed changes
    }
}
