using UnityEngine;
using TMPro; // Ensure TextMeshPro is imported

public class TownNPCDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public GameObject dialogueManager;
    public DialogueGameManager dialogueGameManager;
    public TextAsset npcDialogueFile;

    [Header("In-World UI")]
    // The canvas that appears above the NPC to prompt interaction.
    public GameObject pressFCanvas;
    // The TextMeshProUGUI component on the canvas.
    public TMP_Text pressFText;

    private bool inTrigger = false;

    private void Start()
    {
        // Get the dialogue manager component if not already set.
        if (dialogueManager != null)
        {
            dialogueGameManager = dialogueManager.GetComponent<DialogueGameManager>();
        }
        else
        {
            Debug.LogError("Dialogue Manager is not assigned in " + this.gameObject.name);
        }

        // Ensure the prompt canvas is hidden at start.
        if (pressFCanvas != null)
        {
            pressFCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("pressFCanvas is not assigned in " + this.gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
            Debug.Log("Entered trigger of " + this.gameObject.name);

            // Only show the press F canvas if no conversation is active.
            if (dialogueManager != null && !dialogueManager.activeSelf && pressFCanvas != null)
            {
                pressFCanvas.SetActive(true);
                if (pressFText != null)
                {
                    pressFText.text = "Press F to talk " + this.gameObject.name;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
            Debug.Log("Exited trigger of " + this.gameObject.name);

            // Hide the prompt when the player leaves.
            if (pressFCanvas != null)
            {
                pressFCanvas.SetActive(false);
            }

            // End the conversation and force all dialogue UI elements off.
            if (dialogueManager != null)
            {
                dialogueManager.SetActive(false);
            }
            if (dialogueGameManager != null)
            {
                dialogueGameManager.ResetDialogue();
            }
        }
    }

    private void Update()
    {
        // If a conversation is active, ensure the prompt canvas is hidden.
        if (dialogueManager != null && dialogueManager.activeSelf && pressFCanvas != null)
        {
            if (pressFCanvas.activeSelf)
            {
                pressFCanvas.SetActive(false);
                Debug.Log("Hiding press F canvas because conversation is active.");
            }
        }

        if (inTrigger && Input.GetKeyDown(KeyCode.F))
        {
            if (dialogueManager != null)
            {
                Debug.Log("F key pressed in trigger of " + this.gameObject.name + ". Starting conversation.");
                // Activate dialogue UI.
                dialogueManager.SetActive(true);
                // Set the dialogue file and NPC name.
                dialogueGameManager.inkAsset = npcDialogueFile;
                dialogueGameManager.npcName = this.gameObject.name;

                // Reset and display the dialogue immediately.
                dialogueGameManager.ResetDialogue();
                dialogueGameManager.inkParser.DisplayDialogue();
                // Force an immediate UI update.
                dialogueGameManager.RefreshDialogueUI();

                // Hide the press F canvas once conversation starts.
                if (pressFCanvas != null)
                {
                    pressFCanvas.SetActive(false);
                }
            }
        }

    }
}
