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
        
        // Check for NavMeshAgent and remove it for flying fish
        if (fishController != null && fishController.CanFly)
        {
            UnityEngine.AI.NavMeshAgent navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent != null)
            {
                navAgent.enabled = false;
                Destroy(navAgent);
                Debug.Log("Removed NavMeshAgent from flying fish chatbot");
            }
        }
    }
    
    private void Start()
    {
        // Force teleport to player at startup to ensure correct positioning
        if (fishController != null)
        {
            // Execute with a delay to ensure everything is initialized
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
        
        if (fishController != null)
        {
            // Force teleport to player
            fishController.TeleportToPlayer();
            Debug.Log("Fish teleported to player from ChatbotController");
            
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
    
    private void OnEnable()
    {
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
        // Stop all coroutines when disabled
        StopAllCoroutines();
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
            // Just deactivate it so it can be reused later
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