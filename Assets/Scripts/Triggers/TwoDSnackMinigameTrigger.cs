using UnityEngine;
using UnityEngine.SceneManagement;

public class TwoDSnackMinigameTrigger : MonoBehaviour
{
    public string sceneName;

    private bool minigameOpen = false;
    private bool inTrigger = false;

    public bool minigameComplete = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger && !minigameOpen && !minigameComplete && Input.GetKeyDown(KeyCode.F)) {
            minigameOpen = true;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        else if (minigameComplete) {
            minigameOpen = false;
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    void OnTriggerEnter(Collider other) {
        inTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
}
