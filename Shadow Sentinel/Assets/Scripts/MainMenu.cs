using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public static int currLvl;
    [SerializeField] GameObject continueButton;
    public void Update()
    {
        string lastSave = SaveDataManager.Instance.GetLastModified();
        if(!string.IsNullOrEmpty(lastSave) )
        {
            continueButton.SetActive(true);
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1); //this should be the first level
        SaveDataManager.Instance.NewGame();
        Time.timeScale = 1f;
    }

    public void ContinueGame()
    {
        string lastModifiedSave = SaveDataManager.Instance.GetLastModified();

        if (!string.IsNullOrEmpty(lastModifiedSave))
        {
            SaveDataManager.Instance.LoadGame(lastModifiedSave);
            SceneManager.LoadScene(1); // Load the saved scene
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogWarning("No save file found.");
        }
    }
    public void LoadCredits()
    {
        StartCoroutine(playCredits());
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif !UNITY_WEBGL
    Application.Quit();
#endif
    }

    IEnumerator playCredits()
    {
        SceneManager.LoadScene(6);
        yield return new WaitForSeconds(5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
