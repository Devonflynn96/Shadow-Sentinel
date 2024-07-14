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

    private bool menuActivated;

    private GameManager gameManager;

    private List<gunStats> gunList = new List<gunStats>();
    private List<KeyStats> keyList = new List<KeyStats>();

    void Awake()
    {
        instance = this;
    }

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
                if (gameManager != null)
                {
                    gameManager.stateUnpause();
                }

            }
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
    }

    public void addKey(KeyStats key)
    {
        keyList.Add(key);
        UpdateInventoryUI();
    }

    public bool CheckKey(KeyStats key)
    {
        for (int i = 0; i < keyList.Count; i++)
        {
            if (keyList.Contains(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
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


}
