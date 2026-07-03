using BNG;
using InteractiveCalculator;
using UnityEngine;
using UnityEngine.Events;

public class NpcTriggerDialogue : MonoBehaviour
{
    // ----------------- Editor Variables -----------------
    public UnityEvent triggerEvent;
    public string npcName;

    public GameObject fishPrefab;
   
    public GameObject basket;
    public GameObject fish;
    public GameObject scale;
    public GameObject calculator;
    public GameObject numPad;
    public GameObject handheldCounter;
    public GameObject microscopeSnapPoint;
   
    private Vector3 _basketPosition;
    private Quaternion _basketRotation;
    private Vector3 _handheldCounterPosition;
    private Quaternion _handheldCounterRotation;
    private Vector3 _fishPosition;
    private Quaternion _fishRotation;
  

    private DialogueBoxController _dialogueBoxController;
    private ConversationController _conversationController;
   
    // These variables are used to keep track of the current and previous dialogue
    private string _currentDialogue = "", _oldDialogue = "";
    private int _oldSection, _oldIndex, _currentSection, _currentIndex = 0;
   
   

    private void Start()
    {
        _dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        _conversationController = FindObjectOfType<ConversationController>();

        // Save the initial positions of the objects
        _basketPosition = basket.transform.position;
        _basketRotation = basket.transform.rotation;
        _handheldCounterPosition = handheldCounter.transform.position;
        _handheldCounterRotation = handheldCounter.transform.rotation;
        _fishPosition = fish.transform.position;
        _fishRotation = fish.transform.rotation;
        _dialogueBoxController.m_DialogueChanged.AddListener(OnDialogueChanged);

        // Enable collision to make sure the player can pick up the fish after visiting a scenario which disabled it
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bone"), false);
    } 
   
    private void OnTriggerEnter(Collider other)
    {
        triggerEvent.Invoke();
    }
   
    // Everything correct
    public void Response1() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 0, npcName, 0);

    // Forgot basket
    public void Response2() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 1, npcName, 0);
    
    // Wrong condition factor
    public void Response3() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 2, npcName, 0);

    // Wrong length
    public void Response4() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 3, npcName, 0);
    
    // Wrong weight
    public void Response5() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 4, npcName, 0);

    // Everything is wrong
    public void Response6() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 5, npcName, 0);
   
    // When the player places a object on the weight scale before turning it on
    public void Error1() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("ErrorFeedback"), 0, npcName, 0);
   
    // When the player places the wrong object on the weight scale
    public void Error2() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("ErrorFeedback"), 2, npcName, 0);
   
    // When the player places the fish without using the basket
    public void Error3() 
    {
        // Destroy the fish to return it to its original position
        Destroy(fish);
        fish = Instantiate(fishPrefab, _fishPosition, _fishRotation);
        _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("ErrorFeedback"), 3, npcName, 0);
    }
   
    // When the player places the fish before turning on the weight
    public void Error4() 
    {
        Destroy(fish);
        fish = Instantiate(fishPrefab, _fishPosition, _fishRotation);
        _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("ErrorFeedback"), 0, npcName, 0);
    }
   
    // When the player places something that is not a fish on the number keypad
    public void Error5() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("ErrorFeedback"), 4, npcName, 0);

    // When the player has done something incorrectly and the dialogue needs to return to where it was before NPC gave feedback
    private void ReturnAfterFeedback(string dialogueName)
    {
        if (dialogueName == "ErrorFeedback" || dialogueName == "NpcFeedback")
            _dialogueBoxController.StartDialogue(GetDialogueTreeFromName(_returnDialogue), _returnSection, npcName, _returnIndex);
    }

    // Reset everything and change to introduction dialogue
    private void ReturnToIntroduction()
    {
        _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("Introduction"), 2, npcName, 0);
        ResetObjects();
    }

    // Reset the condition factor dialogue and objects
    private void ResetConditionFactorDialogue()
    {
        _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("LarsDialogue"), 0, npcName, 0);
        ResetObjects();
    }

    // Return to the condition factor calculation part of LarsDialogue
    private void ReturnToConditionFactorCalculation() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("LarsDialogue"), 5, npcName, 0);

    // Return to the weighing part of LarsDialogue
    private void ReturnToMeasuringWeight() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("LarsDialogue"), 1, npcName, 0);

    // Return to the measuring part of LarsDialogue
    private void ReturnToMeasuringLength() => _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("LarsDialogue"), 3, npcName, 0);

    // Reset all task-related objects
    public void ResetObjects()
    {
        basket.transform.SetPositionAndRotation(_basketPosition, _basketRotation);
        
        Destroy(fish);
        fish = Instantiate(fishPrefab, _fishPosition, _fishRotation);

        scale.GetComponent<Scale>().totalWeight = 0;
        
        calculator.GetComponent<Calculator>().OnPressedClearAll();

        numPad.GetComponent<ResultLogger>().SwitchToCondition();
        numPad.GetComponent<ResultLogger>().SwitchToLength();
        numPad.GetComponent<ResultLogger>().SwitchToWeight();
        numPad.GetComponent<ResultLogger>().weight.SetText("Weight");
        numPad.GetComponent<ResultLogger>().length.SetText("Length");
        numPad.GetComponent<ResultLogger>().conditionFactor.SetText("Condition Factor");

        handheldCounter.transform.SetPositionAndRotation(_handheldCounterPosition, _handheldCounterRotation);
        microscopeSnapPoint.GetComponent<SnapZone>().OnDetachEvent.Invoke(null);
    }

   
    // Variables to store the dialogue the player was in before the mistake was made.
    private string _returnDialogue = "";
    private int _returnSection = 0; 
    private int _returnIndex = 0;
   
    /// <summary>
    /// This method is called when the dialogue changes. It saves the current dialogue and the previous dialogue. When the dialogue changes to ErrorFeeback it will save the previous dialogue so it can return to it.
    /// It will also call the ResetDialogue method when the player is ready to return to the task.
    /// </summary>
    private void OnDialogueChanged(string npcName, string dialogueTree, int section, int index)
    {
        // The variables for the old dialogue are set before the new dialogue is set, making it represent the previous duologue.
        _oldDialogue = _currentDialogue;
        _oldSection = _currentSection;
        _oldIndex = _currentIndex;
        // The variables store information about the current dialogue.
        _currentDialogue = dialogueTree;
        _currentSection = section;
        _currentIndex = index;
        
        // The old dialogue is saved when the dialogue tree changes to ErrorFeedback from LarsDialogue or MicroscopeDialogue, which will be used to return to the dialogue the player was in before the mistake was made.
        if (_oldDialogue.Equals("LarsDialogue") && (_currentDialogue.Equals("ErrorFeedback") || _currentDialogue.Equals("NpcFeedback")))
        {
            _returnDialogue = _oldDialogue;
            _returnSection = _oldSection;
            _returnIndex = _oldIndex;
        }
       
        // Moving to the appropriate dialogue depending on the current state
        if (dialogueTree == "ErrorFeedback" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[1].dialogue[0]) // return to the step where the player failed
            ReturnAfterFeedback(dialogueTree);
        else if (dialogueTree == "MicroscopeDialogue" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[2].dialogue[0])
            ReturnToIntroduction();
        else if (dialogueTree == "NpcFeedback" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[3].dialogue[0])
            ReturnToMeasuringWeight();
        else if (dialogueTree == "NpcFeedback" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[4].dialogue[0])
            ReturnToMeasuringLength();
        else if (dialogueTree == "NpcFeedback" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[6].dialogue[0])
            ReturnToConditionFactorCalculation();
        else if (dialogueTree == "NpcFeedback" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[7].dialogue[0])
            ResetConditionFactorDialogue();
        else if (dialogueTree == "NpcFeedback" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[8].dialogue[0])
            ReturnToIntroduction();
    }

    /// <summary>
    /// This method finds the previous dialogue and returns to it.
    /// </summary>
    private void GoToPreviousDialogue()
    {
        DialogueTree previousDialogueTree = null;

        foreach (DialogueTree tree in _conversationController.GetDialogueTrees())
        {
            if (tree.name.Equals(_oldDialogue))
                previousDialogueTree = tree;
        }

        if (previousDialogueTree != null)
            _dialogueBoxController.StartDialogue(previousDialogueTree, _oldSection, npcName, _oldIndex);
    }

    /// <summary>
    /// This method will return the dialogue tree with the name that is passed as a parameter among the list of dialogue trees in the NPC. 
    /// </summary>
    private DialogueTree GetDialogueTreeFromName(string name)
    {
        DialogueTree returnTree = null;

        foreach (DialogueTree tree in _conversationController.GetDialogueTrees())
        {
            if (tree.name.Equals(name))
                returnTree = tree;
        }

        if (returnTree != null)
            return returnTree;
        return null;
    }
   
}
