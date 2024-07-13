using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static int currLvl;
  public void NewGame()
    {
        SceneManager.LoadScene(1); //this should be the first level 
        SaveDataManager.Instance.GetLastModified();
        SaveDataManager.Instance.NewGame();
    }

    public void ContinueGame()
    {
        SaveDataManager.Instance.LoadGame(SaveDataManager.Instance.last);
        //this function should be something along the lines of
        //if a save profile is present then display the continue button 
        //and load the saved profile
        //but the button will only be present if a save profile is present
    }
    public void LevelSelect(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
        //upon clicking this button, a panel will open displaying all levels
        //only levels the player has played will be unlocked to replay
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
