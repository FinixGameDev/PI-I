using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("DR_Cutscene_Intro");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
