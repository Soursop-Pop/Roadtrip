using System.Collections.Generic;
using Ink;
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
        else if (!story.canContinue)
        {
            waitingForChoice = false;
            endOfStory = true;
            Debug.Log("The end.");
            // Optionally, force a UI refresh or clear the dialogue UI:
            //dialogueParentObj.SetActive(false);
            //buttonParentObj.SetActive(false);
        }

        else
        {
            Debug.Log("Error!");
        }
    }

    public void ParseLine(string line)
    {
        int colonIndex = line.IndexOf(": ");
        if (colonIndex == -1)
        {
            // Handle the case where there's no "Speaker: Dialogue" format
            Debug.LogError("No colon found in line: " + line);
            currentSpeakerName = "Unknown";
            currentDialogue = line;
            return;
        }

        currentSpeakerName = line.Substring(0, colonIndex);
        currentDialogue = line.Substring(colonIndex + 2);
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
        else
        {
            buttonOneText = "";
        }
        if (choiceCount > 1)
        {
            string text = choices[1].text;
            int colonIndex = text.IndexOf(": ");
            buttonTwoText = (colonIndex != -1) ? text.Substring(colonIndex + 2) : text;
        }
        else
        {
            buttonTwoText = "";
        }
        if (choiceCount > 2)
        {
            string text = choices[2].text;
            int colonIndex = text.IndexOf(": ");
            buttonThreeText = (colonIndex != -1) ? text.Substring(colonIndex + 2) : text;
        }
        else
        {
            buttonThreeText = "";
        }
        if (choiceCount > 3)
        {
            string text = choices[3].text;
            int colonIndex = text.IndexOf(": ");
            buttonFourText = (colonIndex != -1) ? text.Substring(colonIndex + 2) : text;
        }
        else
        {
            buttonFourText = "";
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

    public void ClickedChoiceOne()
    {
        if (story.currentChoices.Count > 0)
        {
            story.ChooseChoiceIndex(0);
            waitingForChoice = false;
            DisplayDialogue();
            // Force UI refresh right away:
            DialogueGameManager manager = FindFirstObjectByType<DialogueGameManager>();
            if (manager != null)
            {
                manager.RefreshDialogueUI();
            }
        }
    }


    public void ClickedChoiceTwo()
    {
        if (story.currentChoices.Count > 1)
        {
            story.ChooseChoiceIndex(1);
            waitingForChoice = false;
            DisplayDialogue();
            // Force UI refresh right away:
            DialogueGameManager manager = FindFirstObjectByType<DialogueGameManager>();
            if (manager != null)
            {
                manager.RefreshDialogueUI();
            }
        }
    }

    public void ClickedChoiceThree()
    {
        if (story.currentChoices.Count > 2)
        {
            story.ChooseChoiceIndex(2);
            waitingForChoice = false;
            DisplayDialogue();
            // Force UI refresh right away:
            DialogueGameManager manager = FindFirstObjectByType<DialogueGameManager>();
            if (manager != null)
            {
                manager.RefreshDialogueUI();
            }
        }
    }

    public void ClickedChoiceFour()
    {
        if (story.currentChoices.Count > 2)
        {
            story.ChooseChoiceIndex(3);
            waitingForChoice = false;
            DisplayDialogue();
            // Force UI refresh right away:
            DialogueGameManager manager = FindFirstObjectByType<DialogueGameManager>();
            if (manager != null)
            {
                manager.RefreshDialogueUI();
            }
        }
    }

}