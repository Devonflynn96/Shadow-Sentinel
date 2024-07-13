using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int scenesUnlocked;
    public Vector3 playerPos;
    public Quaternion playerRot;
    public List<gunStats> gunList;
    public int playerHP, playerBaseHP;

    public SerializableDict<string, bool> livingEnemies;
    public SerializableDict<string, Vector3> enemyLocations;
    public SerializableDict<string, Quaternion> enemyRots;
}
