using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoadingManager : MonoBehaviour
{
    public static LevelLoadingManager Instance;
    public GameObject loadingScreen; //this will be a prefab game object present in the UI 
    public Image loadingProgress;    //this is the fill bar of the loading panel

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public void SceneLoad(int sceneNumber)
    {
        //this function will call the coroutine to load the scene in the background
        //set the time scale to 1 so the game is not in a pause state
        StartCoroutine(LoadSceneAsync(sceneNumber));
        Time.timeScale = 1.0f;
    }


    //this is the function to load the scene in the background 
    //it takes a parameter of the scene's id in the build and loads that scene
    IEnumerator LoadSceneAsync(int sceneNumber)
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneNumber);

        loadingScreen.SetActive(true);
        //while the level loads the progress bar should fill up showing
        //the progress of the load
        while (!oper.isDone)
        {
            float progVal = Mathf.Clamp01(oper.progress / 0.9f);

            loadingProgress.fillAmount = progVal;

            yield return null;
        }
    }

}
