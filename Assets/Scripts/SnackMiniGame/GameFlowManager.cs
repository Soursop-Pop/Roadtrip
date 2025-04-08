using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    [Header("Container GameObjects")]
    public GameObject shelfContainer;   // Parent for shelf snacks
    public GameObject armsContainer;    // Parent for friend’s arms and related UI

    [Header("Delivery Settings")]
    public int deliveredSnackCount = 0;
    public int maxSnackDeliveries = 4;

    // Store the main game scene's name (the scene we want to return to)
    private string previousSceneName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Record the current active scene as the main game scene.
            // This assumes the GameFlowManager is in the main game scene before the minigame is loaded.
            previousSceneName = SceneManager.GetActiveScene().name;
            // Optionally, you could also call DontDestroyOnLoad if you want this manager to persist across scene loads.
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start with the shelf view active and arms view inactive.
        if (shelfContainer != null)
            shelfContainer.SetActive(true);
        if (armsContainer != null)
            armsContainer.SetActive(false);
    }

    // Call this method when a snack is picked up.
    public void ActivateArmsLayer()
    {
        if (armsContainer != null)
            armsContainer.SetActive(true);
        if (shelfContainer != null)
            shelfContainer.SetActive(false);
    }

    // Call this method when the snack is successfully delivered.
    public void OnSnackDelivered()
    {
        deliveredSnackCount++;

        // Optionally, trigger friend comments here.
        // Then wait 3 seconds before switching back to the shelf view.
        StartCoroutine(TransitionBackToShelf());
    }

    IEnumerator TransitionBackToShelf()
    {
        // Wait for 3 seconds (while the friend’s comment is displayed, for example).
        yield return new WaitForSeconds(3f);

        // Switch back: disable arms view, re-enable shelf view.
        if (shelfContainer != null)
            shelfContainer.SetActive(true);
        if (armsContainer != null)
            armsContainer.SetActive(false);

        // If max deliveries are reached, hand off back to the main game scene.
        if (deliveredSnackCount >= maxSnackDeliveries)
        {
            Debug.Log("Max deliveries reached. Returning to main game scene: " + previousSceneName);

            // Option 1 (Additive mode):
            // If the minigame scene was loaded additively over the main game scene,
            // simply unload the minigame scene. This assumes this manager script is part of the minigame scene.
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            // Option 2 (Non-additive mode):
            // Alternatively, if your minigame isn't loaded additively, or you need to explicitly load the previous scene:
            // SceneManager.LoadScene(previousSceneName);
        }
    }
}
