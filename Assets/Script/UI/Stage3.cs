using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage3 : MonoBehaviour
{
    public void LoadNextStage()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        if (index + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene("GameScene3");
        }
    }
}
