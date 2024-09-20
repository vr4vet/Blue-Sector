using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using BNG;

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
   public DialogueTree conditionFactorDialogueTree;
   [SerializeField]
   public DialogueTree microscopeDialogueTree;
   [SerializeField]
   public DialogueTree dissectDialogueTree;
   [SerializeField]  
   public string npcName;

   private DialogueBoxController dialogueBoxController;

   private void Start()
   {
       dialogueBoxController = FindObjectOfType<DialogueBoxController>();
       ButtonSpawner.OnAnswer += ButtonSpawner_OnAnswer;
   }  
   
   private void Update()
   {
       if (dialogueBoxController.dialogueTreeRestart.name == "NpcFeedback" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[6].dialogue[0])
       {
           ChangeToLars();
           
       }

       /*if (dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[3].dialogue[0])
       {
           dialogueBoxController.StartDialogue(conditionFactorDialogueTree, 0, npcName);
       }*/
   }

   private void ButtonSpawner_OnAnswer(string answer)
   {
       if (dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue")
       {
           if (answer == "Calculating condition factor")
           {
               dialogueBoxController.StartDialogue(conditionFactorDialogueTree, 0, npcName);
           }
           else if (answer == "Analyzing plankton samples")
           {
               dialogueBoxController.StartDialogue(microscopeDialogueTree, 0, npcName);
           }
           else if (answer == "Dissecting fish (Still in development)")
           {
               dialogueBoxController.StartDialogue(dissectDialogueTree, 0, npcName);
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
       dialogueBoxController.StartDialogue(larsDialogueTree, 12, npcName);
   }
   
}
