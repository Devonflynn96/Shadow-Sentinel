using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveDataManager : MonoBehaviour
{
    [Header("File Configuration")]
    [SerializeField] private string fileName;
    [SerializeField] private bool Encrypt;
    public string last;

    //GameData object to save data to/load from
    //and list of objects that save/load data
    private GameData gameData;
    private List<ISaveData> dataSavingObjects;

    private SaveHandler saveHandler;

    //private string selectedSaveName = "";
    public static SaveDataManager Instance;

    [SerializeField] float savingTextTime;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            //Ensure initial save manager is only one throughout game.
            Destroy(this.gameObject);
            return;
        }
        //To ensure Save manager persists through all scenes
        DontDestroyOnLoad(gameObject);
        this.saveHandler = new SaveHandler(Application.persistentDataPath, fileName, Encrypt);
        GetLastModified();
    }

    //New game method. Sets data to a new save data.
    public void NewGame()
    {
        this.gameData = new GameData();
        this.gameData.livingEnemies = new SerializableDict<string, bool>();
        this.gameData.enemyLocations = new SerializableDict<string, Vector3>();
        this.gameData.enemyRots = new SerializableDict<string, Quaternion>();
        this.gameData.gunList = new List<gunStats>();
        this.gameData.keyList = new List<KeyStats>();
    }
    //Load game method, checks to see if the data selected exists and loads it if so.
    public void LoadGame(string saveFileName = "Level")
    {
        //loads data using the save handler
        this.gameData = saveHandler.Load(saveFileName);

        //Check to see if data exists and return if not
        if (this.gameData == null)
        {
            NewGame();
            return;
        }

        //Loop through the project and load the relevant information for each item that exists
        foreach (ISaveData DataSavingObject in dataSavingObjects)
        {
            DataSavingObject.LoadData(gameData);
        }
    }
    //Save Game method, loops through the project, saving all flagged information.
    public void SaveGame(string saveFileName = "Default.Save")
    {
        //If no gameData selected.
        if (this.gameData == null)
            return;

            //Loops through list of dataSaving objects and captures data to save, saving it to a file.
            foreach (ISaveData dataSavingObject in dataSavingObjects)
            {
                dataSavingObject.SaveData(ref gameData);
            }

            //Calls saveHandler to save the data with the fileName
            saveHandler.Save(gameData, saveFileName);
        
    }
    //Use OnApplicationQuit to save the game if left by clicking quit.
    private void OnApplicationQuit()
    {
        SaveGame("Autosave.Save"); 
    }

    private List<ISaveData> FindAllDataSavingObjects()
    {
        //Use an IEnumerable to populate a list of Objects that save data,
        //searching using FindObjectsOfType to locate objects deriving from
        //both MonoBehaviour and ISaveData
        IEnumerable<ISaveData> dataSavers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveData>();
        Debug.Log(dataSavers.Count());

        return new List<ISaveData>(dataSavers);
    }
    //Use OnEnable, as it calls after awake and before start, to call OnSceneLoaded
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    //Use OnDisable to remove objects added on enable
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    //Use OnSceneLoaded to locate all the objects that save data and load the game.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataSavingObjects = FindAllDataSavingObjects();
        LoadGame();
    }
    //Use OnSceneUnloaded to save the game when unloaded
    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }
    //Method to get all the save games loaded into a dictionary.
    public Dictionary<string, GameData> GetAllSaveGames()
    {
        return saveHandler.LoadAllSaves();
    }

    public string GetLastModified()
    {
        return  last = saveHandler.LoadLastModified();
    }
}
