using UnityEngine;

[System.Serializable]
public struct DialogueSection 
{
    [TextArea]
    public string[] dialogue;
    public bool[] interruptableElements;
    public bool endAfterDialogue;

    [Tooltip("Default: display 'speak' button\n" +
             "EndWithRestartButton: display a generic message and enable restart button")]
    public DialogueEnd dialogueEnd;
    public bool disabkeSkipLineButton;
    public bool point;
    public GameObject objectToLookAt;
    public BranchPoint branchPoint;
}

public enum DialogueEnd
{
    Default, EndWithRestartButton
}

[System.Serializable]
public struct BranchPoint 
{
    [TextArea]
    public string question;
    public Answer[] answers;
}

[System.Serializable]
public struct Answer 
{
    public string answerLabel;
    public int nextElement;
    public bool endAfterAnswer;
}
