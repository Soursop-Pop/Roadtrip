using Ink;
using Ink.Runtime;
using UnityEngine;

public class TownNPCDialogueTrigger : MonoBehaviour
{
    public GameObject dialogueManager;
    public DialogueGameManager dialogueGameManager;
    public TextAsset npcDialogueFile;
    

    public bool inTrigger = false;

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


    //alex previous ontrigger stay
    //void OnTriggerStay(Collider other) {
    //    Debug.Log("In " + this.gameObject.name + "'s trigger");
    //    if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F)) {
    //        dialogueManager.SetActive(true);
    //        dialogueGameManager.inkAsset = npcDialogueFile;
    //        dialogueGameManager.npcName = this.gameObject.name;
    //    }
    //}


    //marty attempt to fix on trigger stay - we are doing this because convo only tiggers for first character in a scene

    void OnTriggerStay(Collider other)
    {
        
        inTrigger = true;
        Debug.Log("In " + this.gameObject.name + "'s trigger");
        if (other.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.F) && inTrigger)
        {
            dialogueManager.SetActive(true);
            dialogueGameManager.inkAsset = npcDialogueFile;
            dialogueGameManager.npcName = this.gameObject.name;

            // Reinitialize the Ink story with the new dialogue file:
            //dialogueGameManager.inkParser.story = new Story(npcDialogueFile.text);
            //dialogueGameManager.inkParser.waitingForChoice = false;
            //dialogueGameManager.inkParser.endOfStory = false;

            dialogueGameManager.ResetDialogue();
            dialogueGameManager.inkParser.DisplayDialogue();

        }

    }


    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
        //dialogueGameManager.ResetDialogue();
        dialogueManager.SetActive(false);
    }
}
