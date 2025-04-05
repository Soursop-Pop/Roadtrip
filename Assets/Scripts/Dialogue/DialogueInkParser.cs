using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class DialogueInkParser : MonoBehaviour
{
    public Story story;
    public bool waitingForChoice = false;
    public bool endOfStory = false;

    public string currentLine = "";
    public string currentSpeakerName = "";
    public string currentDialogue = "";

    public string buttonOneText = "";
    public string buttonTwoText = "";
    public string buttonThreeText = "";
    public string buttonFourText = "";

    public Sprite[] emotionSprites;
    public Sprite currentEmotionSprite;
    public string currentEmotion;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayDialogue() {
        if (story.canContinue && story.currentChoices.Count <= 0) { // if story is not over & no choices
            waitingForChoice = false;
            currentLine = story.Continue(); // grab line of text
            ParseLine(currentLine);
            Debug.Log("Current Speaker: " + currentSpeakerName);
            Debug.Log(currentDialogue);

            if (story.currentTags.Count > 0) {
                ParseEmotionIcon(story.currentTags);
            }
        }
        else if (story.currentChoices.Count > 0) { // if there are choices
            ParseButtonLines(story.currentChoices);
            waitingForChoice = true;
            Debug.Log("Waiting for choice");
        }
        else if (!story.canContinue) {
            waitingForChoice = false;
            endOfStory = true;
            Debug.Log("The end.");
        }
        else {
            Debug.Log("Error!");
        }
    }

    public void ParseLine(string line) {
        // Parse out speaker
        int colonIndex = line.IndexOf(": ");
        currentSpeakerName = currentLine.Substring(0, colonIndex);
        currentDialogue = currentLine.Substring(colonIndex + 2);
    }

    public void ParseButtonLines(List<Choice> choices) {
        int colonIndex = -1;

        buttonOneText = story.currentChoices[0].text;
        colonIndex = buttonOneText.IndexOf(": ");
        currentSpeakerName = buttonOneText.Substring(0, colonIndex); // should all be the same speaker
        buttonOneText = buttonOneText.Substring(colonIndex + 2);

        buttonTwoText = story.currentChoices[1].text;
        colonIndex = buttonTwoText.IndexOf(": ");
        buttonTwoText = buttonTwoText.Substring(colonIndex + 2);

        buttonThreeText = story.currentChoices[2].text;
        colonIndex = buttonThreeText.IndexOf(": ");
        buttonThreeText = buttonThreeText.Substring(colonIndex + 2);

        buttonFourText = story.currentChoices[3].text;
        colonIndex = buttonFourText.IndexOf(": ");
        buttonFourText = buttonFourText.Substring(colonIndex + 2);
    }
    
    public void ParseEmotionIcon(List<string> tags) {
        switch (tags[0]) {
            case "confused":
                currentEmotionSprite = emotionSprites[0];
                currentEmotion = tags[0];
                break;
            case "shocked":
                currentEmotionSprite = emotionSprites[1];
                currentEmotion = tags[0];
                break;
            case "sad":
                currentEmotionSprite = emotionSprites[2];
                currentEmotion = tags[0];
                break;
            case "happy":
                currentEmotionSprite = emotionSprites[3];
                currentEmotion = tags[0];
                break;
            case "annoyed":
                currentEmotionSprite = emotionSprites[4];
                currentEmotion = tags[0];
                break;
            case "angry":
                currentEmotionSprite = emotionSprites[5];
                currentEmotion = tags[0];
                break;
            case "love":
                currentEmotionSprite = emotionSprites[6];
                currentEmotion = tags[0];
                break;
        }
    }

    public void ClickedChoiceOne() {
        story.ChooseChoiceIndex(0);
        waitingForChoice = false;
        DisplayDialogue();
    }

    public void ClickedChoiceTwo() {
        story.ChooseChoiceIndex(1);
        waitingForChoice = false;
        DisplayDialogue();
    }

    public void ClickedChoiceThree() {
        story.ChooseChoiceIndex(2);
        waitingForChoice = false;
        DisplayDialogue();
    }

    public void ClickedChoiceFour() {
        story.ChooseChoiceIndex(3);
        waitingForChoice = false;
        DisplayDialogue();
    }
}