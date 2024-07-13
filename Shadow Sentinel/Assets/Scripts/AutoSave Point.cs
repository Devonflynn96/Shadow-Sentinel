using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSavePoint : MonoBehaviour
{
    [SerializeField] float savingTextTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(AutoSave());
        SaveDataManager.Instance.SaveGame("Autosave.Save");
        Destroy(gameObject);
    }

    IEnumerator AutoSave()
    {
        GameManager.instance.savingTxt.SetActive(true);
        yield return new WaitForSeconds(savingTextTime);
        GameManager.instance.savingTxt.SetActive(false);
    }
}
