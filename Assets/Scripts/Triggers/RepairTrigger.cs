using UnityEngine;
using UnityEngine.SceneManagement;

public class RepairTrigger : MonoBehaviour
{
    public VehicleController vehicle;
    private bool playerNearby = false;
    public LoopGameManager loopGameManager;
    public GameObject loopGameObject;

    public int puzzleDifficulty = 0;


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
            UIManager.ShowRepairPrompt(true); // Show UI "Press F to Fix"
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
        //SceneManager.LoadScene("PipeRotationPuzzle_CAR", LoadSceneMode.Additive);
        //loopGameObject.SetActive(true);
        loopGameManager.SetupPuzzleForDifficulty(puzzleDifficulty);

    }

    public void EndMinigame()
    {
        //SceneManager.UnloadSceneAsync("PipeRotationPuzzle_CAR");
        loopGameManager.ExitMinigame();
        loopGameObject.SetActive(false);
        vehicle.Repair();
    }
}