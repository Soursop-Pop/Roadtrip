using UnityEngine;

public class TwoDGasMinigameTrigger : MonoBehaviour
{
    public GameObject minigameObj;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter() {
        minigameObj.SetActive(true);
    }

    void OnTriggerExit() {
        minigameObj.SetActive(false);
    }
}
