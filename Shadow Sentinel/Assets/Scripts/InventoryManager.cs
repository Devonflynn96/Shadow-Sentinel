using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private GameObject inventoryItemPrefab;

    private List<gunStats> inventory = new List<gunStats>();

    void Awake()
    {
        instance = this;
    }

    public void AddToInventory(gunStats gun)
    {
        inventory.Add(gun);
        UpdateInventoryUI();
    }

    public void RemoveFromInventory(gunStats gun)
    {
        inventory.Remove(gun);
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventory)
        {
            GameObject obj = Instantiate(inventoryItemPrefab, inventoryContent);
            obj.GetComponentInChildren<TMP_Text>().text = item.name;
            // Optionally set up other UI elements like icon or stats
        }
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void addItem(string itemName, int quantity, MeshRenderer itemMesh)
    {
       

    }
}
