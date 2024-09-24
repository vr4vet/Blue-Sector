using System;
using System.Collections;
using System.Collections.Generic;
using InteractiveCalculator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
   public string npcName;
   public GameObject basket;
   public GameObject fish;
   public GameObject scale;
   public GameObject calculator;
   public GameObject numPad;
   public GameObject handheldCounter;
   public GameObject slider;

   private DialogueBoxController dialogueBoxController;

   private void Start()
   {
       dialogueBoxController = FindObjectOfType<DialogueBoxController>();
       // fish.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
   }  
   
   private void Update()
   {
       if (dialogueBoxController.dialogueTreeRestart != null)
       {
           if (dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[0].dialogue[1])
           {
               ChangeToLars();
           
           }
       }

   }
   
   private void OnTriggerEnter(Collider other)
   {
      triggerEvent.Invoke();
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
   
   public void ChangeToLars()
   {
       dialogueBoxController.StartDialogue(larsDialogueTree, 1, npcName);
       
       basket.transform.position = new Vector3(-3.2309f, 0.874f, -1.8569f);
       basket.transform.rotation = Quaternion.Euler(270f, 358.180023f, 0.0f);
         
       // fish.transform.position = new Vector3(-3.2342f, 0.895799994f, -2.30819988f);
       
       scale.GetComponent<Scale>().totalWeight = 0;
       
       calculator.GetComponent<Calculator>().OnPressedClearAll();
       
       numPad.GetComponent<ResultLogger>().SwitchToCondition();
       numPad.GetComponent<ResultLogger>().SwitchToLength();
       numPad.GetComponent<ResultLogger>().SwitchToWeight();
       numPad.GetComponent<ResultLogger>().weight.SetText("Weight");
       numPad.GetComponent<ResultLogger>().length.SetText("Length");
       numPad.GetComponent<ResultLogger>().conditionFactor.SetText("Condition Factor");
       
       handheldCounter.transform.position = new Vector3(-3.19779992f, 0.883899987f, 1.10399997f);
       handheldCounter.transform.rotation = Quaternion.Euler(270f, 76.1088028f, 0.0f);
       
       slider.transform.position = new Vector3(-3.21790004f, 0.862619996f, 1.67770004f);
       slider.GetComponent<MicroscopeSlide>().RemoveMicroscopeSlide();

   }
   
}
