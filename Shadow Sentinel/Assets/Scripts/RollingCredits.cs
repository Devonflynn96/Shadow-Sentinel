using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RollingCredits : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReturnToMainMenu());
    }

 IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(20f);
        SceneManager.LoadScene(0);
    }
}
