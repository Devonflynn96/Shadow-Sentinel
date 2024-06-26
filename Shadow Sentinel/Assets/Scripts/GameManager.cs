using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;


    //menu variables
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    [SerializeField] TMP_Text enemyCountTxt;
    [SerializeField] TMP_Text currentMagTxt;
    [SerializeField] TMP_Text magCapTxt;
    public TMP_Text invisStatusText;
    public Image playerHPBar;
    public Image playerStealthBar;
    public Image invisCooldownBar;

    public GameObject player;
    public playerController playerScript;

    public bool isPaused;
    int enemyCount;

    [SerializeField] int numberSeenBy;
    [SerializeField] float stealthMod;

    bool hasLost;

    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        GameManager.instance.playerStealthBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStealthBar();

        //if escape is pressed the game is paused
        if (Input.GetButtonDown("Cancel"))
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

        if (GameManager.instance.playerStealthBar.fillAmount >= 1 && !hasLost)
        {
            youLose();
            hasLost = true;
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
        enemyCountTxt.text = enemyCount.ToString();


        if (enemyCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }

    }

    //Readout for current num bullets in mag
    public void currentMagCount(int amount)
    {
        currentMagTxt.text = amount.ToString("F0");
    }

    //UI Mag capacity update
    public void MagCap(int amount)
    {
        magCapTxt.text = amount.ToString("F0");
    }

    //Loss menu functionality
    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }

    public void AddSeen()
    {
        numberSeenBy++;
    }

    public void RemoveSeen() 
    { 
        numberSeenBy--;
    }

    public void UpdateStealthBar()
    {
        GameManager.instance.playerStealthBar.fillAmount += (numberSeenBy * stealthMod) * Time.deltaTime;

        if (numberSeenBy == 0)
        {
            GameManager.instance.playerStealthBar.fillAmount -= stealthMod * Time.deltaTime;
        }
    }
}
