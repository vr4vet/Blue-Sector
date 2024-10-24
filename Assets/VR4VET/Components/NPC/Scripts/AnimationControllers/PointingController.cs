using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Tutorials.Core.Editor;
using UnityEngine;

public class PointingController : MonoBehaviour
{
    public GameObject npc;
    private GameObject ObjectToLookAt;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    
    
    private DialogueBoxController dialogueBoxController;
    // Start is called before the first frame update
    void Start()
    {
        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        // Get the NPC object
        npc = GameObject.Find("Ch16_nonPBR(Clone)");
        // Save the initial position of the NPC
        _initialPosition = npc.transform.position;
        _initialRotation = npc.transform.rotation;
        
    }
    
    public void ChangeDirection(string dialogueText)
    {
        npc.transform.position = _initialPosition;
        
        // Set the direction the NPC should look at
        if (npc != null && dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue")
        {
            if (dialogueText.Contains("scale"))
            {
                ObjectToLookAt = GameObject.Find("scale");
            }
            
            else if (dialogueText.Contains("board"))
            {
                ObjectToLookAt = GameObject.Find("ruler");
            }
            
            else if (dialogueText.Contains("calculator"))
            {
                ObjectToLookAt = GameObject.Find("Calculator_1978");
            }
            else if (dialogueText.Contains("microscope"))
            {
                ObjectToLookAt = GameObject.Find("MicroscopeComplete");
            }
            else if (dialogueText.Contains("number keypad"))
            {
                ObjectToLookAt = GameObject.Find("NumPad");
            }
            
            Vector3 direction = ObjectToLookAt.transform.position - npc.transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                npc.transform.rotation = Quaternion.LookRotation(direction);
            }
        } 
        
    }

    public void ResetDirection()
    {
        npc.transform.rotation = _initialRotation;
    }
    
}


