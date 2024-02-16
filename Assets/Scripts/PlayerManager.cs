using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public static bool isGameStarted;

    public GameObject gameOverPanel;
    public GameObject startPanel;
    public PlayerController player;
    public int winDistance = 1000;

    [SerializeField] Image progressBar;
    [SerializeField] Image energyBar;

    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _energy = 1f;

    [SerializeField] private string _nextScene = "";
    [SerializeField] CanvasGroup _fadeScreen;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        Time.timeScale = 1f;
        isGameStarted = false;

        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        if (SwipeManager.tap && !isGameStarted)
        {
            isGameStarted = true;
            Destroy(startPanel);
        }

        if (gameOver)
        {
            //Time.timeScale = 0f;
            //gameOverPanel.SetActive(true);
            SceneManager.LoadScene("DR_Level");
        }

        progressBar.fillAmount =  GetProgress();

        if (progressBar.fillAmount >= 1)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        StartCoroutine(FadeOut(_nextScene));
    }

    public float GetProgress()
    {
        float progress = (player.transform.position.z / winDistance);
        return progress;
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
            time += Time.deltaTime / 4;
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
