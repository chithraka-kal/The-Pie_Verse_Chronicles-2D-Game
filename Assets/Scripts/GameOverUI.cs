using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject musicObject; // Drag your music GameObject here in Inspector

    private void Awake()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        else
            Debug.LogWarning("GameOver panel is not assigned!");
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;

            if (musicObject != null)
                musicObject.SetActive(false); // Stop music
            else
                Debug.LogWarning("Music object not assigned!");

            Debug.Log("[GAME OVER] Game Over panel shown. Game paused.");
        }
    }

    public void Retry()
    {
        ResumeGameState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        ResumeGameState();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        ResumeGameState();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ResumeGameState()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (musicObject != null)
            musicObject.SetActive(true); // Resume music if reloading

        Debug.Log("Game resumed. Music re-enabled.");
    }
}
