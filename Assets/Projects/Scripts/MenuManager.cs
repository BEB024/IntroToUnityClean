using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Diagnostics;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;   // the menu with Start/Scores/Quit buttons
    public GameObject scoresPanel;     // the panel showing top score

    [Header("UI Texts")]
    public Text highScoreText;         // text element to show best score

    [Header("Scene Settings")]
    public string firstSceneName = "SceneOne"; // scene to load when pressing Start

    void Start()
    {
        // Start on main menu
        mainMenuPanel.SetActive(true);
        if (scoresPanel != null)
            scoresPanel.SetActive(false);
    }

    // ---------------- BUTTON FUNCTIONS ----------------

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(firstSceneName);
    }

    public void ShowScores()
    {
        mainMenuPanel.SetActive(false);
        scoresPanel.SetActive(true);
        DisplayHighestScore();
    }

    public void BackToMenu()
    {
        mainMenuPanel.SetActive(true);
        scoresPanel.SetActive(false);
    }

    public void QuitGame()
    {
        //Debug.Log("Quitting game...");
        Application.Quit();
    }

    // ---------------- SCORE DISPLAY ----------------

    void DisplayHighestScore()
    {
        if (highScoreText == null) return;

        int highest = PlayerPrefs.GetInt("HighScore0", 0);
        highScoreText.text = $"Highest Score:\n{highest} POINTS";
    }
}
