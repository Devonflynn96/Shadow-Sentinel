using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;


    //menu variables

    [Header("------ Menu UI --------")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuShop;
  

    [Header("------ Player UI --------")]
    [SerializeField] TMP_Text enemyCountTxt;
    [SerializeField] TMP_Text currentMagTxt;
    [SerializeField] TMP_Text magCapTxt;
    [SerializeField] TMP_Text invisStatusText;
    [SerializeField] TMP_Text scoreCountTxt;
    [SerializeField] TMP_Text objectiveEnemy;
    [SerializeField] TMP_Text objectiveDetection;
    [SerializeField] TMP_Text objectiveRate;
    public GameObject reloadingTxt;
    public GameObject activateAbilityTxt;
    public Image playerHPBar;
    public Image playerStealthBar;
    public Image invisCooldownBar;

    [Header("------ Game Data --------")]
    public GameObject player;
    public playerController playerScript;
    public bool isPaused;
    int enemyCount;
    public int money;
    [SerializeField] int numberSeenBy;
    [SerializeField] float stealthMod;

    public event Action<Vector3> OnPlayerShoot;


    bool hasBeenDetected;
  

    private int score = 0;

    public InventoryManager inventoryManager;



    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        GameManager.instance.playerStealthBar.fillAmount = 0;

        UpdateCoinScoreText();

        inventoryManager = GetComponent<InventoryManager>();
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

        if (GameManager.instance.playerStealthBar.fillAmount >= 1 && !hasBeenDetected)
        {
            objectiveDetection.color = Color.red;
            hasBeenDetected = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleShopMenu();
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
        scoreCountTxt.text = score.ToString();
    }

    public void YouWin()
    {
            if (!hasBeenDetected)
            {
                objectiveDetection.color = Color.green;
                objectiveRate.text = "100%";
            }
            else
            {
                objectiveRate.text = "50%";
            }
            objectiveEnemy.color = Color.green;
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
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

    public void SetInvisText(string text)
    {
        invisStatusText.text = text;
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateCoinScoreText();
    }

    private void UpdateCoinScoreText()
    {
        scoreCountTxt.text = "Coins: " + score.ToString();
    }

 

    public void ToggleShopMenu()
    {
        bool isActive = menuShop.activeSelf;
        menuShop.SetActive(!isActive);
        statePause();
    }

    public bool SpendMoney(int amount)
    {
        if (score >= amount)
        {
            score -= amount;
            return true;
        }
        return false;
    }
 



    public void PlayerShoot(Vector3 shootPosition)
    {
        if (OnPlayerShoot != null)
        {
            OnPlayerShoot(shootPosition);
        }
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }

}
