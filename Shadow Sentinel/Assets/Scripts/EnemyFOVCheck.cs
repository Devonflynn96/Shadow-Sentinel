using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOVCheck : MonoBehaviour
{
    [SerializeField] GameObject rayStartPoint;
    [SerializeField] BoxCollider fieldOfView;

    [SerializeField] int visDistance;

    [SerializeField] Vector3 offset;

    private bool isCrouched;

    private bool playerCheck;
    public bool playerSeen;

    void Start()
    {
        isCrouched = GameManager.instance.playerScript.GetCrouch();
    }

    void Update()
    {
        //isCrouched bool is set to the isCrouched bool variable in playerScript. -Devon
        isCrouched = GameManager.instance.playerScript.GetCrouch();

        if (playerCheck)
        {
            CheckRay();
        }

        //These if statements will change the size of the enemy FOV depending on the state of the isCrouched bool. -Devon
        if (isCrouched)
        {
            fieldOfView.size = new Vector3(16, 1, 16);
        }

        if (!isCrouched)
        {
            fieldOfView.size = new Vector3(32, 1, 32);
        }

    }

    //If the player enters the FOV collider, the playerCheck bool is set to true.
    //Once the player leaves the FOV collider, the playerCheck bool is false. -Devon

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            playerCheck = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerCheck = false;
            playerSeen = false;
            GameManager.instance.RemoveSeen();
        }
    }

    //This function will draw a raycast from the enemy towards the player.
    //If there are any objects with the tag "Obstacle" between the player and the enemy,
    //the player will not be seen. -Devon
    private void CheckRay()
    {
        RaycastHit hit;

        Debug.DrawRay(rayStartPoint.transform.position, GameManager.instance.player.transform.position - rayStartPoint.transform.position, Color.red);

        if (Physics.Raycast(rayStartPoint.transform.position, GameManager.instance.player.transform.position - rayStartPoint.transform.position, out hit))
        {
            Debug.Log(hit.transform.name);

            if (hit.transform.tag == "Player")
            {
                playerSeen = true;
            }
            else if (hit.transform.tag == "Obstacle")
            {
                playerSeen = false;
            }
        }
    }
}
