using System;
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
    
    [Tooltip("Set to true to ensure this component works even if PlayerIdleDetector exists")]
    public bool overridePlayerIdleDetector = true;
    
    [Tooltip("Debug mode - logs extra information")]
    public bool debugMode = true;

    private IdleTimer idleTimer;
    private PlayerIdleDetector[] existingIdleDetectors;
    private bool hasConflictingDetectors = false;

    private void Awake()
    {
        // Check for potentially conflicting PlayerIdleDetector components
        existingIdleDetectors = FindObjectsOfType<PlayerIdleDetector>();
        hasConflictingDetectors = existingIdleDetectors != null && existingIdleDetectors.Length > 0;
        
        if (hasConflictingDetectors && debugMode)
        {
            Debug.Log($"[IdleNPCDialogueTrigger] Found {existingIdleDetectors.Length} PlayerIdleDetector components in the scene");
            
            if (overridePlayerIdleDetector)
            {
                Debug.Log("[IdleNPCDialogueTrigger] Will override PlayerIdleDetector(s) as configured");
            }
            else
            {
                Debug.Log("[IdleNPCDialogueTrigger] Will coexist with PlayerIdleDetector(s) - watch for potential conflicts");
            }
        }
        
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
        
        // If we're overriding the PlayerIdleDetector and it exists,
        // disable its Update method using a lambda to cleanly deactivate it
        if (overridePlayerIdleDetector && hasConflictingDetectors)
        {
            foreach (var detector in existingIdleDetectors)
            {
                if (detector != null && detector.enabled)
                {
                    if (debugMode)
                    {
                        Debug.Log($"[IdleNPCDialogueTrigger] Disabling PlayerIdleDetector on {detector.gameObject.name}");
                    }
                    
                    // Disable it - this is the cleaner approach that doesn't modify the script
                    detector.enabled = false;
                }
            }
        }
    }

    private void HandleIdleThresholdReached(string idlePrompt)
    {
        // Debug the idle prompt
        if (debugMode)
        {
            Debug.Log($"[IdleNPCDialogueTrigger] Idle dialogue prompt: {idlePrompt}");
        }

        // Use ActionManager to handle idle messages
        if (ActionManager.Instance != null)
        {
            // Let ActionManager handle finding the nearest NPC and making it speak
            ActionManager.Instance.SendIdleTimeoutReport(idlePrompt);
            
            if (debugMode)
            {
                Debug.Log("[IdleNPCDialogueTrigger] Sent idle timeout report to ActionManager");
            }
        }
        else
        {
            Debug.LogError("[IdleNPCDialogueTrigger] ActionManager instance is null, cannot send idle prompt");
        }
    }
    
    /// <summary>
    /// Check for issues with IdleTimer setup
    /// </summary>
    private void OnEnable()
    {
        if (idleTimer != null)
        {
            // Check if there's a default value for the idle threshold
            System.Reflection.FieldInfo fieldInfo = typeof(IdleTimer).GetField("defaultIdleThresholdInSeconds", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (fieldInfo != null)
            {
                float value = (float)fieldInfo.GetValue(idleTimer);
                if (value <= 0)
                {
                    Debug.LogWarning("[IdleNPCDialogueTrigger] IdleTimer has a defaultIdleThresholdInSeconds value of 0 or less. This will prevent idle detection.");
                }
            }
        }
    }
}