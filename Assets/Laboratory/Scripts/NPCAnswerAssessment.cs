using UnityEngine;

public class NPCAnswerAssessment : MonoBehaviour
{
    private string playerAnswer;
    private DialogueBoxController dialogueBoxController;
    private MicroscopeSlideWithGrid slide;


    [SerializeField] private MicroscopeInfoSubmitUI planktonNotepad;

    // Start is called before the first frame update
    void Start()
    {
        if (planktonNotepad == null)
            Debug.LogError("Plankton notepad is not set. Microscope task will not work!");

        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        if (dialogueBoxController == null)
            Debug.LogError("Could not find dialogueBoxController! There are likely no NPCs in the scene!");
    }

    private float startTime = 0;
    private void Update()
    {
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
                        $"There are {slide.GetTotalAmountOfChaetoceros()} Chaetoceros diatom, " +
                        $"{slide.GetTotalAmountOfPseudoNitzschia()} Pseudo-nitzschia, " +
                        $"and {slide.GetTotalAmountOfSkeletonema()} Skeletonema.";

                    dialogueBoxController.StartDialogue(dialogueBoxController.dialogueTreeRestart, 17, "LarsDialogue");
                }
            }
        }
    }

    public void SetCurrentSlideWithGrid(MicroscopeSlideWithGrid slide)
        { this.slide = slide; }

    public void RemoveCurrentSlide()
        { this.slide = null; }
}