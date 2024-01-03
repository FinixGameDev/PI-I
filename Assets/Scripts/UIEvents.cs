using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEvents : MonoBehaviour
{
    public void ReplayGame()
    {
        SceneManager.LoadScene("DR_Level");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
