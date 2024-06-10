using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    public Image playerHPBar;
    public GameObject player;
    public playerController playerController;
    public playerController playerScript;

    //variable for enemycount display
    [SerializeField] TMP_Text enemyCountTxt;
    //menu variables
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    

    int enemyCount;
    bool isPaused;
    

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if escape is pressed the game is paused
        if(Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }
    //functions for pause states
    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        menuActive = null;
    }

    // ***** Game Goal reporting, to be completed once game goal is finalized *****
    public void gameGoalUpdate(int amount)
    {
        //win condition: once enemy count is zero player should be able to escape
        enemyCount += amount;
        enemyCountTxt.text = enemyCount.ToString("F0");


        if(enemyCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }

    }    

    //Loss menu functionality
    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }
}
