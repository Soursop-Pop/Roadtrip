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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     dialogueManager.SetActive(true);
    //     dialogueGameManager.inkAsset = npcDialogueFile;
    //     dialogueGameManager.npcName = this.gameObject.name;
    //     dialogueManager.SetActive(false);
    // }

    void OnTriggerStay(Collider other) {
        Debug.Log("In " + this.gameObject.name + "'s trigger");
        if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F)) {
            dialogueManager.SetActive(true);
            dialogueGameManager.inkAsset = npcDialogueFile;
            dialogueGameManager.npcName = this.gameObject.name;
        }
    }

    // void OnTriggerExit(Collider other)
    // {
    //     dialogueManager.SetActive(false);
    // }
}
