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
            if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[3].dialogue[1]) 
            {
                dialogueBoxController.SkipLine();
                
            }
           
            

            float lengt = collision.GetComponent<Weight>().fish.transform.localScale.x;
            float fishLength = (float)(4.58 * Math.Exp(2.33 * lengt) + 10.31);
            if (!fishLengths.ContainsKey(collision.GetComponent<Weight>().fish))
            {
                fishLengths.Add(collision.GetComponent<Weight>().fish, fishLength);
            }
        }
    }
}