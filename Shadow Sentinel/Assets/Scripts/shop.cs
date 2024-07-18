using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shop : MonoBehaviour
{
    public void BuyItem(itemsInShop item)
    {
        if (GameManager.instance.SpendMoney(item.cost))
        {
            
            Vector3 playerPosition = GameManager.instance.player.transform.position; // get player position
            Vector3 playerForward = GameManager.instance.player.transform.forward; // player foward direction

            float distanceInFront = 1.0f; // distance in front of player
            Vector3 dropPosition = playerPosition + playerForward * distanceInFront; // drop postion in front of player

            Instantiate(item.healthBox, playerPosition, Quaternion.identity); //drop at player position
            Instantiate(item.invisibility, playerPosition, Quaternion.identity);
            Debug.Log("Item purchased and dropped!");

            if (item.invisibility != null)
            {
                InventoryManager.instance.addInvisibility();
            }
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }
}
