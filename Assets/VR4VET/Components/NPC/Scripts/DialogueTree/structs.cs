using UnityEngine;

public enum DialogueEnd
{
    Default, EndWithRestartButton
}

[System.Serializable]
public struct DialogueSection
{
    [TextArea]
    public string[] dialogue;
    public bool endAfterDialogue;
    public string walkOrTurnTowardsAfterDialogue;
    public bool disabkeSkipLineButton;
    public string pointAt;
    public BranchPoint branchPoint;
    [Tooltip("Check this box if")]
    public bool[] interruptableElements; // Array size should match 'dialogue' array size
    [Tooltip("Determines behavior when 'End After Dialogue' is checked.")] 
    public DialogueEnd dialogueEnd; // Default behavior
    
    // 'point' and 'objectToLookAt' from deprecated version are ignored for now based on user choice.
    // public bool point;
    // public GameObject objectToLookAt;

    public static DialogueSection CreateDefault()
    {
        return new DialogueSection
        {
            dialogue = new string[0],
            endAfterDialogue = false,
            walkOrTurnTowardsAfterDialogue = string.Empty,
            disabkeSkipLineButton = false,
            pointAt = string.Empty,
            branchPoint = new BranchPoint(),
            interruptableElements = new bool[0],
            dialogueEnd = DialogueEnd.Default
        };
    }
}

[System.Serializable]
public struct BranchPoint
{
    [TextArea]
    public string question;
    public Answer[] answers;

    public static BranchPoint CreateDefault()
    {
        return new BranchPoint
        {
            question = string.Empty,
            answers = new Answer[0]
        };
    }
}

[System.Serializable]
public struct Answer
{
    public string answerLabel;
    public int nextElement;
    public bool endAfterAnswer;
    public string walkOrTurnTowardsAfterAnswer;

    public static Answer CreateDefault()
    {
        return new Answer
        {
            answerLabel = string.Empty,
            nextElement = 0,
            endAfterAnswer = false,
            walkOrTurnTowardsAfterAnswer = string.Empty
        };
    }
}
