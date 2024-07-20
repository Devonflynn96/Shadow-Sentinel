using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    public static Spawnpoint Instance;

    [System.Serializable]
    public class LevelSpawnPoint
    {
        public int level;
        public Transform spawnPoint;
    }

    public List<LevelSpawnPoint> levelSpawnPoints;
    private Dictionary<int, Vector3> spawnPointDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSpawnPoints();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSpawnPoints()
    {
        spawnPointDictionary = new Dictionary<int, Vector3>();
        foreach (var levelSpawn in levelSpawnPoints)
        {
            if (levelSpawn.spawnPoint != null)
            {
                spawnPointDictionary.Add(levelSpawn.level, levelSpawn.spawnPoint.position);
                Debug.Log($"Added spawn point for level {levelSpawn.level}");
            }
            else
            {
                Debug.LogError($"Spawn point for level {levelSpawn.level} is null");
            }
        }
    }

    public Vector3 GetSpawnPoint(int level)
    {
        if (spawnPointDictionary.ContainsKey(level))
        {
            return spawnPointDictionary[level];
        }
        else
        {
            Debug.LogError("Spawn point for level " + level + " not found!");
            return Vector3.zero; // Default fallback position if not found
        }
    }
}
