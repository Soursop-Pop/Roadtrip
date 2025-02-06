using Ink.Runtime;
using TMPro;
using UnityEngine;

public class DialogueGameManager : MonoBehaviour
{
    public DialogueInkParser inkParser;
    public TextAsset inkAsset;

    public GameObject playerBubbleAnchor;
    public GameObject npcBubbleAnchor;

    public string currentLine;
    public string currentSpeaker;

    public GameObject dialogueParentObj;
    public GameObject characterNameObj;
    public GameObject dialogueTextObj;

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
        UpdateStory();
        //UpdateBubblePosition();
        if (Input.GetKeyDown(KeyCode.E)) {
            inkParser.displayDialogue();
        }
    }

    void UpdateStory() {
        if (inkParser.story.canContinue) {
            dialogueParentObj.SetActive(true);
            buttonParentObj.SetActive(false);

            currentSpeaker = inkParser.currentSpeakerName;
            currentLine = inkParser.currentDialogue;
            characterNameObj.GetComponent<TMP_Text>().text = currentSpeaker;
            dialogueTextObj.GetComponent<TMP_Text>().text = currentLine;
        }
        else {
            if (inkParser.waitingForChoice) {
                dialogueParentObj.SetActive(false);
                buttonParentObj.SetActive(true);
                
                currentSpeaker = inkParser.currentSpeakerName;
                buttonChoiceOneObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonOneText;
                buttonChoiceTwoObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonTwoText;
                buttonChoiceThreeObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonThreeText;
            }
            if (inkParser.endOfStory) {
                dialogueParentObj.SetActive(false);
            }
            else {
                characterNameObj.GetComponent<TMP_Text>().text = inkParser.currentSpeakerName;
                dialogueTextObj.GetComponent<TMP_Text>().text = inkParser.currentDialogue;
            }
        }
    }

    void UpdateBubblePosition() {
        if (currentSpeaker == "Player") {
            dialogueParentObj.transform.position = new Vector3(-1,3,0);
        }
        else {
            dialogueParentObj.transform.position =  new Vector3(-1,1,0);
        }
    }
}
