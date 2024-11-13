using BNG;
using UnityEngine;
using UnityEngine.Events;

public class NPCMicroscopeTask : MonoBehaviour
{
    private string playerAnswer;
    private DialogueBoxController dialogueBoxController;
    private MicroscopeSlideWithGrid slide;
    [SerializeField] private MicroscopeInfoSubmitUI planktonNotepad;
    private ObjectPositions objectPositions;

    public UnityEvent EnablePlanktonHighlights;
    public UnityEvent DisablePlanktonHighlights;
    private bool HighLightPlankton = false;


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
        if (dialogueBoxController.dialogueTreeRestart.name != "LarsDialogue")
            return;

        // checking if NPC is currently letting the player select task. if so, reset the microscope task. 
        if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[1].dialogue[0]
            ||
            dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[0])
        {
            // prevent uneccessary resources spent on constantly resetting objects (the operations included are expensive)
            if (haveBeenReset)
                return;

            // resetting water sample position
            RestoreSlidePositionAndHierarchy();

            // emptying note sheet input fields and removing red or green marking
            planktonNotepad.ClearAllInputFields();
            planktonNotepad.MarkResetAll();

            if (!objectPositions.planktonNotepad.GetComponent<Grabbable>().BeingHeld)
                objectPositions.planktonNotepad.transform.SetPositionAndRotation(objectPositions._planktonNotepadPosition, objectPositions._planktonNotepadRotation);
            
            // resetting position of counter, note sheet, and informational posters
            RestorePostersPosition();

            haveBeenReset = true;
        }
        else
            haveBeenReset = false;

        // checking if NPC is currently verifying plankton count. jump to the appropriate dialogue section depending on if correct or incorrect.
        if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[15].dialogue[0])
        {
            if (startTime == 0)
                startTime = Time.time;

            if (Time.time - startTime >= 8) // npc 'thinks' for 8 seconds
            {
                startTime = 0;

                // checking if player has placed slide onto microscope
                if (slide == null)
                {
                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 19, "LarsDialogue");
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
                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 16, "LarsDialogue");
                else
                {
                    // dynamically adding dialogue with correct answers. this could be a risky operation!
                    dialogueBoxController.dialogueTreeRestart.sections[18].dialogue[0] =
                        $"I counted {slide.GetTotalAmountOfChaetoceros()} Chaetoceros diatom, " +
                        $"{slide.GetTotalAmountOfPseudoNitzschia()} Pseudo-nitzschia diatom, " +
                        $"and {slide.GetTotalAmountOfSkeletonema()} Skeletonema diatom.";

                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 17, "LarsDialogue");
                }
            }
        }

        // checking if NPC is attempting to highlight plankton
        if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[21].dialogue[0])
        {
            if (startTime == 0)
                startTime = Time.time;

            if (Time.time - startTime >= 5) // npc 'thinks' for 8 seconds
            {
                startTime = 0;

                // checking if player has placed slide onto microscope
                if (slide == null)
                {
                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 19, "LarsDialogue");
                    return;
                }

                // highlighting plankton
                if (HighLightPlankton)
                {
                    EnablePlanktonHighlights.Invoke();
                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 22, "LarsDialogue");
                }     
                else
                {
                    DisablePlanktonHighlights.Invoke();
                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 23, "LarsDialogue");
                }   
            }
        }
    }

    private void RestoreSlidePositionAndHierarchy()
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

    private void RestorePostersPosition()
    {
        if (!objectPositions.handheldCounter.GetComponent<Grabbable>().BeingHeld)
            objectPositions.handheldCounter.transform.SetPositionAndRotation(objectPositions._handheldCounterPosition, objectPositions._handheldCounterRotation);
        if (!objectPositions.chaetocerosPoster.GetComponent<Grabbable>().BeingHeld)
            objectPositions.chaetocerosPoster.transform.SetPositionAndRotation(objectPositions._chaetocerosPosterPosition, objectPositions._chaetocerosPosterRotation);
        if (!objectPositions.pseudoNitzschiaPoster.GetComponent<Grabbable>().BeingHeld)
            objectPositions.pseudoNitzschiaPoster.transform.SetPositionAndRotation(objectPositions._pseudoNitzschiaPosterPosition, objectPositions._pseudoNitzschiaPosterRotation);
        if (!objectPositions.skeletonemaPoster.GetComponent<Grabbable>().BeingHeld)
            objectPositions.skeletonemaPoster.transform.SetPositionAndRotation(objectPositions._skeletonemaPosterPosition, objectPositions._skeletonemaPosterRotation);
    }

    /// <summary>
    /// Storing the player's answer when asked if they want to have plankton highlighted.
    /// </summary>
    /// <param name="answer"></param>
    private void ButtonSpawner_OnAnswer(string answer)
    {
        if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[20].branchPoint.question)
        {
            if (answer == "Yes")
                HighLightPlankton = true;
            else
                HighLightPlankton = false;
        }
    }

    public void SetCurrentSlideWithGrid(MicroscopeSlideWithGrid slide)
        { this.slide = slide; }

    public void RemoveCurrentSlide()
        { this.slide = null; }
}