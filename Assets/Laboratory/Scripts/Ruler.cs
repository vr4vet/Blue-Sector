using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruler : MonoBehaviour
{
    private DialogueBoxController dialogueBoxController;


    
    private Dictionary<GameObject,float> fishLengths = new Dictionary<GameObject, float>();

    


    private void Start()
    {
        
        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        
    }

    private void OnTriggerEnter(Collider collision) 
    {
        if (collision.GetType() == typeof(CapsuleCollider) && collision.GetComponent<Weight>())
        {
            if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[5].dialogue[0]) 
            {
                dialogueBoxController.SkipLine();
                
            }
        }
    }
}