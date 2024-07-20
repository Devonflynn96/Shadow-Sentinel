using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatController : MonoBehaviour
{
    [SerializeField] TMP_Text validation;
    [SerializeField] GameObject inputField;
    [SerializeField] TMP_InputField input;
    
    [SerializeField] GameObject player;
    [SerializeField] playerController playerScript;

    // Start is called before the first frame update
    void Start()
    {
        input = inputField.GetComponent<TMP_InputField>();
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        input.onEndEdit.AddListener(delegate { ParseInput(input); });
        validation.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        //If the pause menu is open, the player can press C to open the cheat input.
        if (Input.GetKeyDown(KeyCode.C) && GameManager.instance.isPaused)
        {
            ToggleInputField();
        }
        //If the player closes the pause menu, the input field will automatically close.
        if (inputField.activeSelf && !GameManager.instance.isPaused)
        {
            inputField.SetActive(false);
        }
    }

    public void ToggleInputField()
    {
        bool isActive = inputField.activeSelf;
        inputField.SetActive(!isActive);

    }

    public void ClearInputField()
    {
        input.text = string.Empty;
    }

    public void ParseInput(TMP_InputField cheat)
    {
        Debug.Log("Checking String");

        if (cheat.text.ToString() == "allyourbasearebelongtous")
        {
            Debug.Log("God Mode Activated!");
            playerScript.SetHPCurrent(99999);
            ClearInputField();
            StartCoroutine(GoodValidation());
        }
        else if (cheat.text.ToString() == "glitteringprizes")
        {
            Debug.Log("Money Cheat Activated!");
            GameManager.instance.AddScore(1000);
            ClearInputField();
            StartCoroutine(GoodValidation());
        }
        else
        {
            ClearInputField();
            StartCoroutine(BadValidation());
        }
    }

    IEnumerator GoodValidation()
    {
        validation.color = Color.green;
        validation.text = "Correct Input!";
        yield return new WaitForSeconds(5);
        validation.text = string.Empty;
    }

    IEnumerator BadValidation()
    {
        validation.color = Color.red;
        validation.text = "Incorrect Input!";
        yield return new WaitForSeconds(5);
        validation.text = string.Empty;
    }
}
