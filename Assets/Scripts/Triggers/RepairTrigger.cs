using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RepairTrigger : MonoBehaviour
{
    public VehicleController vehicle;
    private bool playerNearby = false;
    public LoopGameManager loopGameManager;
    public GameObject loopGameObject;
    public bool engineMiniGameRunning = false;
    public bool engineMingameStarted = false;

    public int puzzleDifficulty = 81;


    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.F))
        {
            loopGameManager.GeneratePuzzle();
            loopGameObject.SetActive(true);
            //StartEngineMinigame(puzzleDifficulty);
            if (loopGameObject.activeInHierarchy == true)
            {
                engineMingameStarted = true;
            }
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            UIManager.ShowRepairPrompt(true); // Show UI "Press F to Fix"
        }
        else
        {
            {
                UIManager.ShowRepairPrompt(false);
            }
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

    public void StartEngineMinigame(int puzzleDifficulty)
    {
        //SceneManager.LoadScene("PipeRotationPuzzle_CAR", LoadSceneMode.Additive);

        //loopGameManager.SetupPuzzleForDifficulty(puzzleDifficulty);
        //loopGameManager.puzzle.difficulty = this.puzzleDifficulty;
        loopGameManager.puzzle.height = (int)Mathf.Sqrt(puzzleDifficulty);
        loopGameManager.puzzle.width = (int)Mathf.Sqrt(puzzleDifficulty);
        loopGameObject.SetActive(true);
        engineMiniGameRunning = true;
        //loopGameObject.GetComponent<CinemachineCamera>().Priority = 20;


    }

    public void EndEngineMinigame()
    {
        //SceneManager.UnloadSceneAsync("PipeRotationPuzzle_CAR");
        loopGameManager.ExitMinigame();
        loopGameObject.SetActive(false);
        vehicle.Repair();
        engineMiniGameRunning = false;
        engineMingameStarted = false;
    }
}