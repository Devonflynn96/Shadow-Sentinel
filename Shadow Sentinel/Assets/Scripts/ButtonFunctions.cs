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
        SceneManager.LoadScene(0);
        SaveDataManager.Instance.SaveGame("Autosave.Save");
    }

}
