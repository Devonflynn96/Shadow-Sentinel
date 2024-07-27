using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void resumePlay()
    {
        GameManager.instance.stateUnpause();
    }

    public void restartLevel()
    {
        LevelLoadingManager.Instance.SceneLoad(SceneManager.GetActiveScene().buildIndex);
        GameManager.instance.stateUnpause();
        InventoryManager.instance.gunList.Clear();
        InventoryManager.instance.keyList.Clear();
        GameManager.instance.playerScript.SetSelectedGun(0);
    }
    public void quitLevel()
    {
        //quit level will no longer quit out of the application altogether
        //instead it will bring the player back to the title screen 
        SaveDataManager.Instance.SaveGame("Autosave.Save");
        LevelLoadingManager.Instance.SceneLoad(0);
    }

    public void nextLevel()
    {
        SaveDataManager.Instance.SaveGame();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            LevelLoadingManager.Instance.SceneLoad(nextSceneIndex);
            GameManager.instance.stateUnpause();
            InventoryManager.instance.gunList.Clear();
            InventoryManager.instance.keyList.Clear();
            GameManager.instance.playerScript.SetSelectedGun(0);
        }
        else
        {
            Debug.LogWarning("No next level found.");
            LevelLoadingManager.Instance.SceneLoad(0);
        }
    }
}
