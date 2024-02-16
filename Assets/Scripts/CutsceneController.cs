using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("DR_Level");
    }


    public void ReturnToMenu()
    {
        SceneManager.LoadScene("DR_Menu");
        
    }
 
}
