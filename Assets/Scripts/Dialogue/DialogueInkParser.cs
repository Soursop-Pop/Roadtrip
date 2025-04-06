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

    public void ParseButtonLines(List<Choice> choices)
    {
        int choiceCount = choices.Count;

        if (choiceCount > 0)
        {
            string text = choices[0].text;
            int colonIndex = text.IndexOf(": ");
            currentSpeakerName = (colonIndex != -1) ? text.Substring(0, colonIndex) : "";
            buttonOneText = (colonIndex != -1) ? text.Substring(colonIndex + 2) : text;
        }
        if (choiceCount > 1)
        {
            string text = choices[1].text;
            int colonIndex = text.IndexOf(": ");
            buttonTwoText = (colonIndex != -1) ? text.Substring(colonIndex + 2) : text;
        }
        if (choiceCount > 2)
        {
            string text = choices[2].text;
            int colonIndex = text.IndexOf(": ");
            buttonThreeText = (colonIndex != -1) ? text.Substring(colonIndex + 2) : text;
        }
        if (choiceCount > 3)
        {
            string text = choices[3].text;
            int colonIndex = text.IndexOf(": ");
            buttonFourText = (colonIndex != -1) ? text.Substring(colonIndex + 2) : text;
        }
    }

    public void ParseEmotionIcon(List<string> tags) {
        switch (tags[0]) {
            case "anger":
                currentEmotionSprite = emotionSprites[0];
                currentEmotion = tags[0];
                break;
            case "bored":
                currentEmotionSprite = emotionSprites[1];
                currentEmotion = tags[0];
                break;
            case "chipper":
                currentEmotionSprite = emotionSprites[2];
                currentEmotion = tags[0];
                break;
            case "confusion":
                currentEmotionSprite = emotionSprites[3];
                currentEmotion = tags[0];
                break;
            case "excited":
                currentEmotionSprite = emotionSprites[4];
                currentEmotion = tags[0];
                break;
            case "neutral":
                currentEmotionSprite = emotionSprites[5];
                currentEmotion = tags[0];
                break;
            case "sad":
                currentEmotionSprite = emotionSprites[6];
                currentEmotion = tags[0];
                break;
            case "shock":
                currentEmotionSprite = emotionSprites[7];
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