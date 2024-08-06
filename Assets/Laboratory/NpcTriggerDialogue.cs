using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcTriggerDialogue : MonoBehaviour
{
   public UnityEvent triggerEvent;

   public DialogueTree dialogueTree;

   public int section;

   public string name;

   private DialogueBoxController dialogueBoxController;

   private void Start()
   {
       dialogueBoxController = FindObjectOfType<DialogueBoxController>();
   }  
   
   private void OnTriggerEnter(Collider other)
   {
       triggerEvent.Invoke();

   }
   
   // Everything correct
   public void response1() {
         dialogueBoxController.StartDialogue(dialogueTree, 0, name);
   }
// Forgot basket
   public void response2() {
         dialogueBoxController.StartDialogue(dialogueTree, 1, name);
   }
// Wrong condition factor
   public void response3() {
         dialogueBoxController.StartDialogue(dialogueTree, 2, name);
   }
// Wrong length
   public void response4() {
         dialogueBoxController.StartDialogue(dialogueTree, 3, name);
   }
// Wrong weight
   public void response5() {
         dialogueBoxController.StartDialogue(dialogueTree, 4, name);
   }
// Everything is wrong
   public void response6() {
         dialogueBoxController.StartDialogue(dialogueTree, 5, name);
   }
   
}
