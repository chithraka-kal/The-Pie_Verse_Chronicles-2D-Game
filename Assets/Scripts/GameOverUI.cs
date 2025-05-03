using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void Awake()
    {
        Time.timeScale = 1f; // Ensure game is unpaused on scene load
    }

    private void Start()
    {
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); // Start with panel off
        else
            Debug.LogWarning("GameOver panel is not assigned!");
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
            sound.SetActive(false);  // Pause the game
            Debug.Log("Game Over panel shown. Game paused.");
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Ensure this scene is added in Build Settings
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in editor
#endif
    }
 public GameObject sound;
    public void PauseGame()
{
    Time.timeScale = 0f;  // Pauses all in-game physics, animations, etc.
    AudioListener.pause = true;
    sound.SetActive(false); // Optionally disable sound or mute audio
     // Optionally pause all audio
    Debug.Log("Game Paused");
}

public void ResumeGame()
{
    Time.timeScale = 1f;  // Resumes game time
    AudioListener.pause = false;
}

}