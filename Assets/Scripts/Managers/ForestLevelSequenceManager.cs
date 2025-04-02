using UnityEngine;
using System.Collections;

public class ForestLevelSequenceManager : MonoBehaviour
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
    public NPCPathFollower npcController;
    public VehicleController vehicleController;
    public RepairTrigger repairTrigger;


    //dialogue ink assets
    public TextAsset paige_walking_up;
    

    void Start()
    {
        StartCoroutine(RunSequence());
    }
    IEnumerator RunSequence()
    {
        #region Step 1: Car breakdown
        // Step 1: Car breakdown
        Debug.Log("Step 1: Car breakdown");
        yield return new WaitUntil(() => vehicleController.isBrokenDown);
        #endregion


        #region Step 2: NPC walks if triggered
        // Step 2: NPC walks if triggered
        Debug.Log("Step 2: NPC walks up");
        yield return new WaitUntil(() => npcController.startOnTrigger);
        currentStep = StoryStep.NPCWalksUp;
        yield return new WaitUntil(() => !npcController.isMoving);
        Debug.Log("Paige is Done Moving");
        #endregion


        #region Step 3: Hardminigame
        // Step 3: Hardminigame
        Debug.Log("Step 3: Hard minigame running");
        currentStep = StoryStep.HardMinigame;
        Debug.Log("StoryStep.HardMinigame");

        // Set up a hard puzzle a difficulty value of 25 yields a 5x5 puzzle.
        repairTrigger.puzzleDifficulty = 25;
        repairTrigger.loopGameManager.SetupPuzzleForDifficulty(25);
        Debug.Log("SET GAME LEVEL TO 25");

        //play puzzle for a few seconds
        yield return new WaitForSeconds(10f);

        // Disable piece interaction while the NPC explains the puzzle.
        repairTrigger.loopGameManager.SetPuzzleInteractable(false);
        Debug.Log("SetPuzzleInteractable(false)");
        //yield return new WaitUntil(() => repairTrigger.loopGameManager.SetPuzzleInteractable == false);
        repairTrigger.loopGameManager.ExitMinigame();
        Debug.Log("ExitMinigame()");
        // Assume the minigame is already loaded externally.
        #endregion


        #region Step 4: NPC intro dialogue
        // Step 4: NPC intro dialogue
        Debug.Log("Step 3: Hard minigame running");

        // Pause player input during dialogue (already disabled above).
        currentStep = StoryStep.NPCIntroDialogue;
        Debug.Log("Step 4: NPC intro dialogue (explaining hard puzzle)");
        yield return StartCoroutine(LoadAndPlayInkStory(paige_walking_up));

        // End the current (hard) minigame.
        repairTrigger.loopGameManager.ExitMinigame();

        // Re-enable puzzle interaction (if needed for the next phase).
        repairTrigger.loopGameManager.SetPuzzleInteractable(true);
        #endregion


        #region Step 5: StoryStep EasyMinigame
        // Step 5: StoryStep EasyMinigame
        Debug.Log("Step 5: Easy minigame running");

        currentStep = StoryStep.EasyMinigame;
        
        // Set up an easier puzzle; e.g., a difficulty value of 9 yields a 3x3 puzzle.
        repairTrigger.loopGameManager.SetupPuzzleForDifficulty(9);
        // Optionally, wait until the player succeeds.
        //yield return new WaitUntil(() => repairTrigger.loopGameManager.Win();
        #endregion


        #region Step 6: StoryStep NPCWrapUpDialogue
        // Step 6: StoryStep NPCWrapUpDialogue
        // Continue with further dialogue or game steps...
        currentStep = StoryStep.NPCWrapUpDialogue;
        Debug.Log("Step 6: NPC wrap-up dialogue");
        //yield return StartCoroutine(LoadAndPlayInkStory(paige_walking_up)); // Or another dialogue asset
        #endregion


        #region Step 7: Getting in car
        // Step 7: Getting in car
        currentStep = StoryStep.GetInCar;
        Debug.Log("Step 7: Getting in car");
        //npcController.EnterCar();
        yield return new WaitForSeconds(2f);
        #endregion


        #region Step 8: DriveOff
        // Step 8: DriveOff
        currentStep = StoryStep.DriveOff;
        Debug.Log("Step 8: Driving off and chatting");
        //yield return StartCoroutine(LoadAndPlayInkStory(paige_walking_up));
        // Fade out or switch camera here, as needed.
        #endregion
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
