using FMOD.Studio;
using FMODUnity;
using Ink.Runtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueGameManager : MonoBehaviour
{
    public DialogueInkParser inkParser;
    public TextAsset inkAsset;

    public string currentLine = "";
    public string currentSpeaker = "";
    public string currentEmotion = "";
    public string soundEmotion = "";
    public Sprite currentEmotionSprite = null;

    private string lastChirpSpeaker = "";
    private string lastChirpEmotion = "";

    public GameObject dialogueParentObj;
    public Transform dialogueParentTransform;

    public GameObject characterNameObj;
    public GameObject dialogueTextObj;
    public GameObject emotionSpriteObj;

    //public string playerName = "Rhodes";
    public string npcName = "Paige";
    public TMP_Text npcNameTextField;
    public TMP_Text npcDialogueTextField;
    public Image npcEmotionImage;

    //public GameObject playerDialogueObj;
    public GameObject npcDialogueObj;
    //public GameObject playerEmotionSprite;
    public GameObject npcEmotionSprite;

    // BUTTON-RELATED
    public GameObject buttonParentObj;
    public Transform buttonParentTransform;

    public GameObject buttonChoiceOneObj;
    public TMP_Text buttonChoiceOneTextField;

    public GameObject buttonChoiceTwoObj;
    public TMP_Text buttonChoiceTwoTextField;

    public GameObject buttonChoiceThreeObj;
    public TMP_Text buttonChoiceThreeTextField;

    public GameObject buttonChoiceFourObj;
    public TMP_Text buttonChoiceFourTextField;
    
    public bool makingChoices = false;

    // FMOD
    public FMODUnity.EventReference fmodEvent;
    private bool hasPlayedChirp = false;

    void Start()
    {
        Debug.Log("DialogueGameManager Start() called. Initializing story with default inkAsset.");
        if (inkAsset != null)
        {
            inkParser.story = new Story(inkAsset.text);
        }
        else
        {
            Debug.LogWarning("inkAsset is null at Start!");
        }
        // Do not auto-display dialogue here to avoid initial double triggering.
        // inkParser.DisplayDialogue();
    }

    void Update()
    {
        if (inkAsset != null)
        {
            UpdateBubblePosition();
            UpdateStory();

            // This click-advance mechanism works for both town and driving scenes once the dialogue is active.
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Mouse0 pressed. Advancing dialogue...");
                inkParser.DisplayDialogue();
            }
        }
    }

    void UpdateStory()
    {
        Debug.Log("UpdateStory called. canContinue: " + inkParser.story.canContinue +
                  ", waitingForChoice: " + inkParser.waitingForChoice +
                  ", endOfStory: " + inkParser.endOfStory);

        if (inkParser.story.canContinue)
        {
            //dialogueParentObj.SetActive(true);
            //buttonParentObj.SetActive(false);
            makingChoices = false;

            currentSpeaker = inkParser.currentSpeakerName;
            currentLine = inkParser.currentDialogue;
            currentEmotionSprite = inkParser.currentEmotionSprite;

            Debug.Log("Displaying dialogue line: " + currentLine + " from " + currentSpeaker);
            if (characterNameObj != null) characterNameObj.GetComponent<TMP_Text>().text = currentSpeaker;
            dialogueTextObj.GetComponent<TMP_Text>().text = currentLine;
            if (emotionSpriteObj != null)
            {
                emotionSpriteObj.GetComponent<Image>().sprite = currentEmotionSprite;
            }
            else
            {
                Debug.LogWarning("emotionSpriteObj is not assigned!");
            }

            currentEmotion = inkParser.currentEmotion;

            if (currentSpeaker != lastChirpSpeaker || currentEmotion != lastChirpEmotion)
            {
                if (currentEmotion == "anger" || currentEmotion == "bored" || currentEmotion == "confusion" || currentEmotion == "neutral")
                {
                    soundEmotion = "neutral";
                }
                else if (currentEmotion == "chipper" || currentEmotion == "excited")
                {
                    soundEmotion = "happy";
                }
                else
                {
                    soundEmotion = "sad";
                }

                if (soundEmotion == "happy" || soundEmotion == "neutral" || soundEmotion == "sad")
                {
                    Debug.Log("Playing chirp for speaker: " + currentSpeaker + " with mood: " + soundEmotion);
                    PlayChirp(currentSpeaker, soundEmotion);

                    lastChirpSpeaker = currentSpeaker;
                    lastChirpEmotion = currentEmotion;
                }
            }
        }
        else
        {
            makingChoices = true;
            hasPlayedChirp = false;

            if (inkParser.waitingForChoice)
            {
                dialogueParentObj.SetActive(false);
                buttonParentObj.SetActive(true);

                int choiceCount = inkParser.story.currentChoices.Count;
                Debug.Log("Waiting for choice. Available choices count: " + choiceCount);

                if (choiceCount > 0)
                {
                    Debug.Log("Setting button 1 text: " + inkParser.buttonOneText);
                    buttonChoiceOneObj.SetActive(true);
                    buttonChoiceOneObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonOneText;
                }
                else
                {
                    buttonChoiceOneObj.SetActive(false);
                    Debug.Log("Button 1 deactivated due to no choice available.");
                }
                if (choiceCount > 1)
                {
                    Debug.Log("Setting button 2 text: " + inkParser.buttonTwoText);
                    buttonChoiceTwoObj.SetActive(true);
                    buttonChoiceTwoObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonTwoText;
                }
                else
                {
                    buttonChoiceTwoObj.SetActive(false);
                    Debug.Log("Button 2 deactivated due to less than 2 choices available.");
                }
                if (choiceCount > 2)
                {
                    Debug.Log("Setting button 3 text: " + inkParser.buttonThreeText);
                    buttonChoiceThreeObj.SetActive(true);
                    buttonChoiceThreeObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonThreeText;
                }
                else
                {
                    buttonChoiceThreeObj.SetActive(false);
                    Debug.Log("Button 3 deactivated due to less than 3 choices available.");
                }
                if (choiceCount > 3)
                {
                    Debug.Log("Setting button 4 text: " + inkParser.buttonFourText);
                    buttonChoiceFourObj.SetActive(true);
                    buttonChoiceFourObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonFourText;
                }
                else
                {
                    buttonChoiceFourObj.SetActive(false);
                    Debug.Log("Button 4 deactivated due to less than 4 choices available.");
                }
            }
            else if (inkParser.endOfStory)
            {
                makingChoices = false;
                dialogueParentObj.SetActive(false);
                Debug.Log("End of story reached.");
            }
            else
            {
                if (characterNameObj != null)
                    characterNameObj.GetComponent<TMP_Text>().text = inkParser.currentSpeakerName;
                else
                    Debug.LogError("characterNameObj is null when trying to set text!");

                if (dialogueTextObj != null)
                    dialogueTextObj.GetComponent<TMP_Text>().text = inkParser.currentDialogue;
                else
                    Debug.LogError("dialogueTextObj is null when trying to set text!");

                Debug.Log("Updating dialogue without choices. Speaker: " + inkParser.currentSpeakerName +
                          ", Dialogue: " + inkParser.currentDialogue);
            }
        }
    }

    void UpdateBubblePosition()
    {
        // if (currentSpeaker == playerName)
        // {
        //     playerDialogueObj.SetActive(true);
        //     npcDialogueObj.SetActive(false);

        //     characterNameObj = GameObject.Find("Player Name Text");
        //     dialogueTextObj = GameObject.Find("Player Text");
        //     emotionSpriteObj = GameObject.Find("Player Expression Sprite");

        //     if (characterNameObj == null)
        //         Debug.LogError("Player Name Text not found in scene!");
        //     if (dialogueTextObj == null)
        //         Debug.LogError("Player Text not found in scene!");

        //     Debug.Log("Dialogue bubble positioned for player.");
        // }
        // if (currentSpeaker == npcName)
        // {
        //     //playerDialogueObj.SetActive(false);
        //     npcDialogueObj.SetActive(true);

        //     characterNameObj = GameObject.Find("NPC Name Text");
        //     dialogueTextObj = GameObject.Find("NPC Text");
        //     emotionSpriteObj = GameObject.Find("NPC Expression Sprite");

        //     // if (characterNameObj == null)
        //     //     Debug.LogError("NPC Name Text not found in scene!");
        //     // if (dialogueTextObj == null)
        //     //     Debug.LogError("NPC Text not found in scene!");

        //     Debug.Log("Dialogue bubble positioned for NPC: " + npcName);
        // }

        // CREATE VAR FOR POSITION???

        // if (makingChoices) {
        //     buttonParentTransform.Translate(Vector3.zero);
        //     dialogueParentTransform.Translate(new Vector3(0, 1000, 0));
        // }
        // else if (!makingChoices) {
        //     buttonParentTransform.Translate(new Vector3(0, 1000, 0));
        //     dialogueParentTransform.Translate(Vector3.zero);
        // }
    }

    public void PlayChirp(string character, string mood)
    {
        var characterDict = new Dictionary<string, float>()
        {
            { "Paige", 0f },
            { "August", 1f },
            { "Ayesha", 2f },
        };

        var moodDict = new Dictionary<string, float>()
        {
            { "neutral", 0f },
            { "happy", 1f },
            { "sad", 2f },
        };

        if (characterDict.TryGetValue(character, out float charVal) &&
            moodDict.TryGetValue(mood, out float moodVal))
        {
            Debug.Log("Playing chirp with FMOD. Character value: " + charVal + ", Mood value: " + moodVal);
            EventInstance chirp = RuntimeManager.CreateInstance(fmodEvent);
            chirp.setParameterByName("Character", charVal);
            chirp.setParameterByName("Mood", moodVal);
            chirp.start();
            chirp.release();
        }
        else
        {
            Debug.LogWarning("Failed to find FMOD parameters for character: " + character + " or mood: " + mood);
        }
    }

    public void ResetDialogue()
    {
        Debug.Log("ResetDialogue called. Reinitializing story with new inkAsset: " + (inkAsset != null ? inkAsset.name : "null"));
        if (inkAsset != null)
        {
            inkParser.story = new Story(inkAsset.text);
            inkParser.waitingForChoice = false;
            inkParser.endOfStory = false;
        }
        else
        {
            Debug.LogError("ResetDialogue failed because inkAsset is null!");
        }

        // Hide dialogue UI elements.
        if (dialogueParentObj != null)
        {
            dialogueParentObj.SetActive(false);
        }
        if (buttonParentObj != null)
        {
            buttonParentObj.SetActive(false);
        }
    }

    // PUBLIC helper so we can force the UI update immediately after starting dialogue.
    public void RefreshDialogueUI()
    {
        UpdateStory();
    }
}
