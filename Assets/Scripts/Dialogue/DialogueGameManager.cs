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
    public GameObject characterNameObj;
    public GameObject dialogueTextObj;
    public GameObject emotionSpriteObj;

    public string playerName = "Rhodes";
    public string npcName = "Paige";

    public GameObject playerDialogueObj;
    public GameObject npcDialogueObj;

    public GameObject buttonParentObj;
    public GameObject buttonChoiceOneObj;
    public GameObject buttonChoiceTwoObj;
    public GameObject buttonChoiceThreeObj;
    public GameObject buttonChoiceFourObj;

    public GameObject playerEmotionSprite;
    public GameObject npcEmotionSprite;

    //FMOD
    public FMODUnity.EventReference fmodEvent;
    private bool hasPlayedChirp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inkParser.story = new Story(inkAsset.text);
        inkParser.DisplayDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBubblePosition();
        UpdateStory();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            inkParser.DisplayDialogue();
        }
    }

    void UpdateStory()
    {
        if (!inkParser.story.canContinue)
        {
            dialogueParentObj.SetActive(true);
            buttonParentObj.SetActive(false);

            currentSpeaker = inkParser.currentSpeakerName;
            currentLine = inkParser.currentDialogue;
            currentEmotionSprite = inkParser.currentEmotionSprite;

            characterNameObj.GetComponent<TMP_Text>().text = currentSpeaker;
            dialogueTextObj.GetComponent<TMP_Text>().text = currentLine;
            emotionSpriteObj.GetComponent<Image>().sprite = currentEmotionSprite;

            currentEmotion = inkParser.currentEmotion;

            // Only play chirp when emotion or speaker changes
            if (currentSpeaker != lastChirpSpeaker || currentEmotion != lastChirpEmotion)
            {
                if (currentEmotion == "anger" || currentEmotion == "bored" || currentEmotion == "confusion" ||  currentEmotion == "neutral")
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

                if (soundEmotion == "happy" || soundEmotion == "neutral" ||  soundEmotion == "sad")
                {
                    PlayChirp(currentSpeaker, soundEmotion);

                    // Save last played combo
                    lastChirpSpeaker = currentSpeaker;
                    lastChirpEmotion = currentEmotion;
                }
            }
        }
        else
        {
            hasPlayedChirp = false;

            if (inkParser.waitingForChoice)
            {
                dialogueParentObj.SetActive(false);
                buttonParentObj.SetActive(true);

                currentSpeaker = inkParser.currentSpeakerName;
                
                buttonChoiceOneObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonOneText;
                buttonChoiceTwoObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonTwoText;
                buttonChoiceThreeObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonThreeText;
                buttonChoiceFourObj.GetComponentInChildren<TMP_Text>().text = inkParser.buttonFourText;
            }
            else if (inkParser.endOfStory)
            {
                dialogueParentObj.SetActive(false);
            }
            else
            {
                characterNameObj.GetComponent<TMP_Text>().text = inkParser.currentSpeakerName;
                dialogueTextObj.GetComponent<TMP_Text>().text = inkParser.currentDialogue;
            }
        }
    }


    void UpdateBubblePosition()
    {
        if (currentSpeaker == playerName)
        {
            playerDialogueObj.SetActive(true);
            npcDialogueObj.SetActive(false);

            characterNameObj = GameObject.Find("Player Name Text");
            dialogueTextObj = GameObject.Find("Player Text");
            emotionSpriteObj = GameObject.Find("Player Expression Sprite");
        }
        else if (currentSpeaker == npcName)
        {
            playerDialogueObj.SetActive(false);
            npcDialogueObj.SetActive(true);

            characterNameObj = GameObject.Find("NPC Name Text");
            dialogueTextObj = GameObject.Find("NPC Text");
            emotionSpriteObj = GameObject.Find("NPC Expression Sprite");
        }
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
            EventInstance chirp = RuntimeManager.CreateInstance(fmodEvent);
            chirp.setParameterByName("Character", charVal);
            chirp.setParameterByName("Mood", moodVal);
            chirp.start();
            chirp.release();
        }
    }


}
