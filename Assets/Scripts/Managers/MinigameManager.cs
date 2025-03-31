using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    public bool isMinigameLoaded = false;
    public string minigameSceneName = "PipeRotationPuzzle_CAR";
    public LoopGameManager loopGameManager; // Reference to LoopGameManager if needed

    // Flag to determine if the minigame is paused
    public bool isPaused = false;

  

    // Start the minigame with a given difficulty and scene name.
    public void StartMinigame(int difficulty, string sceneName)
    {
        if (!isMinigameLoaded)
        {
            minigameSceneName = sceneName;

            // Setup the puzzle based on the given difficulty.
            if (loopGameManager != null)
            {
                loopGameManager.SetupPuzzleForDifficulty(difficulty);
            }
            else
            {
                Debug.LogError("LoopGameManager reference not set in MinigameManager.");
            }

            // Load the minigame scene additively.
            SceneManager.LoadScene(minigameSceneName, LoadSceneMode.Additive);
            isMinigameLoaded = true;
        }
    }

    // Ends the minigame.
    public void EndMinigame()
    {
        if (isMinigameLoaded)
        {
            SceneManager.UnloadSceneAsync(minigameSceneName);
            isMinigameLoaded = false;
        }
    }

 



}
