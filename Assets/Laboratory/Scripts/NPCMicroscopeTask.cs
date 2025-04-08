using BNG;
using System.Collections;
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
    public UnityEvent OnPlanktonCountAssess;
    public UnityEvent OnPlanktonCountCorrect;
    public UnityEvent OnPlanktonCountCorrectFirstTry;
    private bool HighLightPlankton = false;
    private int tries = 0;


    // Start is called before the first frame update
    void Start()
    {
        EnablePlanktonHighlights ??= new UnityEvent();
        DisablePlanktonHighlights ??= new UnityEvent();
        OnPlanktonCountAssess ??= new UnityEvent();
        OnPlanktonCountCorrect ??= new UnityEvent();
        OnPlanktonCountCorrectFirstTry ??= new UnityEvent();

        if (planktonNotepad == null)
            Debug.LogError("Plankton notepad is not set. Microscope task will not work!");

        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        if (dialogueBoxController == null)
            Debug.LogError("Could not find dialogueBoxController! There are likely no NPCs in the scene!");

        objectPositions = ObjectPositions.Instance;

        dialogueBoxController.m_DialogueChanged.AddListener(OnDialogueChanged);
        ButtonSpawner.OnAnswer += ButtonSpawner_OnAnswer;
    }

    private bool _correctAnswer = false;
    private void OnDialogueChanged(string npcName, string dialogueTree, int section, int index)
    {

        // checking if NPC is currently letting the player select task, or if the player wants to try again. if so, reset the microscope task. 
        if ((dialogueBoxController.dialogueTreeRestart.name == "Introduction" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[2].dialogue[0])
            ||
            (dialogueBoxController.dialogueTreeRestart.name == "MicroscopeDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[1].dialogue[0])
            || 
            (dialogueBoxController.dialogueTreeRestart.name == "MicroscopeDialogue") && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[3].branchPoint.question && _correctAnswer)
        {
            // resetting water sample, counter, note sheet, and informational posters
            ResetWaterSample();
            ResetHandheldCounter();
            ResetPosters();
            ResetPlanktonNoteSheet();
            _correctAnswer = false;
        }

        // checking if NPC is currently verifying plankton count. jump to the appropriate dialogue section depending on if correct or incorrect.
        if (dialogueBoxController.dialogueTreeRestart.name == "MicroscopeDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[4].dialogue[0])
            StartCoroutine(nameof(NPCVerifyAnswer));

        // checking if NPC is attempting to highlight plankton
        if (dialogueBoxController.dialogueTreeRestart.name == "MicroscopeDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[10].dialogue[0])
            StartCoroutine(nameof(NPCHighlightPlankton));
    }

    /// <summary>
    /// Coroutine that makes NPC wait a little before verifying the player's answers
    /// </summary>
    /// <returns></returns>
    private IEnumerator NPCVerifyAnswer()
    {
        tries++;

        OnPlanktonCountAssess.Invoke(); // complete step in tablet

        yield return new WaitForSeconds(8); // NPC 'thinks' for 8 seconds

        // checking if player has placed slide onto microscope
        if (slide == null)
        {
            dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 8, "MicroscopeDialogue", 0);
            yield break;
        }

        // checking all three plankton count answers
        bool correct = true;
        for (int i = 0; i < 3; i++)
        {
            if (!planktonNotepad.VerifyAnswer(i))
                correct = false;
        }
        _correctAnswer = correct; // prevent resetting task-related objects in OnDialogueChanged if the player answered incorrectly and wants to try again

        if (correct)
        {
            OnPlanktonCountCorrect.Invoke(); // complete step in tablet
            dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 5, "MicroscopeDialogue", 0);
            if (tries < 2)
                OnPlanktonCountCorrectFirstTry.Invoke();
        }
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

    /// <summary>
    /// Coroutine that makes the NPC wait a bit before highlighting Plankton
    /// </summary>
    /// <returns></returns>
    private IEnumerator NPCHighlightPlankton()
    {
        yield return new WaitForSeconds(5); // NPC 'thinks' for 5 seconds

        // checking if player has placed slide onto microscope
        if (slide == null)
        {
            dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 8, "MicroscopeDialogue", 0);
            yield break;
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
    private void ButtonSpawner_OnAnswer(string answer, string questin, string name)
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
    }

    public void SetCurrentSlideWithGrid(MicroscopeSlideWithGrid slide) => this.slide = slide;

    public void RemoveCurrentSlide() => this.slide = null;

    private void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= ButtonSpawner_OnAnswer;
    }
}