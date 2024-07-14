using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSavePoint : MonoBehaviour
{
    [SerializeField] float savingTextTime = 2.0f; // Adjust the default saving text display time if needed

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(AutoSave());
        // Optionally, you can destroy the autosave point after triggering
       
    }

    IEnumerator AutoSave()
    {
        // Display saving text or indicator
        GameManager.instance.savingTxt.SetActive(true);

        // Perform autosave using SaveDataManager
        SaveDataManager.Instance.SaveGame("Autosave.Save");

        // Wait for specified time before hiding saving text
        yield return new WaitForSeconds(savingTextTime);

        // Hide saving text or indicator
        GameManager.instance.savingTxt.SetActive(false);
    }
}
