using UnityEngine;

public class TownNPCDialogueTrigger : MonoBehaviour
{
    public GameObject dialogueManager;
    public DialogueGameManager dialogueGameManager;
    public TextAsset npcDialogueFile;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueGameManager = dialogueManager.GetComponent<DialogueGameManager>();
        if (dialogueManager.activeInHierarchy) {
            dialogueManager.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other) {
        Debug.Log("In " + this.gameObject.name + "'s trigger");
        if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F)) {
            dialogueManager.SetActive(true);
            dialogueGameManager.inkAsset = npcDialogueFile;
            dialogueGameManager.npcName = this.gameObject.name;
        }
    }
}
