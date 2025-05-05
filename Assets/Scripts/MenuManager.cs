using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject aboutPanel;
    public GameObject gameOverPanel;

    public int gameStartScene;

    void Start()
    {
        Debug.Log("[MenuManager] Start() called");
        StartCoroutine(DelayedCheckGameOver());
    }

    private System.Collections.IEnumerator DelayedCheckGameOver()
    {
        yield return null; // Wait 1 frame to allow UI to initialize
        yield return null; // Wait extra frame to ensure PlayerPrefs is read correctly

        int flag = PlayerPrefs.GetInt("ShowGameOver", 0);
        Debug.Log("[MenuManager] Checking GameOver flag: " + flag);

        if (flag == 1)
        {
            // Show Game Over panel
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                mainMenuPanel.SetActive(false);
                Debug.Log("[MenuManager] Game Over panel shown.");
            }

            // Clear the flag after use
            PlayerPrefs.SetInt("ShowGameOver", 0);
            PlayerPrefs.Save();
        }
        else
        {
            // Default main menu setup
            mainMenuPanel.SetActive(true);
            levelSelectPanel?.SetActive(false);
            aboutPanel?.SetActive(false);
            gameOverPanel?.SetActive(false);
            Debug.Log("[MenuManager] No GameOver flag. Showing Main Menu.");
        }
    }

    public void PlayGame()
    {
        // Reset Game Over flag when starting new game
        Debug.Log("[MenuManager] PlayGame() clicked. Resetting GameOver flag and loading level...");
        PlayerPrefs.SetInt("ShowGameOver", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameStartScene);
    }

    public void OpenLevels()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void OpenAbout()
    {
        mainMenuPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        levelSelectPanel?.SetActive(false);
        aboutPanel?.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    
}
