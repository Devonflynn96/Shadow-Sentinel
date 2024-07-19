using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiniMapController : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject playerIconPrefab;
    public GameObject enemyIconPrefab;
    public GameObject pickupIconPrefab;
    public Canvas miniMapCanvas;
    public RectTransform miniMapTransform;
    public float miniMapScale = 0.5f;
    public float miniMapviewh = 50f;

    private Dictionary<Transform, GameObject> miniMapIcons = new Dictionary<Transform, GameObject>();
    private GameObject playerIcon;

    void Start()
    {
        // Initialize mini-map icons for enemies
        InitializeIcons("Enemy", enemyIconPrefab);

        InitializeIcons("Target", enemyIconPrefab);

        // Initialize mini-map icons for pickups
        InitializeIcons("Pickup", pickupIconPrefab);

        InitializeIcons("Player", playerIconPrefab);
    }

    void Update()
    {
        UpdateMiniMapIcons();
    }

    void InitializeIcons(string tag, GameObject iconPrefab)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

        foreach (var obj in objectsWithTag)
        {
            AddMiniMapIcon(obj.transform, iconPrefab);
        }
    }

    void AddMiniMapIcon(Transform targetTransform, GameObject iconPrefab)
    {
        GameObject icon = Instantiate(iconPrefab, transform); // Instantiate icon under the MiniMapController object
        miniMapIcons[targetTransform] = icon;
    }

    void UpdateMiniMapIcons()
    {
        List<Transform> iconsToRemove = new List<Transform>();

        foreach (var pair in miniMapIcons)
        {
            Transform targetTransform = pair.Key;
            GameObject icon = pair.Value;

            // Check if targetTransform is still valid (object is not destroyed)
            if (targetTransform == null)
            {
                iconsToRemove.Add(targetTransform);
                Destroy(icon);
                continue;
            }

            // Get the position of the target in world space
            Vector3 targetPosition = targetTransform.position;

            // Update the position of the icon to be above the target
            icon.transform.position = targetPosition + Vector3.up * miniMapviewh; // Adjust the height offset as needed
        }

        // Remove icons from dictionary that correspond to destroyed objects
        foreach (var target in iconsToRemove)
        {
            miniMapIcons.Remove(target);
        }
    }
}
