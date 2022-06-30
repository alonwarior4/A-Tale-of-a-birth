using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{

    public void ManageSplashScreen()
    {
        if (PlayerPrefs.HasKey("FirstTimePassed"))
        {
            int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneBuildIndex);
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Thank");
        }
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
