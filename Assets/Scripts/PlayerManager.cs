using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        Time.timeScale = 1f;
        isGameStarted = false;
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
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }

        progressBar.fillAmount =  GetProgress();
    }

    public float GetProgress()
    {
        float progress = (player.transform.position.z / winDistance);
        return progress;
    }
}
