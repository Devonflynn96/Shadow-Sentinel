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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnpause();
    }
    public void quitLevel()
    {
        //quit level will no longer quit out of the application altogether
        //instead it will bring the player back to the title screen 
        SaveDataManager.Instance.SaveGame("Autosave.Save");
        SceneManager.LoadScene(0);
    }

    public void nextLevel()
    {
        SaveDataManager.Instance.SaveGame();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            GameManager.instance.stateUnpause();
        }
        else
        {
            Debug.LogWarning("No next level found.");
            SceneManager.LoadScene(0);
        }
    }
}
