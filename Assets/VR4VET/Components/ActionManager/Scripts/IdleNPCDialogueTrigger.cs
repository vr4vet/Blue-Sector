using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that connects the IdleTimer to NPC dialogue
/// Makes the nearest NPC speak when player is idle
/// </summary>
public class IdleNPCDialogueTrigger : MonoBehaviour
{
    [Tooltip("Tag used to identify NPCs in the scene")]
    public string npcTag = "NPC";

    [Tooltip("Maximum distance to search for NPCs")]
    public float maxSearchDistance = 20f;

    [Tooltip("Name to use for the NPC that speaks idle dialogue")]
    public string defaultNpcName = "Assistant";

    [Tooltip("Reference to the idle dialogue tree asset")]
    public DialogueTree idleDialogueTree;

    [Tooltip("Section index to use in the idle dialogue tree")]
    public int idleDialogueSectionIndex = 0;

    private IdleTimer idleTimer;

    private void Awake()
    {
        // Get reference to the IdleTimer component
        idleTimer = GetComponent<IdleTimer>();
        
        if (idleTimer == null)
        {
            Debug.LogError("IdleNPCDialogueTrigger requires an IdleTimer component on the same GameObject!");
            enabled = false;
            return;
        }

        // Initialize the UnityEvent if it's null
        if (idleTimer.OnIdleThresholdReached == null)
        {
            idleTimer.OnIdleThresholdReached = new UnityEngine.Events.UnityEvent<string>();
        }

        // Subscribe to the idle event
        idleTimer.OnIdleThresholdReached.AddListener(HandleIdleThresholdReached);
    }

    private void HandleIdleThresholdReached(string idlePrompt)
    {
        // Debug the idle prompt
        Debug.Log($"Idle dialogue prompt: {idlePrompt}");

        // Find the closest NPC
        GameObject nearestNPC = FindNearestNPC();

        if (nearestNPC == null)
        {
            Debug.LogWarning("No NPC found to speak idle dialogue");
            return;
        }

        // Get NPC components
        NpcTriggerDialogue npcTrigger = nearestNPC.GetComponent<NpcTriggerDialogue>();
        if (npcTrigger != null)
        {
            Debug.Log($"Found NPC with NpcTriggerDialogue: {nearestNPC.name}");
            MakeNPCSpeak(npcTrigger, idlePrompt);
        }
        else
        {
            // Try to find the components in children
            npcTrigger = nearestNPC.GetComponentInChildren<NpcTriggerDialogue>();
            if (npcTrigger != null)
            {
                Debug.Log($"Found NPC with NpcTriggerDialogue in children: {nearestNPC.name}");
                MakeNPCSpeak(npcTrigger, idlePrompt);
            }
            else
            {
                Debug.LogWarning($"NPC {nearestNPC.name} doesn't have NpcTriggerDialogue component");
            }
        }
    }

    private void MakeNPCSpeak(NpcTriggerDialogue npcTrigger, string idlePrompt)
    {
        // Get the dialogue box controller
        DialogueBoxController dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        if (dialogueBoxController == null)
        {
            Debug.LogError("Could not find DialogueBoxController in the scene");
            return;
        }

        // Get the conversation controller from the NPC
        ConversationController conversationController = npcTrigger.GetComponentInChildren<ConversationController>();
        if (conversationController == null)
        {
            Debug.LogError($"Could not find ConversationController on NPC {npcTrigger.name}");
            return;
        }

        // Check if we have a valid idle dialogue tree assigned
        if (idleDialogueTree != null)
        {
            // Use the assigned idle dialogue tree
            dialogueBoxController.StartDialogue(idleDialogueTree, idleDialogueSectionIndex, npcTrigger.npcName, 0);
            Debug.Log($"Started idle dialogue on NPC {npcTrigger.npcName} using assigned dialogue tree");
        }
        else
        {
            // If no idle dialogue tree is assigned, try to use the ActionManager to send the report
            if (ActionManager.Instance != null)
            {
                Debug.Log($"Using ActionManager to handle idle dialogue for {npcTrigger.npcName}");
                ActionManager.Instance.SendIdleTimeoutReport(idlePrompt);
                
                // For immediate visual feedback, we can also make the NPC speak directly
                // using a simple dialogue message if possible
                TryDirectDialogue(dialogueBoxController, npcTrigger.npcName, idlePrompt);
            }
            else
            {
                Debug.LogWarning("ActionManager instance is null, cannot send idle timeout report");
                
                // Try direct dialogue as fallback
                TryDirectDialogue(dialogueBoxController, npcTrigger.npcName, idlePrompt);
            }
        }
    }

    // Helper method to try direct dialogue without a dialogue tree
    private void TryDirectDialogue(DialogueBoxController dialogueBoxController, string npcName, string message)
    {
        // This is a fallback method that might work depending on how your DialogueBoxController is implemented
        // You may need to adjust this based on your actual implementation
        try
        {
            // Try to access the dialogue text directly if possible
            var dialogueText = dialogueBoxController._dialogueText;
            if (dialogueText != null)
            {
                dialogueText.text = message;
                Debug.Log($"Set dialogue text directly for NPC {npcName}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to set dialogue text directly: {e.Message}");
        }
    }

    private GameObject FindNearestNPC()
    {
        // Find all NPCs in the scene with the specified tag
        GameObject[] npcs = GameObject.FindGameObjectsWithTag(npcTag);
        
        if (npcs.Length == 0)
        {
            Debug.LogWarning($"No GameObjects with tag '{npcTag}' found in the scene");
            return null;
        }

        // Find the closest NPC to the player
        GameObject closestNPC = null;
        float closestDistance = maxSearchDistance;
        Vector3 playerPosition = Camera.main.transform.position; // Use main camera position as player position
        
        foreach (GameObject npc in npcs)
        {
            float distance = Vector3.Distance(playerPosition, npc.transform.position);
            if (distance < closestDistance)
            {
                closestNPC = npc;
                closestDistance = distance;
            }
        }
        
        if (closestNPC != null)
        {
            Debug.Log($"Found nearest NPC: {closestNPC.name} at distance {closestDistance}m");
        }
        
        return closestNPC;
    }
}