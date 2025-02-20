using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    private bool isMinigameLoaded = false;
    private string minigameSceneName = "PipeRotationPuzzleTest"; // Change this to your scene name

    public void StartMinigame()
    {
        if (!isMinigameLoaded)
        {
            SceneManager.LoadScene(minigameSceneName, LoadSceneMode.Additive);
            isMinigameLoaded = true;
        }
    }

    public void EndMinigame()
    {
        if (isMinigameLoaded)
        {
            SceneManager.UnloadSceneAsync(minigameSceneName);
            isMinigameLoaded = false;
        }
    }
}