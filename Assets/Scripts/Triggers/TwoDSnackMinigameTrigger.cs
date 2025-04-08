using UnityEngine;
using UnityEngine.SceneManagement;

public class TwoDSnackMinigameTrigger : MonoBehaviour
{
    [Header("Minigame Settings")]
    [Tooltip("Name of the minigame scene to load additively.")]
    public string sceneName;

    private bool minigameOpen = false;
    private bool inTrigger = false;

    [Tooltip("This flag should be set to true when the minigame is complete.")]
    public bool minigameComplete = false;

    void Update()
    {
        // If the player is in the trigger, the minigame is not open, hasn't completed yet, and the player presses F, start the minigame.
        if (inTrigger && !minigameOpen && !minigameComplete && Input.GetKeyDown(KeyCode.F))
        {
            StartMinigame();
        }
        // If the minigame is open and the minigameComplete flag is true, then end the minigame.
        if (minigameOpen && minigameComplete)
        {
            EndMinigame();
        }
    }

    void StartMinigame()
    {
        Debug.Log("Starting minigame: " + sceneName);
        minigameOpen = true;
        // Pause the main game. (Be sure your minigame runs with unscaled time if needed.)
        Time.timeScale = 0;
        // Load the minigame scene additively.
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    void EndMinigame()
    {
        Debug.Log("Ending minigame: " + sceneName);
        minigameOpen = false;
        // Unload the minigame scene.
        SceneManager.UnloadSceneAsync(sceneName);
        // Resume the main game.
        Time.timeScale = 1;
        // Reset the flag so the minigame can be played again in the future.
        minigameComplete = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }
}