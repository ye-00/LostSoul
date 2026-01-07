using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene1"); // replace with your scene name
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
