using UnityEngine;
using System.Collections;

/// <summary>
/// A simple animator controller for fish NPCs that provides compatibility
/// with systems expecting humanoid animations.
/// </summary>
public class FishAnimatorController : MonoBehaviour
{
    // Reference to the animator component 
    private Animator animator;
    
    // Required animation parameter names used by ConversationController
    private readonly string TALK_TRIGGER = "Talk";
    private readonly string RANDOM_TALK_TRIGGER = "RandomTalk";
    private readonly string HEAD_BOBBLE_TRIGGER = "HeadBobble";
    private readonly string IDLE_ANIMATION = "Idle";
    
    // Required animation parameters used by DialogueBoxController
    private readonly string IS_TALKING_BOOL = "isTalking";
    private readonly string HAS_NEW_DIALOGUE_OPTIONS_BOOL = "hasNewDialogueOptions";
    private readonly string IS_POINTING_BOOL = "isPointing";
    private readonly string IS_LISTENING_BOOL = "isListening";
    
    // Reference to our fish controller for actual animations
    private SimpleFishController fishController;
    
    // Track if we've already checked for missing parameters
    private bool addedMissingParameters = false;
    
    private void Awake()
    {
        // Get or add required components
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = gameObject.AddComponent<Animator>();
            Debug.Log("Added Animator component to FishAnimatorController");
        }
        
        // Get fish controller or add one
        fishController = GetComponent<SimpleFishController>();
        if (fishController == null)
        {
            fishController = gameObject.AddComponent<SimpleFishController>();
            Debug.Log("Added SimpleFishController component to FishAnimatorController");
        }
        
        // Ensure animation parameters are handled
        EnsureParametersHandled();
    }
    
    private void OnEnable()
    {
        // We need to detect animation triggers that other scripts might try to call
        if (animator != null)
        {
            // Add missing parameters if needed
            if (!addedMissingParameters)
            {
                EnsureParametersHandled();
            }
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
        // Guard against animator errors - only check parameters if we have a valid controller
        if (animator != null)
        {
            // Check if DialogueBoxController's isTalking parameter is set
            if (CheckAnimatorBool(IS_TALKING_BOOL))
            {
                // Tell the fish to animate talking but DISABLE MOVEMENT while talking
                if (fishController != null)
                {
                    fishController.SetTalking(true);
                    // Critical fix: Disable follow behavior while talking
                    fishController.SetFollowPlayerEnabled(false);
                }
            }
            else
            {
                // If not talking, make sure the fish is not in talking animation
                if (fishController != null)
                {
                    fishController.SetTalking(false);
                    // Re-enable follow if needed (with moderate distance)
                    fishController.SetFollowPlayerEnabled(true);
                }
            }
            
            // Handle listening animation
            if (CheckAnimatorBool(IS_LISTENING_BOOL))
            {
                // Make fish bob up and down more intensely
                if (fishController != null)
                {
                    fishController.SetIntensity(1.5f);
                    // Critical fix: Disable follow during listening as well
                    fishController.SetFollowPlayerEnabled(false);
                }
            }
            else
            {
                // Back to normal intensity
                if (fishController != null)
                {
                    fishController.SetIntensity(1.0f);
                }
            }
            
            // Standard ConversationController compat
            if (CheckAnimatorTrigger(TALK_TRIGGER))
            {
                // Tell the fish to animate talking
                if (fishController != null)
                {
                    fishController.SetTalking(true);
                    // Critical fix: Disable follow behavior while talking
                    fishController.SetFollowPlayerEnabled(false);
                }
                
                // Reset the trigger after detecting it
                ResetAnimatorTrigger(TALK_TRIGGER);
            }
        }
        else if (animator != null && animator.runtimeAnimatorController == null)
        {
            // Log this only once to avoid spam
            if (!loggedMissingControllerWarning)
            {
                Debug.LogWarning("FishAnimatorController: No RuntimeAnimatorController assigned to Animator - some animation features will be disabled");
                loggedMissingControllerWarning = true;
            }
        }
    }
    
    // Flag to ensure we only log the warning once
    private bool loggedMissingControllerWarning = false;
    
    // Public method to set talking state
    public void SetTalking(bool talking)
    {
        if (fishController != null)
        {
            fishController.SetTalking(talking);
        }
    }

    // Safely check if a bool parameter exists and is set
    private bool CheckAnimatorBool(string paramName)
    {
        if (animator == null) return false;
        
        // Use HasParameter to check if the parameter exists
        if (HasParameter(paramName))
        {
            try
            {
                return animator.GetBool(paramName);
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        return false;
    }
    
    // Safely check if a trigger parameter has been activated
    private bool CheckAnimatorTrigger(string paramName)
    {
        // We can't directly check if a trigger is active in the Animator API
        // So we use a workaround with our own tracking
        
        if (animator == null) return false;
        
        // Check if the parameter exists
        if (HasParameter(paramName))
        {
            // For triggers, we can only reset them (not get their value)
            // Just check if the parameter exists
            return true;
        }
        return false;
    }
    
    // Helper to check if a parameter exists
    private bool HasParameter(string paramName)
    {
        if (animator == null) return false;
        
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
    
    // Safely set a bool parameter if it exists
    private void SetAnimatorBool(string paramName, bool value)
    {
        if (animator == null) return;
        
        // Use HasParameter to check if the parameter exists
        if (HasParameter(paramName))
        {
            try
            {
                animator.SetBool(paramName, value);
            }
            catch (System.Exception)
            {
                // Parameter doesn't exist, silently ignore
            }
        }
    }
    
    // Safely reset a trigger parameter if it exists
    private void ResetAnimatorTrigger(string paramName)
    {
        if (animator == null) return;
        
        // Use HasParameter to check if the parameter exists
        if (HasParameter(paramName))
        {
            try
            {
                animator.ResetTrigger(paramName);
            }
            catch (System.Exception)
            {
                // Parameter doesn't exist, silently ignore
            }
        }
    }
    
    // Method that can be called directly by ConversationController
    public void OnTalkAnimationStarted()
    {
        if (fishController != null)
        {
            fishController.SetTalking(true);
        }
    }
    
    // Method that can be called directly by ConversationController
    public void OnTalkAnimationEnded()
    {
        if (fishController != null)
        {
            fishController.SetTalking(false);
        }
    }
    
    // Hook into various animation events that other systems might call
    public void HeadBobble()
    {
        if (fishController != null)
        {
            // Make the fish do a quick more intense movement
            fishController.SwimTo(
                transform.position + transform.up * 0.1f, 0.3f);
        }
    }
    
    // Called to ensure the controller handles all necessary parameters
    public void EnsureParametersHandled()
    {
        // Reset talking and listening states
        if (fishController != null)
        {
            fishController.SetTalking(false);
            fishController.SetIntensity(1.0f);
        }
        
        // If any DialogueBoxController is looking for us, update its animator reference
        DialogueBoxController dialogueController = GetComponent<DialogueBoxController>();
        if (dialogueController != null && animator != null)
        {
            dialogueController.updateAnimator(animator);
            addedMissingParameters = true;
        }
    }
}