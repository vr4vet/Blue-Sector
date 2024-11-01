using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Tutorials.Core.Editor;
using UnityEngine;

public class PointingController : MonoBehaviour
{
    private List<NpcData> npcs;
    private GameObject ObjectToLookAt;
    private DialogueBoxController dialogueBoxController;
    // Start is called before the first frame update
    void Start()
    {
        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        
        // This list will store necessary data for each NPC in the scene
        npcs = new List<NpcData>();
        
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

        // Loop through all the game objects in the scene
        foreach (GameObject obj in allGameObjects)
        {
            // Check if the name contains "PBR", which means it is an NPC model
            if (obj.name.Contains("PBR"))
            {
                // Store the initial position and rotation of the NPC which would be used to reset it position
                Vector3 initialPosition = obj.transform.position;
                Quaternion initialRotation = obj.transform.rotation;
                npcs.Add(new NpcData(obj, initialPosition, initialRotation));
            }
        }

        // Loop through the list of NPCs
        foreach (NpcData npcData in npcs)
        {
            /* Instead of keeping its generic model name it will be changed to the actual name of the NPC which the parent has.
            This is done to make it easier to identify the NPC later in the script */
            Transform parentTransform = npcData.Npc.transform.parent;
            if (parentTransform != null)
            {
                npcData.Npc.name = parentTransform.gameObject.name;
            }
            else
            {
                Debug.Log(npcData.Npc.name + " has no parent.");
            }
        }
    }
    
    public void ChangeDirection(string dialogueText, GameObject talkingNpc)
    {
        // Find the correct NPC based on the currently speaking NPC in the scene
        NpcData npcData = npcs.Find(npc => npc.Npc.name == talkingNpc.name);
        
        // Set the direction the NPC should look at
        if (npcData != null)
        {
            // Reset the position of the NPC to make sure it stays in the same place
            npcData.Npc.transform.position = npcData.InitialPosition;
            
            if (dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue")
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
            }
            
            // Look at the correct object based on the dialogue text
            Vector3 direction = ObjectToLookAt.transform.position - npcData.Npc.transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                npcData.Npc.transform.rotation = Quaternion.LookRotation(direction);
            }
        } 
        
    }

    // Reset the direction of the NPC
    public void ResetDirection(GameObject talkingNpc)
    {
        NpcData npcData = npcs.Find(npc => npc.Npc.name == talkingNpc.name);
        
        if (npcData != null)
        { 
            npcData.Npc.transform.rotation = npcData.InitialRotation;
        }
        
    }
    
}


