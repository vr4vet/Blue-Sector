// Purpose: Manages AI context, recording trigger, and AI request initiation.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Keep for potential future direct input mapping

// Note: Ensure Whisper namespace exists if Transcribe uses it directly
#if WHISPER_UNITY_PACKAGE_AVAILABLE // Example conditional compile
using Whisper;
#endif

public class AIConversationController : MonoBehaviour
{
    private ActionManager actionManager;

    // Dependencies (Assign in Inspector or find dynamically)
    [SerializeField] private Transcribe _Transcribe;
    [SerializeField] public SpriteRenderer microphoneIcon; // Assign the mic icon child object in Inspector

    // Configuration (Set via NPCSpawner from NPC Scriptable Object)
    [TextArea(3, 10)]
    public string contextPrompt;
    public int maxTokens = 150; // Adjusted default
    public List<Message> messages = new List<Message>();

    // Internal References
    private ConversationController _conversationController; // Found on child object
    private DialogueBoxController _dialogueBoxController; // Found on same object

    // Mic Animation State
    public float animationSpeed = 1.0f;
    public float minScale = 0.8f; // Adjusted scale
    public float maxScale = 1.2f; // Adjusted scale
    private bool growing = true;
    private float currentScale;
    private bool isAnimating = false;

    void Awake()
    {
        actionManager = ActionManager.Instance;
    }
    void Start()
    {
        // Find required components dynamically if not assigned
        if (_Transcribe == null)
        {
            GameObject transcriptionManager = GameObject.Find("TranscriptionManager"); // Or use a singleton pattern
            if (transcriptionManager != null)
            {
                _Transcribe = transcriptionManager.GetComponent<Transcribe>();
            }
            if (_Transcribe == null)
            {
                Debug.LogError("AIConversationController: Transcribe component not found on TranscriptionManager GameObject.", this);
            }
        }

        _conversationController = GetComponentInChildren<ConversationController>();
        if (_conversationController == null)
        {
            Debug.LogError("AIConversationController: ConversationController not found in children.", this);
        }

        _dialogueBoxController = GetComponent<DialogueBoxController>();
        if (_dialogueBoxController == null)
        {
            Debug.LogError("AIConversationController: DialogueBoxController not found on the same GameObject.", this);
        }


        // Set up microphone animation state
        currentScale = maxScale;
        if (microphoneIcon != null)
        {
            microphoneIcon.enabled = false; // Initially hide
        }
        else
        {
            Debug.LogWarning("AIConversationController: microphoneIcon SpriteRenderer is not assigned.", this);
        }
        isAnimating = false;
    }

    // Called by ConversationController when VR input is detected
    public void TriggerRecording(bool start)
    {
        if (_Transcribe == null)
        {
            Debug.LogError("Cannot trigger recording: Transcribe component is missing.", this);
            return;
        }

        if (start)
        {
            Debug.Log("AIConversationController: Starting Recording");
            _Transcribe.StartRecording(this); // Start transcribing and set reference to this conversation
            if (microphoneIcon != null) microphoneIcon.enabled = true;
            isAnimating = true; // Start mic animation
            if (_dialogueBoxController != null) _dialogueBoxController.startThinking(); // Show listening animation
        }
        else
        {
            Debug.Log("AIConversationController: Ending Recording");
            isAnimating = false; // Stop mic animation
            if (microphoneIcon != null) microphoneIcon.enabled = false; // Hide the microphone icon
            _Transcribe.EndRecording(); // Stop transcribing
            // Listening animation stopped in DialogueBoxController via AIRequest->DisplayResponse/Thinking flow
        }
    }

    public Transcribe GetTranscribe()
    {
        return this._Transcribe;
    }

    // --- Deprecated Direct Input Handling (Keep for reference, but use TriggerRecording via ConversationController) ---
    // public void PressButton(InputAction.CallbackContext context)
    // {
    //     if (_conversationController == null || _dialogueBoxController == null) return;

    //     bool canTalk = _conversationController.playerInsideTrigger && _dialogueBoxController.isTalkable && !_dialogueBoxController.dialogueEnded;

    //     if (context.started && canTalk)
    //     {
    //         TriggerRecording(true);
    //     }

    //     if (context.canceled && _Transcribe != null && _Transcribe.IsRecording()) // Check if actually recording before stopping
    //     {
    //         TriggerRecording(false);
    //     }
    // }
    // --- End Deprecated Input Handling ---

    // Called by Transcribe when transcription is finished
    public void CreateRequest(string transcript)
    {
        if (string.IsNullOrWhiteSpace(transcript) || transcript.ToLowerInvariant().Contains("inaudible") || transcript.ToLowerInvariant().Contains("blank_audio"))
        {
            // Handle poor transcription - maybe provide canned response?
            transcript = "I'm sorry, I couldn't understand what you said. Could you please try again? Make sure to hold the button while speaking.";
            Debug.Log("Transcription was empty or inaudible. Using fallback prompt.");
            HandleFallbackResponse(transcript);
            return;
        }
        else
        {
            Debug.Log($"AIConversationController: Creating AIRequest with transcript: '{transcript}'");
            // Add components and create OpenAI query based on transcript
            AIRequest request = gameObject.AddComponent<AIRequest>(); // Add dynamically
            request.query = transcript + " Please answer concisely and in the language: " + _Transcribe.currentLanguage; // Add language constraint
            request.maxTokens = this.maxTokens;
            request.requestPayload = actionManager.GetUploadData();
            // References will be fetched by AIRequest's Start/Awake
        }
    }

    // Handle cases where we don't call OpenAI (e.g., bad transcription)
    void HandleFallbackResponse(string fallbackText)
    {
        Debug.Log($"AIConversationController: Handling fallback response: '{fallbackText}'");
        if (_dialogueBoxController != null)
        {
            _dialogueBoxController.stopThinking();
            StartCoroutine(_dialogueBoxController.DisplayResponse(fallbackText));
        }
        else
        {
            Debug.LogError("DialogueBoxController is null. Cannot display fallback response.");
        }
    }


    // Called by NPCSpawner and AIRequest to manage context
    public void AddMessage(Message message)
    {
        // Optional: Limit context window size
        // const int maxMessages = 10; // Example limit
        // if (messages.Count >= maxMessages) {
        //    messages.RemoveAt(1); // Remove oldest user/assistant message (keep system prompts at index 0)
        // }
        if (actionManager != null)
        {
            actionManager.AddChatMessage(message); // Add message to global chatlog
        }
        else
        {
            Debug.LogWarning("AIConversationController: actionManager is null, message will only be added to local context", this);
        }
        messages.Add(message);
        Debug.Log($"AIContext: Added '{message.role}' message. Total messages: {messages.Count}");
    }

    // Update is used for animations only now
    void Update()
    {
        // Animation of microphone
        if (isAnimating && microphoneIcon != null)
        {
            AnimateMicrophoneIcon();
        }
    }

    private void AnimateMicrophoneIcon()
    {
        if (growing)
        {
            currentScale += animationSpeed * Time.deltaTime * 2f; // Use Time.deltaTime
            if (currentScale >= maxScale)
            {
                currentScale = maxScale;
                growing = false;
            }
        }
        else
        {
            currentScale -= animationSpeed * Time.deltaTime * 2f; // Use Time.deltaTime
            if (currentScale <= minScale)
            {
                currentScale = minScale;
                growing = true;
            }
        }
        microphoneIcon.transform.localScale = new Vector3(currentScale, currentScale, 1);
    }

    public void PopulateGlobalMemory()
    {
        messages = actionManager.GetGlobalChatLogs();
    }
}