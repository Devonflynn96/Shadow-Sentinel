using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public Vector3 playerPos;
    public Quaternion playerRot;
    public List<gunStats> gunList;
    public int playerHP, playerBaseHP;
}
