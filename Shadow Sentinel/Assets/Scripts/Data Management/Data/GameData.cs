using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int scenesUnlocked;
    public int currScene;
    public Vector3 playerPos;
    public Quaternion playerRot;
    public List<gunStats> gunList;
    public List<KeyStats> keyList;
    public int selectedGun;
    public bool hasInvis;
    public int playerHP, playerBaseHP;

    public SerializableDict<string, bool> livingEnemies;
    public SerializableDict<string, Vector3> enemyLocations;
    public SerializableDict<string, Quaternion> enemyRots;
}
