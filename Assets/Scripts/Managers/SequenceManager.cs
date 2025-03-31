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
        HardMinigame,
        EasyMinigame,
        NPCWrapUpDialogue,
        GetInCar,
        DriveOff
    }

    public StoryStep currentStep = StoryStep.CarBreakdown;

    public DialogueGameManager dialogueManager;
    public MinigameManager minigameManager;
    public NPCPathFollower npcController;
    public LoopGameManager loopGameManager;
    public VehicleController vehicleController;

    // Example dialogue ink asset (for NPC explanation)
    public TextAsset paige_walking_up;


    void Start()
    {
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {

        // Step 1: Car breakdown (optional)
        yield return new WaitUntil(() => vehicleController.isBrokenDown);
        Debug.Log("Step 1: Car breakdown");
        // Play any car breakdown VFX/SFX here.
        yield return new WaitForSeconds(2f);


        // Step 2: NPC walks up if triggered
        yield return new WaitUntil(() => npcController.startOnTrigger);
        currentStep = StoryStep.NPCWalksUp;
        Debug.Log("Step 2: NPC walks up");
        npcController.StartMovement();
        yield return new WaitUntil(() => !npcController.isMoving);
        Debug.Log("Paige is Done Moving");
        

        
        currentStep = StoryStep.HardMinigame;
        // === Hard Minigame Phase ===
        // Set up a hard puzzle; e.g., a difficulty value of 20 yields roughly a 5x5 puzzle.
        //loopGameManager.SetupPuzzleForDifficulty(20);
        //play puzzle for a few seconds
        // Disable piece interaction while the NPC explains the puzzle.
        //loopGameManager.SetPuzzleInteractable(false);
        // Assume the minigame is already loaded externally.
        // Set the state to HardMinigame.
        Debug.Log("Step 3: Hard minigame running");
        // (Your hard minigame would be running here.
        //  You might check for failure or simply use the puzzle’s difficulty as a condition.)

        // When a hard puzzle is detected (e.g. difficulty > threshold), interrupt with dialogue.
        if (loopGameManager.puzzle.difficulty > 12)
        {
            // Pause player input during dialogue (already disabled above).
            currentStep = StoryStep.NPCIntroDialogue;
            Debug.Log("Step 4: NPC intro dialogue (explaining hard puzzle)");
            yield return StartCoroutine(LoadAndPlayInkStory(paige_walking_up));

            // End the current (hard) minigame.
            minigameManager.EndMinigame();

            // Re-enable puzzle interaction (if needed for the next phase).
            loopGameManager.SetPuzzleInteractable(true);



            currentStep = StoryStep.EasyMinigame;
            // === Transition to Easy Minigame Phase ===
            // Set up an easier puzzle; e.g., a difficulty value of 9 yields a 3x3 puzzle.
            loopGameManager.SetupPuzzleForDifficulty(9);
            Debug.Log("Step 5: Easy minigame running");
            // Optionally, wait until the player succeeds.
            // yield return new WaitUntil(() => minigameManager.PlayerSucceeded);
        }

        // Continue with further dialogue or game steps...
        currentStep = StoryStep.NPCWrapUpDialogue;
        Debug.Log("Step 6: NPC wrap-up dialogue");
        yield return StartCoroutine(LoadAndPlayInkStory(paige_walking_up)); // Or another dialogue asset

        currentStep = StoryStep.GetInCar;
        Debug.Log("Step 7: Getting in car");
        //npcController.EnterCar();
        yield return new WaitForSeconds(2f);

        currentStep = StoryStep.DriveOff;
        Debug.Log("Step 8: Driving off and chatting");
        yield return StartCoroutine(LoadAndPlayInkStory(paige_walking_up));
        // Fade out or switch camera here, as needed.
    }

    IEnumerator LoadAndPlayInkStory(TextAsset inkFile)
    {
        dialogueManager.inkAsset = inkFile;
        dialogueManager.inkParser.story = new Ink.Runtime.Story(inkFile.text);
        dialogueManager.inkParser.DisplayDialogue();

        // Wait until the Ink story has finished.
        yield return new WaitUntil(() => dialogueManager.inkParser.endOfStory);
    }
}
