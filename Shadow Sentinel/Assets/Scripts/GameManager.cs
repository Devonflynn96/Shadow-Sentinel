using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Globalization;

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
    [SerializeField] GameObject menuInventory;
  

    [Header("------ Player UI --------")]
    [SerializeField] TMP_Text enemyCountTxt;
    [SerializeField] TMP_Text currentMagTxt;
    [SerializeField] TMP_Text magCapTxt;
    [SerializeField] TMP_Text invisStatusText;
    [SerializeField] TMP_Text scoreCountTxt;
    [SerializeField] TMP_Text objectiveEnemy;
    [SerializeField] TMP_Text objectiveDetection;
    [SerializeField] TMP_Text objectiveRate;
    public GameObject pickUpMessage;
    public GameObject reloadingTxt;
    public GameObject savingTxt;
    public GameObject activateAbilityTxt;
    public Image playerHPBar;
    public Image playerStealthBar;
    public Image invisCooldownBar;
    [SerializeField] public GameObject invisOverlay;

    [Header("------ Game Data --------")]
    public GameObject player;
    public playerController playerScript;
    public bool isPaused;
    int enemyCount;
    int secondaryEnemyCount;
    public int money;
    [SerializeField] int numberSeenBy;
    [SerializeField] float stealthMod;

    public event Action<Vector3> OnPlayerShoot;


    bool hasBeenDetected;
  

    private int score = 0;
    private const string ScoreKey = "ScoreKey";
    public InventoryManager inventoryManager;



    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        GameManager.instance.playerStealthBar.fillAmount = 0;

        UpdateCoinScoreText();

        inventoryManager = GetComponent<InventoryManager>();
        score = PlayerPrefs.GetInt(ScoreKey, 0);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateStealthBar();
        UpdateCoinScoreText();

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
            if (menuActive == null)
            {
                ToggleShopMenu();
                menuActive = menuShop;
            }
            else if (menuActive == menuShop)
            {
                stateUnpause();
            }


        }

        if (Input.GetButtonDown("Inventory"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuInventory;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuInventory)
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
    }

    public void secondaryGoalUpdate(int amt)
    {
        secondaryEnemyCount += amt;
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
            if (secondaryEnemyCount >= 0)
            {
                enemyCountTxt.color = Color.green;
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


    void OnApplicationQuit()
    {
        // Save the score when the application quits
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
    }


    public void AddScore(int value)
    {
        money += value;
        UpdateCoinScoreText();
    }

    public void UpdateCoinScoreText()
    {
        scoreCountTxt.text = "Coins: " + money.ToString();
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
            UpdateCoinScoreText();
            return true;
        }
        return false;
    }

    public void OpenShopMenuButton()
    {
        ToggleShopMenu();
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
