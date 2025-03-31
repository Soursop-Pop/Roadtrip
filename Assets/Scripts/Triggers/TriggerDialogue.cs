using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public GameObject dialogueManager;

    void OnTriggerEnter(Collider other)
    {
        dialogueManager.SetActive(true);
    }
}
