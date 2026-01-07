using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private bool gamePaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // ================= PAUSE =================
    public void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
        pauseMenu.SetActive(true);
    }

    // ================= RESUME =================
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        pauseMenu.SetActive(false);
    }

    // ================= UI BUTTON =================
    public void Home()
    {
        Time.timeScale = 1f; // penting!
        SceneManager.LoadScene(0);
    }
}
