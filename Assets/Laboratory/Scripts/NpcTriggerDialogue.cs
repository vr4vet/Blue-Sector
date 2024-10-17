using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using InteractiveCalculator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEditor;

public class NpcTriggerDialogue : MonoBehaviour
{
   // ----------------- Editor Variables -----------------
   [SerializeField]   
   public UnityEvent triggerEvent;
   [FormerlySerializedAs("dialogueTree")] [SerializeField]     
   public DialogueTree feedbackDialogueTree;
   [SerializeField]
   public DialogueTree larsDialogueTree;
   [SerializeField]
   public DialogueTree errorFeedbackDialogueTree;
   [SerializeField]  
   public string npcName;

   public GameObject fishPrefab;
   
   public GameObject basket;
   public GameObject fish;
   public GameObject scale;
   public GameObject calculator;
   public GameObject numPad;
   public GameObject handheldCounter;
   public GameObject slider;
   public GameObject microscopeSnapPoint;
   
   private Vector3 _basketPosition;
   private Quaternion _basketRotation;
   private Vector3 _handheldCounterPosition;
   private Quaternion _handheldCounterRotation;
   private Vector3 _fishPosition;
   private Quaternion _fishRotation;
  

   private DialogueBoxController dialogueBoxController;
   private int _dialogueIndex;

   private void Start()
   {
       dialogueBoxController = FindObjectOfType<DialogueBoxController>();
       // Save the initial positions of the objects
       _basketPosition = basket.transform.position;
       _basketRotation = basket.transform.rotation;
       _handheldCounterPosition = handheldCounter.transform.position;
       _handheldCounterRotation = handheldCounter.transform.rotation;
       _fishPosition = fish.transform.position;
       _fishRotation = fish.transform.rotation;
         
       
       
       
   }  
   
   private void Update()
   {
       // Check the current dialogue and if it is the correct one, change the dialogue
       if (dialogueBoxController.dialogueTreeRestart != null)
       {
           if ((dialogueBoxController.dialogueTreeRestart.name == "NpcFeedback" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[6].dialogue[0]) || (dialogueBoxController.dialogueTreeRestart.name == "ErrorFeedback" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[1].dialogue[0]))
           {
               Debug.Log(dialogueBoxController.dialogueTreeRestart.name);
               ChangeToLars(dialogueBoxController.dialogueTreeRestart.name);
           
           }
       }

   }
   
   private void OnTriggerEnter(Collider other)
   {
      triggerEvent.Invoke();
   }

   // Find the index of the current dialogue section
   public void FindDialogueSection()
   {
       for (int i = 0; i < dialogueBoxController.dialogueTreeRestart.sections.Length; i++)
       {
           for (int j = 0; j < dialogueBoxController.dialogueTreeRestart.sections[i].dialogue.Length; j++)
           {
               if (dialogueBoxController.dialogueTreeRestart.sections[i].dialogue[j] == dialogueBoxController._dialogueText.text)
               {
                   _dialogueIndex = i;
                   break;
               }
           }
       }
   }
   
   // Everything correct
   public void response1() {
         dialogueBoxController.StartDialogue(feedbackDialogueTree, 0, npcName);
   }
// Forgot basket
   public void response2() {
         dialogueBoxController.StartDialogue(feedbackDialogueTree, 1, npcName);
   }
// Wrong condition factor
   public void response3() {
         dialogueBoxController.StartDialogue(feedbackDialogueTree, 2, npcName);
   }
// Wrong length
   public void response4() {
         dialogueBoxController.StartDialogue(feedbackDialogueTree, 3, npcName);
   }
// Wrong weight
   public void response5() {
         dialogueBoxController.StartDialogue(feedbackDialogueTree, 4, npcName);
   }
// Everything is wrong
   public void response6() {
         dialogueBoxController.StartDialogue(feedbackDialogueTree, 5, npcName);
   }
   
   // When the player places a object on the weight scale before turning it on
   public void Error1() {
         
         dialogueBoxController.StartDialogue(errorFeedbackDialogueTree, 0, npcName);
         
   }
   
   // When the player places the wrong object on the weight scale
   public void Error2() {
         dialogueBoxController.StartDialogue(errorFeedbackDialogueTree, 2, npcName);
   }
   
   // When the player places the fish without using the basket
   public void Error3() {
         GameObject[] instances = GameObject.FindGameObjectsWithTag("Fish");
         foreach (GameObject instance in instances)
         {
             Destroy(instance);
         }
         Instantiate(fishPrefab, _fishPosition, _fishRotation);
         dialogueBoxController.StartDialogue(errorFeedbackDialogueTree, 3, npcName);
   }
   
   // When the player places the fish before turning on the weight
   public void Error4() {
       GameObject[] instances = GameObject.FindGameObjectsWithTag("Fish");
       foreach (GameObject instance in instances)
       {
           Destroy(instance);
       }
       Instantiate(fishPrefab, _fishPosition, _fishRotation);
       dialogueBoxController.StartDialogue(errorFeedbackDialogueTree, 0, npcName);
   }
   
   public void Error5() {
         dialogueBoxController.StartDialogue(errorFeedbackDialogueTree, 4, npcName);
   }
   
   public void ChangeToLars(string dialogueName)
   {
       Debug.Log(dialogueName);
       if (dialogueName == "NpcFeedback")
       {
           dialogueBoxController.StartDialogue(larsDialogueTree, 1, npcName);
       
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
       else if (dialogueName == "ErrorFeedback")
       {
           dialogueBoxController.StartDialogue(larsDialogueTree, _dialogueIndex, npcName);
       }

   }
   
}
