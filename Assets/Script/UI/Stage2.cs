using UnityEngine;
using UnityEngine.SceneManagement;


public class Stage2 : MonoBehaviour
{
    public GameObject exitPanel;
    public string nextSceneName;

    void Start()
    {
        exitPanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            exitPanel.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            exitPanel.SetActive(false);
        }
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
