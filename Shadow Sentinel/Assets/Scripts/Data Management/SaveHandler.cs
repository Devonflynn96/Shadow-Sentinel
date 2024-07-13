using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveHandler
{
    // String variables to hold save data directory pathway
    private string saveDirectoryPath = "";
    private string saveFileName = "";
    private readonly string saveFolder = "Saves";
    private bool useEncryption = false;

    //String to encrypt code, reducing ability to manipulate data
    private readonly string encryptWord = "dossier";


    //Constructor for the save data handler
    public SaveHandler(string saveDirectoryPath, string saveFileName, bool useEncryption)
    {
        this.saveDirectoryPath = saveDirectoryPath;
        this.saveFileName = saveFileName;
        this.useEncryption = useEncryption;
    }

    //Load method, reads serialized data from a file and deserializes it
    public GameData Load(string saveFileName)
    {
        // create a string called fullPath that combines the saveDirectoryPath,
        // SaveFolder, and saveFileName, using Path.Combine as it accomodates
        // different path separators.
        string fullPath = Path.Combine(saveDirectoryPath, saveFolder, saveFileName);
        // Initialize a GameData object to null for now, 
        // will be set to a value if data exists
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            //Use try-catch to catch errors if any during file loading
            try
            {
                //Initialize string to store data as it's loaded in.
                string datatoLoad;
                // Use a using() filestream as a safer way to
                // ensure file gets closed after reading
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    // using() streamreader, only operates within brackets
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        //Set string equal to the data contained within the file 
                        datatoLoad = reader.ReadToEnd();
                    }
                }

                if(useEncryption)
                {
                    datatoLoad = XOREncrypt(datatoLoad);
                }
                //Deserialize the data from JSon and make it readable in C#
                loadedData = JsonUtility.FromJson<GameData>(datatoLoad);

            }
            catch
            {
                Debug.LogWarning("Error when trying to load from data.");
            }
        }
        //Returns null if file doesn't exist, or the
        //saved GameData from the file if it exists
        return loadedData;
    }
    //Save method, saves data to a file and serializes it
    public void Save(GameData data, string saveFileName)
    {
        // Create a path using Path.Combine to accomodate all OS's
        string fullPath = Path.Combine(saveDirectoryPath, saveFolder, saveFileName);
        //Try-catch to write the data or report an error
        try
        {
            //Create a directory for the file to go to
            //if it doesn't exist previously
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize data into another format (in this case JSon), using true to format it
            string datatoSave = JsonUtility.ToJson(data, true);

            if(useEncryption)
            {
                datatoSave = XOREncrypt(datatoSave);
            }
            //Write to system using a using() block to ensure file closed
            //after operations complete
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(datatoSave);
                }
            }
        }
        catch
        {
            Debug.LogWarning("Error saving data");
        }
    }

    //Create a dictionary of all Saves available using the string and connected data
    public Dictionary<string, GameData> LoadAllSaves()
    {
        // Create a new dictionary to add saves to.
        Dictionary<string, GameData> savedGames = new Dictionary<string, GameData>();
        //Use an IEnumerable (cleaner, easier to read code) to loop through
        //all directory names in the data path directory
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(saveDirectoryPath).GetDirectories();

        foreach (DirectoryInfo dirInfo in dirInfos) 
        {
            // Check if file exists
            string fullPath = Path.Combine(saveDirectoryPath, saveFolder, saveFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading because no save data found." + saveFileName);
                continue;
            }
            //If file exists, load game data and add to dictionary with fileName
            GameData saveData = Load(saveFileName);
            // Double check to see if data isn't null.
            if(saveData != null)
            {
                savedGames.Add(saveFileName, saveData);
            }
            else
            {
                Debug.LogError("Tried to load profile, but something went wrong. File Name:" +  saveFileName);
            }
        }
        return savedGames;
    }

    public string LoadLastModified()
    {
        DateTime newest = DateTime.MinValue;
        string newestFile = "";
        DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(saveDirectoryPath, saveFolder));
        foreach (FileInfo file in dirInfo.GetFiles())
        {
            DateTime dt = file.LastWriteTime;
            if(dt > newest)
            {
                newest = dt;
                newestFile = file.Name;
            }

        }
        Debug.Log(newestFile);
        return newestFile;
    }



    // Use an Exclusive "or" XOR encryption as it will allow transformation
    // back and forth of data with one method
    private string XOREncrypt(string data)
    {
        string encryptedData = "";
        for (int i = 0; i <data.Length; i++)
        {
            encryptedData += (char)(data[i]) ^ encryptWord[i % encryptWord.Length];
        }
        return encryptedData;
    }    
}
