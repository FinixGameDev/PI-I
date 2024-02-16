using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] CanvasGroup _fadeScreen;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void PlayGame()
    {
        StartCoroutine(FadeOut("DR_Level"));
    }


    public void ReturnToMenu()
    {
        StartCoroutine(FadeOut("DR_Menu"));
        
    }

    private IEnumerator FadeIn()
    {
        float time = 0;
        while (time <= 1)
        {
            _fadeScreen.alpha = Mathf.Lerp(_fadeScreen.alpha, 0, time);
            time += Time.deltaTime / 2;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeOut(string sceneToLoad)
    {
        float time = 0;
        while (time <= 1)
        {
            _fadeScreen.alpha = Mathf.Lerp(_fadeScreen.alpha, 1, time);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(sceneToLoad);
    }

}
