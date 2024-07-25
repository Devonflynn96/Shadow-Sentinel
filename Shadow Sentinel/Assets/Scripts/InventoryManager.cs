using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{


    [Header("Inventory UI")]
    public static InventoryManager instance;

    [SerializeField] public GameObject inventoryMenu;
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private GameObject inventoryItemPrefab;

    [SerializeField] private Vector3 inventoryItemOffset;

    private bool menuActivated;

    private GameManager gameManager;

    public List<gunStats> gunList = new List<gunStats>();
    public List<KeyStats> keyList = new List<KeyStats>();
    public bool hasInvisibility = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in gunList)
        {
            GameObject obj = Instantiate(inventoryItemPrefab, inventoryContent);
            obj.GetComponentInChildren<TMP_Text>().text = item.name;
            // Optionally set up other UI elements like icon or stats
        }
        foreach (var item in keyList)
        {
            GameObject obj = Instantiate(inventoryItemPrefab, inventoryContent);
            obj.GetComponentInChildren<TMP_Text>().text = item.name;
        }
        if (hasInvisibility)
        {
            GameObject invisibilityObj = Instantiate(inventoryItemPrefab, inventoryContent);
            invisibilityObj.GetComponentInChildren<TMP_Text>().text = "Invisibility";
        }
    }

    public void addKey(KeyStats key)
    {
        keyList.Add(key);
        UpdateInventoryUI();
    }

    public bool CheckKey(KeyStats key)
    {
        return keyList.Contains(key);
    }

    public void AddToInventory(gunStats gun)
    {
        gunList.Add(gun);
        UpdateInventoryUI();
    }

    public void RemoveFromInventory(gunStats gun)
    {
        gunList.Remove(gun);
        UpdateInventoryUI();
    }

    public void addInvisibility()
    {
        hasInvisibility = true;
        UpdateInventoryUI();
    }

    public void dropInvisibilty()
    {
        hasInvisibility = false;
        UpdateInventoryUI();
    }
}
