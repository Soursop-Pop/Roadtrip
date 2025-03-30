// This script assumes you have a DialogueManager, MinigameController, and NPCController elsewhere in your project.
using UnityEngine;
using System.Collections;

public class SequenceController : MonoBehaviour
{
    public enum StoryStep
    {
        None,
        CarBreakdown,
        NPCWalksUp,
        NPCIntroDialogue,
        Minigame1,
        Minigame1FailDialogue,
        Minigame2,
        NPCWrapUpDialogue,
        GetInCar,
        DriveOff
    }

    public StoryStep currentStep = StoryStep.CarBreakdown;

    public DialogueGameManager dialogueManager;
    public MinigameManager minigameController;
    public NPCPathFollower npcController;

    public TextAsset paige_walking_up;

    public bool breakdown = false;

    void Start()
    {
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        yield return new WaitForSeconds(1f); // initial delay

        if (breakdown)
        {
            // Step 1: Car breakdown
            Debug.Log("Step 1: Car breakdown");
            // Play VFX/SFX 
            yield return new WaitForSeconds(2f);
        }

        currentStep = StoryStep.NPCWalksUp;
        Debug.Log("Step 2: NPC walks up");
        npcController.StartMovement();
        yield return new WaitUntil(() => !npcController.isMoving);

        currentStep = StoryStep.NPCIntroDialogue;
        Debug.Log("Step 3: NPC intro dialogue");
        yield return StartCoroutine(LoadAndPlayInkStory(paige_walking_up));


        //currentStep = StoryStep.Minigame1;
        //Debug.Log("Step 4: Starting hard minigame");
        //minigameController.StartMinigame(difficulty: 10);
        //yield return new WaitUntil(() => minigameController.PlayerFailed);

        //currentStep = StoryStep.Minigame1FailDialogue;
        //Debug.Log("Step 5: NPC talks while you fail");
        //yield return dialogueManager.PlayDialogue("Don’t worry, that was a tough one.");

        //currentStep = StoryStep.Minigame2;
        //Debug.Log("Step 6: Starting easier minigame");
        //minigameController.StartMinigame(difficulty: 3);
        //yield return new WaitUntil(() => minigameController.PlayerSucceeded);

        //currentStep = StoryStep.NPCWrapUpDialogue;
        //Debug.Log("Step 7: Final NPC dialogue");
        //yield return dialogueManager.PlayDialogue("Nice! You got it this time. Let’s get going.");

        //currentStep = StoryStep.GetInCar;
        //Debug.Log("Step 8: Getting in car");
        //npcController.EnterCar();
        //yield return new WaitForSeconds(2f);

        //currentStep = StoryStep.DriveOff;
        //Debug.Log("Step 9: Driving off and chatting");
        //dialogueManager.PlayDialogue("So, where are we headed?");
        //// Fade out or begin driving camera
    }


    IEnumerator LoadAndPlayInkStory(TextAsset inkFile)
    {
        dialogueManager.inkAsset = inkFile;
        dialogueManager.inkParser.story = new Ink.Runtime.Story(inkFile.text);
        dialogueManager.inkParser.DisplayDialogue();

        // Wait for the Ink story to finish
        yield return new WaitUntil(() => dialogueManager.inkParser.endOfStory);
    }

}

