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
        NPCInterrupt,
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

    //game difficulty
    int enginePuzzleDifficulty = 0;


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


        #region Step 3: NPC intro dialogue
        // Step 3: NPC intro dialogue
        Debug.Log("Step 3: NPC intro dialogue");

        // Pause player input during dialogue (already disabled above).
        currentStep = StoryStep.NPCIntroDialogue;

        Debug.Log("Step 3: NPC intro dialogue (explaining hard puzzle)");
        yield return StartCoroutine(LoadAndPlayInkStory(paige_walking_up));
        
        #endregion


        #region Step 4: Hardminigame
        // Step 4: Hardminigame
        Debug.Log("Step 4: Hard minigame running");
        currentStep = StoryStep.HardMinigame;
        Debug.Log("StoryStep.HardMinigame");


        // Set up a hard puzzle a difficulty value of 81 yields a 9x9 puzzle.
        enginePuzzleDifficulty = 81;
        yield return new WaitUntil(() => repairTrigger.engineMingameStarted == true);
        repairTrigger.StartEngineMinigame(enginePuzzleDifficulty);
        Debug.Log("SET GAME LEVEL TO 81");


        //play puzzle for a few seconds
        yield return new WaitForSeconds(10f);


        #endregion


        #region Step 5: NPC Interrupt
        // Step 5: NPC Interrupt
        // Disable piece interaction while the NPC explains the puzzle.
        repairTrigger.loopGameManager.SetPuzzleInteractable(false);
        Debug.Log("SetPuzzleInteractable(false)");

        //yield return new WaitUntil(() => repairTrigger.loopGameManager.SetPuzzleInteractable == false);
        //leave game
        repairTrigger.EndEngineMinigame();
        Debug.Log("ExitMinigame()");
        //paige explains to player
        //yield return StartCoroutine(LoadAndPlayInkStory(paige_walking_up));

        // Assume the minigame is already loaded externally.
        Debug.Log("Step 5: NPC Interrupt");
        // Continue with further dialogue or game steps...
        currentStep = StoryStep.NPCInterrupt;
        //yield return StartCoroutine(LoadAndPlayInkStory(paige_walking_up)); // Or another dialogue asset
        #endregion


        #region Step 6: StoryStep EasyMinigame
        // Step 6: StoryStep EasyMinigame
        Debug.Log("Step 6: Easy minigame running");

        currentStep = StoryStep.EasyMinigame;
        // Set up an easier puzzle difficulty value of 9 makes a 3x3 puzzle.
        enginePuzzleDifficulty = 9;
        
        repairTrigger.StartEngineMinigame(enginePuzzleDifficulty);
        // wait until the player succeeds.
        yield return new WaitUntil(() => repairTrigger.engineMiniGameRunning == false);
        #endregion


        #region Step 7: StoryStep NPCWrapUpDialogue
        // Step 7: StoryStep NPCWrapUpDialogue
        Debug.Log("Step 7: NPC wrap-up dialogue");
        // Continue with further dialogue or game steps...
        currentStep = StoryStep.NPCWrapUpDialogue;
        //yield return StartCoroutine(LoadAndPlayInkStory(paige_walking_up)); // Or another dialogue asset
        #endregion


        #region Step 9: Getting in car
        // Step 9: Getting in car
        Debug.Log("Step 9: Getting in car");
        currentStep = StoryStep.GetInCar;
        //npcController.EnterCar();
        yield return new WaitForSeconds(2f);
        #endregion


        #region Step 10: DriveOff
        // Step 10: DriveOff
        Debug.Log("Step 10: Driving off and chatting");
        currentStep = StoryStep.DriveOff;
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
