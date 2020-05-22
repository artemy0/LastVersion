using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuController : MonoBehaviour
{
    //элементы меню
    [SerializeField] private GameObject UIPanel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject VictoryPanel;

    private static bool gameIsOver = false;
    private static bool gameIsDefeated = false;

    private void Start()
    {
        PlayerAnchor.OnGameOver += LoseGame;
        PlayerAnchor.OnGameDefeated += WinGame;
    }

    private void OnDestroy()
    {
        PlayerAnchor.OnGameOver -= LoseGame;
        PlayerAnchor.OnGameDefeated -= WinGame;
    }

    public void StartGame()
    {
        if (gameIsOver)
        {
            gameIsOver = false;
            GameOverPanel.SetActive(false);
        }

        if (gameIsDefeated)
        {
            gameIsDefeated = false;
            VictoryPanel.SetActive(false);
        }

        //показывать информацию об текуцей сессии
        UIPanel.SetActive(true);

        Time.timeScale = 1f;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoseGame()
    {
        if (!gameIsOver)
        {
            gameIsOver = true;
            GameOverPanel.SetActive(true);
        }

        //скрыть информацию об текуцей сессии
        UIPanel.SetActive(false);

        Time.timeScale = 0f;

        //доп действия при поражении
    }

    public void WinGame()
    {
        if (!gameIsDefeated)
        {
            gameIsDefeated = true;
            VictoryPanel.SetActive(true);
        }

        //скрыть информацию об текуцей сессии
        UIPanel.SetActive(false);

        Time.timeScale = 0f;

        //доп действия при победе
    }
}
