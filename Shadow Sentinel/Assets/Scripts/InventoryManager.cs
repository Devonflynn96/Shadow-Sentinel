using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
<<<<<<< HEAD
=======
   
>>>>>>> parent of e097c95 (Merge branch 'main' of https://github.com/Devonflynn96/Shadow-Sentinel)

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
      

<<<<<<< HEAD

=======
>>>>>>> parent of e097c95 (Merge branch 'main' of https://github.com/Devonflynn96/Shadow-Sentinel)
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
<<<<<<< HEAD
        
=======
       
>>>>>>> parent of e097c95 (Merge branch 'main' of https://github.com/Devonflynn96/Shadow-Sentinel)
    }
}
