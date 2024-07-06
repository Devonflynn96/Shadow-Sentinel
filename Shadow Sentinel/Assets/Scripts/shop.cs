using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shop : MonoBehaviour
{
    public void BuyItem(itemsInShop item)
    {
        if (GameManager.instance.SpendMoney(item.cost))
        {
            Instantiate(item.healthBox, Vector3.zero, Quaternion.identity); // Drop the item at position (0,0,0)
            Debug.Log("Item purchased and dropped!");
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }
}
