using System.Collections;
using UnityEngine;
using Meta.WitAi.TTS.Utilities;

/// <summary>
/// Controls the behavior of the Chatbot prefab that appears when the player is idle and no NPCs are nearby.
/// </summary>
public class ChatbotController : MonoBehaviour
{
    // Time in seconds before the Chatbot automatically disappears after speaking
    [SerializeField] private float autoDisappearDelay = 30f;
    
    // Reference to the DialogueBoxController (required)
    private DialogueBoxController dialogueController;
    
    // Reference to the AIConversationController (optional)
    private AIConversationController conversationController;
    
    // Reference to the SimpleFishController for fish-specific animations
    private SimpleFishController fishController;
    
    // Reference to the TTSSpeaker component for voice output
    private TTSSpeaker ttsSpeaker;
    
    // Track if the Chatbot is currently speaking
    private bool isSpeaking = false;
    
    private void Awake()
    {
        // Get required components
        dialogueController = GetComponent<DialogueBoxController>();
        if (dialogueController == null)
        {
            Debug.LogError("DialogueBoxController not found on Chatbot prefab!");
        }
        
        // Get optional conversation controller
        conversationController = GetComponent<AIConversationController>();
        
        // Get fish controller if present
        fishController = GetComponent<SimpleFishController>();
        
        // Get TTSSpeaker component
        ttsSpeaker = GetComponentInChildren<TTSSpeaker>();
        if (ttsSpeaker == null)
        {
            Debug.LogWarning("TTSSpeaker not found on Chatbot or its children. Voice output won't be available.");
        }
        
        // Check for NavMeshAgent and remove/disable if we're using flying fish
        if (fishController != null && fishController.CanFly)
        {
            UnityEngine.AI.NavMeshAgent navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent != null)
            {
                // Disable immediately to prevent any position reset
                navAgent.enabled = false;
                Destroy(navAgent); // Remove completely
                Debug.Log("Removed NavMeshAgent from flying fish chatbot");
            }
        }
    }
    
    private void Start()
    {
        // Force teleport to player at startup to ensure correct positioning
        if (fishController != null)
        {
            // Execute in the next frame to ensure everything is initialized
            StartCoroutine(TeleportWithDelay(0.2f));
            
            // Ensure flying is enabled
            fishController.SetFlyingEnabled(true);
            fishController.SetFollowPlayerEnabled(true);
            
            Debug.Log("Fish flying and following enabled in ChatbotController Start");
        }
    }

    // Delay teleport to ensure everything is initialized
    private IEnumerator TeleportWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Double-check that we still have a valid fish controller
        if (fishController != null)
        {
            // Force teleport to player
            fishController.TeleportToPlayer();
            Debug.Log("Fish teleported to player from ChatbotController");
            
            // Additional check - if position is at origin 0,0,0 then try again
            yield return new WaitForSeconds(0.2f);
            
            if (transform.position == Vector3.zero)
            {
                Debug.LogWarning("Fish is at 0,0,0 - attempting second teleport");
                fishController.TeleportToPlayer();
                
                // If still at origin, force manual position
                yield return new WaitForSeconds(0.2f);
                if (transform.position == Vector3.zero && Camera.main != null)
                {
                    Vector3 playerPos = Camera.main.transform.position;
                    Vector3 playerForward = Camera.main.transform.forward;
                    playerForward.y = 0;
                    playerForward.Normalize();
                    
                    // Position directly in front of player
                    transform.position = playerPos + playerForward * 1.5f;
                    transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));
                    Debug.Log("Forced fish position directly in front of player");
                }
            }
        }
    }
    
    private void OnEnable()
    {
        // Register for TTS events if available
        if (ttsSpeaker != null)
        {
            // Connect to the TextPlaybackStart/Finished events
            ttsSpeaker.Events.OnTextPlaybackStart.AddListener(OnTTSStarted);
            ttsSpeaker.Events.OnTextPlaybackFinished.AddListener(OnTTSFinished);
        }
        
        // Start checking for dialogue end
        if (dialogueController != null)
        {
            StartCoroutine(CheckForDialogueEnd());
        }
        
        // Ensure our position is good when enabled
        if (fishController != null && transform.position == Vector3.zero)
        {
            StartCoroutine(TeleportWithDelay(0.1f));
        }
    }
    
    private void OnDisable()
    {
        // Unregister from TTS events
        if (ttsSpeaker != null)
        {
            ttsSpeaker.Events.OnTextPlaybackStart.RemoveListener(OnTTSStarted);
            ttsSpeaker.Events.OnTextPlaybackFinished.RemoveListener(OnTTSFinished);
        }
        
        // Stop all coroutines when disabled
        StopAllCoroutines();
    }
    
    // TTS event handlers
    private void OnTTSStarted(string text)
    {
        Debug.Log("Chatbot TTS started: " + text);
        OnStartSpeaking();
    }
    
    private void OnTTSFinished(string text)
    {
        Debug.Log("Chatbot TTS finished: " + text);
        // Speech finished, but we'll let the dialogue end detection handle the cleanup
    }
    
    /// <summary>
    /// Called when the Chatbot starts speaking
    /// </summary>
    public void OnStartSpeaking()
    {
        isSpeaking = true;
        
        // Cancel any existing disappear coroutine
        StopAllCoroutines();
        
        // Start checking for dialogue end
        if (dialogueController != null)
        {
            StartCoroutine(CheckForDialogueEnd());
        }
        
        // Animate the fish if using SimpleFishController
        if (fishController != null)
        {
            fishController.SetTalking(true);
        }
    }
    
    /// <summary>
    /// Called when the Chatbot finishes speaking
    /// </summary>
    public void OnSpeechFinished()
    {
        isSpeaking = false;
        
        // Stop fish talking animation
        if (fishController != null)
        {
            fishController.SetTalking(false);
        }
        
        // Schedule auto-disappear after delay
        StartCoroutine(DisappearAfterDelay());
    }
    
    /// <summary>
    /// Coroutine to check if the dialogue has ended
    /// </summary>
    private IEnumerator CheckForDialogueEnd()
    {
        // Reset the dialogueEnded flag if it's already set
        if (dialogueController.dialogueEnded)
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        // Wait until dialogue has ended
        while (!dialogueController.dialogueEnded)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        // Dialogue has ended
        OnSpeechFinished();
    }
    
    /// <summary>
    /// Coroutine to make the Chatbot disappear after a delay
    /// </summary>
    private IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(autoDisappearDelay);
        
        // Only disappear if not currently speaking
        if (!isSpeaking)
        {
            // Option 1: Destroy the gameObject completely
            // Destroy(gameObject);
            
            // Option 2: Just deactivate it so it can be reused later
            gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Called when a player gets close to the Chatbot
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the player
        if (other.CompareTag("Player"))
        {
            // Reset disappear timer when player approaches
            StopAllCoroutines();
        }
    }
    
    /// <summary>
    /// Called when the player moves away from the Chatbot
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        // Check if it's the player
        if (other.CompareTag("Player"))
        {
            // Start disappear timer when player leaves
            if (!isSpeaking)
            {
                StartCoroutine(DisappearAfterDelay());
            }
        }
    }
}