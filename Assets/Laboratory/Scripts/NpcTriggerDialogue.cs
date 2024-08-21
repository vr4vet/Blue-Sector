using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcTriggerDialogue : MonoBehaviour
{
   // ----------------- Editor Variables -----------------
   [SerializeField]   
   public UnityEvent triggerEvent;
   [SerializeField]     
   public DialogueTree dialogueTree;
   [SerializeField]  
   public string npcName;

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
         dialogueBoxController.StartDialogue(dialogueTree, 0, npcName);
   }
// Forgot basket
   public void response2() {
         dialogueBoxController.StartDialoguer(dialogueTree, 1, npcName);
   }
// Wrong condition factor
   public void response3() {
         dialogueBoxController.StartDialogue(dialogueTree, 2, npcName);
   }
// Wrong length
   public void response4() {
         dialogueBoxController.StartDialogue(dialogueTree, 3, npcName);
   }
// Wrong weight
   public void response5() {
         dialogueBoxController.StartDialogue(dialogueTree, 4, npcName);
   }
// Everything is wrong
   public void response6() {
         dialogueBoxController.StartDialogue(dialogueTree, 5, npcName);
   }
   
}
