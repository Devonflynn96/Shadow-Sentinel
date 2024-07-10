using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.gameGoalUpdate(1);
    }

    
}
