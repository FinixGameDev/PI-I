using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;
    public PlayerController player;
    public int winDistance = 1000;

    [SerializeField] Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }

        progressBar.fillAmount =  GetProgress();
    }

    public float GetProgress()
    {
        float progress = (player.transform.position.z / winDistance);
        Debug.Log(progress);

        return progress;
    }
}
