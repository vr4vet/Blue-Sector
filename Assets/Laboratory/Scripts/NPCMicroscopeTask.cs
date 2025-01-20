using BNG;
using UnityEngine;
using UnityEngine.Events;

public class NPCMicroscopeTask : MonoBehaviour
{
    private DialogueBoxController dialogueBoxController;
    private MicroscopeSlideWithGrid slide;
    [SerializeField] private MicroscopeInfoSubmitUI planktonNotepad;
    private ObjectPositions objectPositions;

    public UnityEvent EnablePlanktonHighlights;
    public UnityEvent DisablePlanktonHighlights;
    private bool HighLightPlankton = false;
    private bool ResetAfterTryingAgain = false;


    // Start is called before the first frame update
    void Start()
    {
        if (planktonNotepad == null)
            Debug.LogError("Plankton notepad is not set. Microscope task will not work!");

        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        if (dialogueBoxController == null)
            Debug.LogError("Could not find dialogueBoxController! There are likely no NPCs in the scene!");

        objectPositions = ObjectPositions.Instance;

        ButtonSpawner.OnAnswer += ButtonSpawner_OnAnswer;
    }

    private bool haveBeenReset = false;
    private float startTime = 0;
    private void Update()
    {
        if (dialogueBoxController.dialogueTreeRestart == null)
            return;

        // checking if NPC is currently letting the player select task. if so, reset the microscope task. 
        if ((dialogueBoxController.dialogueTreeRestart.name == "Introduction" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[2].dialogue[0])
            ||
            (dialogueBoxController.dialogueTreeRestart.name == "MicroscopeDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[1].dialogue[0]))
        {
            // prevent uneccessary resources spent on constantly resetting objects (the operations included are expensive)
            if (haveBeenReset)
                return;

            // resetting water sample, counter, note sheet, and informational posters
            ResetWaterSample();
            ResetHandheldCounter();
            ResetPosters();
            ResetPlanktonNoteSheet();

            haveBeenReset = true;
        }
        else
            haveBeenReset = false;

        // reset microscope task if the player chose to try again after failing or succeeding in counting plankton
        if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[3].branchPoint.question && dialogueBoxController.dialogueTreeRestart.name == "MicroscopeDialogue")
        {
            if (ResetAfterTryingAgain)
            {
                // resetting water sample, counter, note sheet, and informational posters
                ResetWaterSample();
                ResetHandheldCounter();
                ResetPosters();
                ResetPlanktonNoteSheet();

                ResetAfterTryingAgain = false;
            }
            return;
        }

        // checking if NPC is currently verifying plankton count. jump to the appropriate dialogue section depending on if correct or incorrect.
        if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[4].dialogue[0] && dialogueBoxController.dialogueTreeRestart.name == "MicroscopeDialogue")
        {
            if (startTime == 0)
                startTime = Time.time;

            if (Time.time - startTime >= 8) // npc 'thinks' for 8 seconds
            {
                startTime = 0;

                // checking if player has placed slide onto microscope
                if (slide == null)
                {
                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 8, "MicroscopeDialogue", 0);
                    return;
                }

                // checking all three plankton count answers
                bool correct = true;
                for (int i = 0; i < 3; i++)
                {
                    if (!planktonNotepad.VerifyAnswer(i))
                        correct = false;
                }
                
                if (correct)
                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 5, "MicroscopeDialogue", 0);
                else
                {
                    // dynamically adding dialogue with correct answers. this could be a risky operation!
                    dialogueBoxController.dialogueTreeRestart.sections[7].dialogue[0] =
                        $"I counted {slide.GetTotalAmountOfChaetoceros()} Chaetoceros diatom, " +
                        $"{slide.GetTotalAmountOfPseudoNitzschia()} Pseudo-nitzschia diatom, " +
                        $"and {slide.GetTotalAmountOfSkeletonema()} Skeletonema diatom.";

                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 6, "MicroscopeDialogue", 0);    
                }
            }
            return;
        }

        // checking if NPC is attempting to highlight plankton
        if (dialogueBoxController.dialogueTreeRestart.name == "MicroscopeDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[10].dialogue[0])
        {
            if (startTime == 0)
                startTime = Time.time;

            if (Time.time - startTime >= 5) // npc 'thinks' for 8 seconds
            {
                startTime = 0;

                // checking if player has placed slide onto microscope
                if (slide == null)
                {
                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 8, "MicroscopeDialogue", 0);
                    return;
                }

                // highlighting plankton
                if (HighLightPlankton)
                {
                    EnablePlanktonHighlights.Invoke();
                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 11, "MicroscopeDialogue", 0);
                }     
                else
                {
                    DisablePlanktonHighlights.Invoke();
                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 12, "MicroscopeDialogue", 0);
                }   
            }
        }
    }

    private void ResetWaterSample()
    {
        GameObject child = GameObject.Find("MicroscopeSlideModel");

        if (slide != null)
            slide.transform.parent.GetComponent<SnapZone>().ReleaseAll();
        else if (child.GetComponent<Grabbable>().BeingHeld)
            return;

        GameObject parent = GameObject.Find("MicroscopeSlideWithGrid");

        // make grabbable child element (the visual/physical part of slide) child of its parent again (typically happens automatically when grabbed and released by player), and reset local position/rotation
        child.transform.SetParent(parent.transform);
        child.transform.localPosition = Vector3.zero;
        child.transform.localEulerAngles = Vector3.zero;
    }

    private void ResetPosters()
    {
        if (!objectPositions.chaetocerosPoster.GetComponent<Grabbable>().BeingHeld)
            objectPositions.chaetocerosPoster.transform.SetPositionAndRotation(objectPositions._chaetocerosPosterPosition, objectPositions._chaetocerosPosterRotation);
        if (!objectPositions.pseudoNitzschiaPoster.GetComponent<Grabbable>().BeingHeld)
            objectPositions.pseudoNitzschiaPoster.transform.SetPositionAndRotation(objectPositions._pseudoNitzschiaPosterPosition, objectPositions._pseudoNitzschiaPosterRotation);
        if (!objectPositions.skeletonemaPoster.GetComponent<Grabbable>().BeingHeld)
            objectPositions.skeletonemaPoster.transform.SetPositionAndRotation(objectPositions._skeletonemaPosterPosition, objectPositions._skeletonemaPosterRotation);
        if (!objectPositions.planktonBasicsPoster.GetComponent<Grabbable>().BeingHeld)
            objectPositions.planktonBasicsPoster.transform.SetPositionAndRotation(objectPositions._planktonBasicsPosterPosition, objectPositions._planktonBasicsPosterRotation);
    }

    private void ResetPlanktonNoteSheet()
    {
        planktonNotepad.ClearAllInputFields();
        planktonNotepad.MarkResetAll();
        if (!objectPositions.planktonNotepad.GetComponent<Grabbable>().BeingHeld)
            objectPositions.planktonNotepad.transform.SetPositionAndRotation(objectPositions._planktonNotepadPosition, objectPositions._planktonNotepadRotation);
    }

    private void ResetHandheldCounter()
    {
        if (!objectPositions.handheldCounter.GetComponent<Grabbable>().BeingHeld)
            objectPositions.handheldCounter.transform.SetPositionAndRotation(objectPositions._handheldCounterPosition, objectPositions._handheldCounterRotation);
    }

    /// <summary>
    /// Storing the player's answer when asked if they want to have plankton highlighted.
    /// </summary>
    /// <param name="answer"></param>
    private void ButtonSpawner_OnAnswer(string answer)
    {
        if (dialogueBoxController.dialogueTreeRestart.name != "MicroscopeDialogue")
            return;
        if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[9].branchPoint.question)
        {
            if (answer == "Yes")
                HighLightPlankton = true;
            else
                HighLightPlankton = false;
        }

        if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[5].branchPoint.question 
            || 
            dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[6].branchPoint.question)
        {
            if (answer == "Try again" || answer == "Yes")
                ResetAfterTryingAgain = true;
        }
    }

    public void SetCurrentSlideWithGrid(MicroscopeSlideWithGrid slide)
        { this.slide = slide; }

    public void RemoveCurrentSlide()
        { this.slide = null; }
}