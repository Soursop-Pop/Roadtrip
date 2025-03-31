using UnityEngine;
using System.Collections;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    [Header("Container GameObjects")]
    public GameObject shelfContainer;   // Parent for shelf snacks
    public GameObject armsContainer;    // Parent for friend’s arms and related UI

    [Header("Delivery Settings")]
    public int deliveredSnackCount = 0;
    public int maxSnackDeliveries = 3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // No need for DontDestroyOnLoad if you're staying in one scene.
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

    // Call this method when the snack is dropped into the arms.
    // This can be triggered from your SnackCollision script.
    public void OnSnackDelivered()
    {
        deliveredSnackCount++;

        // Optionally, trigger any friend comments here via your FriendArmsManager.
        // Then wait 3 seconds before switching back to the shelf.
        StartCoroutine(TransitionBackToShelf());
    }

    IEnumerator TransitionBackToShelf()
    {
        // Wait for 3 seconds while the friend’s comment is displayed.
        yield return new WaitForSeconds(3f);

        // Switch back: disable arms view, re-enable shelf view.
        if (shelfContainer != null)
            shelfContainer.SetActive(true);
        if (armsContainer != null)
            armsContainer.SetActive(false);

        // Optionally, handle what happens when max deliveries are reached.
        if (deliveredSnackCount >= maxSnackDeliveries)
        {
            Debug.Log("Max deliveries reached. Proceeding to the next game phase.");
            // Insert code to trigger the next phase.
        }
    }
}
