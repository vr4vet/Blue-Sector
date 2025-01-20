using BNG;
using InteractiveCalculator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class NpcTriggerDialogue : MonoBehaviour
{
   // ----------------- Editor Variables -----------------
   [SerializeField]   
   public UnityEvent triggerEvent;
   [SerializeField]  
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
   } 
   
   private void OnTriggerEnter(Collider other)
   {
      triggerEvent.Invoke();
   }
   
   // Everything correct
   public void response1() {
         _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 0, npcName, 0);
   }
// Forgot basket
   public void response2() {
         _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 1, npcName, 0);
   }
// Wrong condition factor
   public void response3() {
         _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 2, npcName, 0);
   }
// Wrong length
   public void response4() {
         _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 3, npcName, 0);
   }
// Wrong weight
   public void response5() {
         _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 4, npcName, 0);
   }
// Everything is wrong
   public void response6() {
         _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("NpcFeedback"), 5, npcName, 0);
   }
   
   // When the player places a object on the weight scale before turning it on
   public void Error1() {
         
         _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("ErrorFeedback"), 0, npcName, 0);
         
   }
   
   // When the player places the wrong object on the weight scale
   public void Error2() {
         _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("ErrorFeedback"), 2, npcName, 0);
   }
   
   // When the player places the fish without using the basket
   public void Error3() {
       // Destroy the fish to return it to its original position
         GameObject[] instances = GameObject.FindGameObjectsWithTag("Fish");
         foreach (GameObject instance in instances)
         {
             Destroy(instance);
         }
         Instantiate(fishPrefab, _fishPosition, _fishRotation);
         _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("ErrorFeedback"), 3, npcName, 0);
   }
   
   // When the player places the fish before turning on the weight
   public void Error4() {
       GameObject[] instances = GameObject.FindGameObjectsWithTag("Fish");
       foreach (GameObject instance in instances)
       {
           Destroy(instance);
       }
       Instantiate(fishPrefab, _fishPosition, _fishRotation);
       _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("ErrorFeedback"), 0, npcName, 0);
   }
   
   // When the player places something that is not a fish on the number keypad
   public void Error5() {
         _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("ErrorFeedback"), 4, npcName, 0);
   }

   private void ResetDialogue(string dialogueName)
   {
       // Runs when the player has completed calculating the condition factor and resets everything in the scene.
       if (dialogueName == "NpcFeedback")
       {
           _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("Introduction"), 2, npcName, 0);
       
           basket.transform.position = _basketPosition;
           basket.transform.rotation = _basketRotation;
       
       
           Destroy(fish);
           Instantiate(fishPrefab, _fishPosition, _fishRotation);
       
       
           scale.GetComponent<Scale>().totalWeight = 0;
       
           calculator.GetComponent<Calculator>().OnPressedClearAll();
       
           numPad.GetComponent<ResultLogger>().SwitchToCondition();
           numPad.GetComponent<ResultLogger>().SwitchToLength();
           numPad.GetComponent<ResultLogger>().SwitchToWeight();
           numPad.GetComponent<ResultLogger>().weight.SetText("Weight");
           numPad.GetComponent<ResultLogger>().length.SetText("Length");
           numPad.GetComponent<ResultLogger>().conditionFactor.SetText("Condition Factor");
       
           handheldCounter.transform.position = _handheldCounterPosition;
           handheldCounter.transform.rotation = _handheldCounterRotation;
       
           microscopeSnapPoint.GetComponent<SnapZone>().OnDetachEvent.Invoke(null); 
       }
       // Runs when the player has made a mistake and returns the dialogue to the same section the player was in when they made the mistake.
       else if (dialogueName == "ErrorFeedback")
       {
           _dialogueBoxController.StartDialogue(GetDialogueTreeFromName(_returnDialogue), _returnSection, npcName, _returnIndex);
       }
       
       else if (dialogueName == "MicroscopeDialogue")
       {
           _dialogueBoxController.StartDialogue(GetDialogueTreeFromName("Introduction"), 2, npcName, 0);
       }
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
       if ((_oldDialogue.Equals("LarsDialogue") || _oldDialogue.Equals("MicroscopeDialogue")) && _currentDialogue.Equals("ErrorFeedback"))
       {
           _returnDialogue = _oldDialogue;
           _returnSection = _oldSection;
           _returnIndex = _oldIndex;
       }
       
       // The ResetDialogue method after the player has received feedback and is ready to return to the original dialogue.
       if ((dialogueTree == "NpcFeedback" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[6].dialogue[0]) 
           || (dialogueTree == "ErrorFeedback" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[1].dialogue[0]))
       {
           ResetDialogue(dialogueTree);
       }
       else if (dialogueTree == "MicroscopeDialogue" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[2].dialogue[0])
       {
           ResetDialogue(dialogueTree);
       } 
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
