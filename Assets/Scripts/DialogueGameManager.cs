using Ink.Runtime;
using TMPro;
using UnityEngine;

public class DialogueGameManager : MonoBehaviour
{
    public DialogueInkParser inkParser;
    public TextAsset inkAsset;

    public string currentLine = "";
    public string currentSpeaker = "";

    public GameObject dialogueParentObj;
    public GameObject characterNameObj;
    public GameObject dialogueTextObj;

    public string playerName = "Player";
    public string npcName = "Chattan";

    public GameObject playerDialogueObj;
    public GameObject npcDialogueObj;

    public GameObject buttonParentObj;
    public GameObject buttonChoiceOneObj;
    public GameObject buttonChoiceTwoObj;
    public GameObject buttonChoiceThreeObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inkParser.story = new Story(inkAsset.text);
        inkParser.displayDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBubblePosition();
        UpdateStory();
        
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            inkParser.displayDialogue();
        }
    }

    void UpdateStory() {
        if (inkParser.story.canContinue) {
            dialogueParentObj.SetActive(true);
            buttonParentObj.SetActive(false);

            currentSpeaker = inkParser.currentSpeakerName;
            currentLine = inkParser.currentDialogue;

            Debug.Log(characterNameObj.GetComponent<TMP_Text>());
            characterNameObj.GetComponent<TMP_Text>().text = currentSpeaker;
            dialogueTextObj.GetComponent<TMP_Text>().text = currentLine;
        }
        else {
            if (inkParser.waitingForChoice) {
                dialogueParentObj.SetActive(false);
                buttonParentObj.SetActive(true);
                
                //currentSpeaker = inkParser.currentSpeakerName;
                buttonChoiceOneObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonOneText;
                buttonChoiceTwoObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonTwoText;
                buttonChoiceThreeObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonThreeText;
            }
            else if (inkParser.endOfStory) {
                dialogueParentObj.SetActive(false);
            }
            else {
                characterNameObj.GetComponent<TMP_Text>().text = inkParser.currentSpeakerName;
                dialogueTextObj.GetComponent<TMP_Text>().text = inkParser.currentDialogue;
            }
        }
    }

    void UpdateBubblePosition() {
        if (currentSpeaker == playerName) {
            playerDialogueObj.SetActive(true);
            npcDialogueObj.SetActive(false);

            characterNameObj = GameObject.Find("Player Name Text");
            dialogueTextObj = GameObject.Find("Player Text");
        }
        else if (currentSpeaker == npcName) {
            playerDialogueObj.SetActive(false);
            npcDialogueObj.SetActive(true);

            characterNameObj = GameObject.Find("NPC Name Text");
            dialogueTextObj = GameObject.Find("NPC Text");
        }
    }
}
