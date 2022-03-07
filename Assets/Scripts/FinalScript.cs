using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScript : MonoBehaviour
{

    public void _ResetGame()
    {
        PlayerPrefs.SetInt("LEVELS", 1);
        //PlayerPrefs.Save();
        //SceneManager.LoadScene(1);
        SceneTransitionScript.instance.StartransitionTo(1);
    }
}
