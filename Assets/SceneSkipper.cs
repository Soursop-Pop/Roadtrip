using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSkipper : MonoBehaviour
{
    public bool loopAround = true; // Set to false if you don't want wraparound



    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        // Next scene with ]
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            LoadNextScene();
        }

        // Previous scene with [
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            LoadPreviousScene();
        }

        //// Reload current scene with R
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
    }

    void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextIndex = currentIndex + 1;

        if (nextIndex >= totalScenes)
            nextIndex = loopAround ? 0 : currentIndex;

        SceneManager.LoadScene(nextIndex);
    }

    void LoadPreviousScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int prevIndex = currentIndex - 1;

        if (prevIndex < 0)
            prevIndex = loopAround ? totalScenes - 1 : currentIndex;

        SceneManager.LoadScene(prevIndex);
    }
}