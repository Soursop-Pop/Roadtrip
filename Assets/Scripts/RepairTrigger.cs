using UnityEngine;
using UnityEngine.SceneManagement;

public class RepairTrigger : MonoBehaviour
{
    public VehicleController vehicle;
    private bool playerNearby = false;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.F))
        {
            StartMinigame();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            UIManager.ShowRepairPrompt(true); // Show UI "Press E to Fix"
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            UIManager.ShowRepairPrompt(false); // Hide UI
        }
    }

    void StartMinigame()
    {
        SceneManager.LoadScene("PipeRotationPuzzle_CAR", LoadSceneMode.Additive);
    }

    public void EndMinigame()
    {
        SceneManager.UnloadSceneAsync("PipeRotationPuzzle_CAR");
        vehicle.Repair();
    }
}