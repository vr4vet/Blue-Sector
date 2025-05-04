using System.Collections;
using System.Collections.Generic;
using Meta.WitAi;
using UnityEngine;

public class AIConversationController : MonoBehaviour
{
    private ActionManager _actionManager;

    // Dependencies (Assign in Inspector or find dynamically)
    [SerializeField] private Transcribe _Transcribe;
    [SerializeField] public SpriteRenderer microphoneIcon; // Assign the mic icon child object in Inspector
    private AIRequest _aiRequest;

    // Configuration (Set via NPCSpawner from NPC Scriptable Object)
    [TextArea(3, 10)]
    public string contextPrompt;
    public int maxTokens = 150; // Currently unused, but kept for future use
    public List<Message> messages = new List<Message>();

    // Internal References
    private ConversationController _conversationController;
    private DialogueBoxController _dialogueBoxController;

    /// <summary>
    /// Mic animation state.
    /// </summary>
    public float animationSpeed = 1.0f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    private bool growing = true;
    private float currentScale;
    private bool isAnimating = false;

    /// <summary>
    /// Initializes the ActionManager instance before the rest of the script is ran.
    /// </summary>
    void Awake()
    {
        _actionManager = ActionManager.Instance;
    }

    /// <summary>
    /// Initializes the AIConversationController.
    /// </summary>
    void Start()
    {
        if (_Transcribe == null)
        {
            GameObject transcriptionManager = GameObject.Find("TranscriptionManager");
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

    /// <summary>
    /// Returns the Transcribe component.
    /// </summary>
    /// <returns>Transcribe component</returns>
    public Transcribe GetTranscribe()
    {
        return this._Transcribe;
    }

    /// <summary>
    /// Creates an AIRequest based on the provided transcript.
    /// Populates AIRequest payload with data logged from ActionManager.
    /// Gets called when transcription is finished.
    /// </summary>
    /// <param name="transcript"></param>
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
        else if (transcript.Equals("[Error: FAILED TO TRANSCRIBE]"))
        {
            transcript = "Transcription failed!";
            HandleFallbackResponse(transcript);
            return;
        }
        else
        {
            Debug.Log($"AIConversationController: Creating AIRequest with transcript: '{transcript}'");

            if (_aiRequest != null)
                Destroy(_aiRequest);
            _aiRequest = gameObject.AddComponent<AIRequest>();
            _aiRequest.Query = transcript;
            _aiRequest.MaxTokens = this.maxTokens;
            _aiRequest.RequestPayload = _actionManager.GetUploadData();
        }
    }

    /// <summary>
    /// NPC response if transcript fails.
    /// </summary>
    /// <param name="fallbackText"></param>
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


    /// <summary>
    /// Adds a message to the local context and global chatlog.
    /// </summary>
    /// <param name="message"></param>
    public void AddMessage(Message message)
    {
        if (_actionManager != null)
        {
            _actionManager.AddChatMessage(message); // Add message to global chatlog
        }
        else
        {
            Debug.LogWarning("AIConversationController: actionManager is null, message will only be added to local context", this);
        }
        messages.Add(message);
        ShortenList(messages, 20);
        Debug.Log($"AIContext: Added '{message.role}' message. Total messages: {messages.Count}");
    }

    /// <summary>
    /// Shortens the list to a specified limit.
    /// Identical logic to the one in ActionManager.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="limit"></param>
    private void ShortenList<T>(List<T> list, int limit)
    {
        if (list.Count >= limit)
        {
            list.RemoveRange(0, list.Count - limit);
        }
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

    /// <summary>
    /// Populates the global memory with chat logs.
    /// Called when NPC is spawned and has global memory toggled on.
    /// </summary>
    public void PopulateGlobalMemory()
    {
        messages = _actionManager.GetGlobalChatLogs();
    }
}