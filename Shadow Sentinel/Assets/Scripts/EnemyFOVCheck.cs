using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOVCheck : MonoBehaviour
{
    [SerializeField] GameObject rayStartPoint;

    [SerializeField] int visDistance;

    [SerializeField] Vector3 offset;

    private bool playerCheck;
    public bool playerSeen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck) 
        {
            CheckRay();
        }
    }

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
        }
    }

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
