using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Meta.WitAi.TTS.Utilities; // Required for TTSSpeaker type
using UnityEngine.Events; // Required for UnityEvent
using UnityEngine.UI; // Required for Button, Image

// Keep existing event definition
[System.Serializable]
public class DialogueChanged : UnityEvent<string, string, int, int> { }

public class DialogueBoxController : MonoBehaviour
{
    // --- Existing UI References (Ensure assigned in Inspector) ---
    [SerializeField] public TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject _dialogueBox; // Parent object for dialogue background/text
    [SerializeField] private GameObject _answerBox; // Parent object for answer buttons
    [SerializeField] private GameObject _dialogueCanvas; // The root canvas
    [SerializeField] private GameObject _skipLineButton;
    [SerializeField] private GameObject _exitButton; // Used in original non-AI flow?
    [SerializeField] private GameObject _speakButton; // Spawned by ButtonSpawner in new flow

    // --- New/Modified AI UI References (Ensure assigned in Inspector/Prefab) ---
    [SerializeField] public SpriteRenderer holdBToTalkMessage; // Assign the SpriteRenderer for "Hold B to Talk" message
    [SerializeField] public GameObject _restartConversationButton; // Assign the "Restart" button GameObject

    // --- Component References ---
    [SerializeField] public GameObject TTSSpeaker; // Reference to GameObject containing Wit TTSSpeaker (Set by NPCSpawner)
    [HideInInspector] public ButtonSpawner buttonSpawner; // Get in Awake (Uses NEW ButtonSpawner)
    [HideInInspector] public AIConversationController _AIConversationController; // Set by NPCSpawner for AI NPCs
    [HideInInspector] public AIResponseToSpeech _AIResponseToSpeech; // Set by NPCSpawner for AI NPCs

    // --- Events ---
    [HideInInspector] public static event Action<string> OnDialogueStarted;
    [HideInInspector] public static event Action<string> OnDialogueEnded;
    public DialogueChanged m_DialogueChanged; // Keep existing UnityEvent

    // --- State Variables ---
    [HideInInspector] private bool _skipLineTriggered;
    [HideInInspector] private bool _answerTriggered;
    [HideInInspector] private int _answerIndex;
    [HideInInspector] public bool dialogueIsActive;
    [HideInInspector] public DialogueTree dialogueTreeRestart; // Keep track of the current tree for restart/speak button
    [HideInInspector] public bool dialogueEnded;
    [HideInInspector] public int timesEnded = 0; // Track how many times dialogue ended
    [HideInInspector] private int _activatedCount = 0; // Track answer activations?

    // --- AI State Variables ---
    [HideInInspector] public bool useWitAI = true; // Default to WitAI (set by NPCSpawner based on SO)
    [HideInInspector] public bool isTalkable = false; // Can the player interrupt with voice?
    private Coroutine _thinkingCoroutine; // Reference to the "..." coroutine

    // --- Animation Hashes ---
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _isTalkingHash;
    [HideInInspector] private int _hasNewDialogueOptionsHash;
    [HideInInspector] private int _isPointingHash; // Keep hash, but pointing logic disabled for now
    [HideInInspector] private int _isListeningHash; // New hash for listening animation

    // --- UI Layout ---
    private Vector2 _oldDialogueCanvasSizeDelta; // Keep track of original canvas size for speak button state

    // --- Pointing ---
    private bool _isThinking = false; // Flag to track thinking state


    private void Awake()
    {
        buttonSpawner = GetComponent<ButtonSpawner>(); // Get reference to the NEW ButtonSpawner
        if (buttonSpawner == null)
        {
            Debug.LogError("DialogueBoxController: ButtonSpawner component not found!", this);
        }

        // Initialize state
        dialogueIsActive = false;
        dialogueEnded = true; // Start as ended
        isTalkable = false;

        // Get Animator reference
        updateAnimator(); // Find animator in children

        // Initialize event if null
        if (m_DialogueChanged == null)
        {
            m_DialogueChanged = new DialogueChanged();
        }

        // Ensure required UI elements are assigned
        if (_dialogueText == null) Debug.LogError("DialogueBoxController: _dialogueText not assigned!", this);
        if (_dialogueBox == null) Debug.LogError("DialogueBoxController: _dialogueBox not assigned!", this);
        if (_dialogueCanvas == null) Debug.LogError("DialogueBoxController: _dialogueCanvas not assigned!", this);
        if (_skipLineButton == null) Debug.LogError("DialogueBoxController: _skipLineButton not assigned!", this);
        if (_exitButton == null) Debug.LogError("DialogueBoxController: _exitButton not assigned!", this); // Assuming still used by non-AI?
        if (_speakButton == null) Debug.LogError("DialogueBoxController: _speakButton not assigned!", this);
        if (holdBToTalkMessage == null) Debug.LogWarning("DialogueBoxController: holdBToTalkMessage not assigned. AI interrupt message won't show.", this);
        if (_restartConversationButton == null) Debug.LogWarning("DialogueBoxController: _restartConversationButton not assigned. AI restart function won't show.", this);


        ResetBox(); // Initial UI state
    }

    private void Start()
    {
        // _pointingScript = GetComponent<PointingScript>(); // Get new pointing script
        // Pointing disabled for now

        // Assign the event camera for UI interaction (Raycasting)
        if (_dialogueCanvas != null)
        {
            Canvas canvas = _dialogueCanvas.GetComponent<Canvas>();
            if (canvas != null && canvas.worldCamera == null) // Only assign if not already set
            {
                GameObject cameraCaster = GameObject.Find("CameraCaster"); // Find scene object
                if (cameraCaster != null)
                {
                    Camera eventCamera = cameraCaster.GetComponent<Camera>();
                    if (eventCamera != null)
                    {
                        canvas.worldCamera = eventCamera;
                        Debug.Log($"DialogueBoxController: Assigned '{eventCamera.name}' as world camera for '{_dialogueCanvas.name}'.", this);
                    }
                    else { Debug.LogError("DialogueBoxController: CameraCaster GameObject does not have a Camera component!", cameraCaster); }
                }
                else { Debug.LogWarning("DialogueBoxController: CameraCaster GameObject not found in the scene. UI might not be interactable.", this); }
            }
            else if (canvas == null) { Debug.LogError("DialogueBoxController: _dialogueCanvas does not have a Canvas component!", _dialogueCanvas); }
        }
        else { Debug.LogError("DialogueBoxController: _dialogueCanvas is not assigned!", this); }

        // Get the original canvas size for restoring after 'Speak' button state
        if (_dialogueCanvas != null)
        {
            RectTransform canvasRect = _dialogueCanvas.GetComponent<RectTransform>();
            if (canvasRect != null)
            {
                _oldDialogueCanvasSizeDelta = canvasRect.sizeDelta;
            }
            else
            {
                Debug.LogError("DialogueBoxController: _dialogueCanvas is missing RectTransform!", _dialogueCanvas);
            }
        }

        // Add listener for the restart button if it exists
        if (_restartConversationButton != null)
        {
            Button restartBtnComp = _restartConversationButton.GetComponent<Button>();
            if (restartBtnComp != null)
            {
                restartBtnComp.onClick.AddListener(RestartConversation);
            }
            else
            {
                Debug.LogError("DialogueBoxController: _restartConversationButton is missing Button component!", _restartConversationButton);
            }
        }

    }

    public void updateAnimator()
    {
        this._animator = GetComponentInChildren<Animator>(); // Find animator on self or children
        if (this._animator != null)
        {
            _isTalkingHash = Animator.StringToHash("isTalking");
            _hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
            _isPointingHash = Animator.StringToHash("isPointing");
            _isListeningHash = Animator.StringToHash("isListening"); // Add listening hash
        }
        else
        {
            // Changed from error to warning since the animator might be attached later
            Debug.LogWarning("DialogueBoxController: Animator not found in children! Will try again later when model is loaded.", this);
        }
    }

    // Called by SetCharacterModel when animator changes
    public void updateAnimator(Animator animator)
    {
        this._animator = animator;
        if (this._animator != null)
        {
            _isTalkingHash = Animator.StringToHash("isTalking");
            _hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
            _isPointingHash = Animator.StringToHash("isPointing");
            _isListeningHash = Animator.StringToHash("isListening"); // Add listening hash
        }
        else
        {
            Debug.LogWarning("DialogueBoxController: updateAnimator called with null Animator!", this);
        }
    }

    // --- Dialogue Flow Control ---

    // Start standard dialogue or initial AI dialogue section
    public void StartDialogue(DialogueTree dialogueTree, int startSection, string name, int element = 0) // Add element param from new sig
    {
        if (dialogueTree == null) { Debug.LogError("StartDialogue called with null DialogueTree!", this); return; }
        if (_animator == null) { Debug.LogError("StartDialogue called but Animator is null!", this); return; }

        dialogueIsActive = true;
        dialogueEnded = false;
        isTalkable = false; // Reset talkable state
        timesEnded = 0; // Reset end counter

        _animator.SetBool(_hasNewDialogueOptionsHash, false); // Stop "new dialogue" animation

        ResetBox(); // Clear previous buttons/state
        _dialogueBox.SetActive(true); // Show dialogue UI container
        if (_dialogueCanvas != null) _dialogueCanvas.SetActive(true); // Ensure canvas is active

        // Restore canvas size if it was changed for speak button
        RestoreCanvasSize();

        OnDialogueStarted?.Invoke(name); // Notify listeners
        _activatedCount = 0; // Reset answer count

        dialogueTreeRestart = dialogueTree; // Store for potential restart

        // Start the dialogue coroutine
        StartCoroutine(RunDialogue(dialogueTree, startSection, element)); // Pass element

        //_exitButton.SetActive(true); // Control exit button visibility within RunDialogue or based on context
    }

    IEnumerator RunDialogue(DialogueTree dialogueTree, int section, int element) // Added element parameter
    {
        if (section < 0 || section >= dialogueTree.sections.Length)
        {
            Debug.LogError($"RunDialogue: Invalid section index {section} for DialogueTree '{dialogueTree.name}'", this);
            ExitConversation(); yield break;
        }

        DialogueSection currentSection = dialogueTree.sections[section];

        // --- Process Dialogue Lines ---
        // Handle element parameter to potentially skip lines (from new repo version)
        int startLine = (element >= 0 && element < currentSection.dialogue.Length) ? element : 0;

        for (int i = startLine; i < currentSection.dialogue.Length; i++)
        {
            // Reset pointing state (if pointing was enabled)
            // if (_pointingScript != null) _pointingScript.ResetDirection(_animator.transform);
            if (_animator != null) _animator.SetBool(_isPointingHash, false);

            // Set text, handling potential length issues for display
            string lineText = currentSection.dialogue[i];
            _dialogueText.text = TruncateForDisplay(lineText); // Use helper for consistent truncation

            // Determine if this line is interruptable (Check array bounds)
            isTalkable = (_AIConversationController != null && // Only AI NPCs can be talkable
                          currentSection.interruptableElements != null &&
                          i < currentSection.interruptableElements.Length &&
                          currentSection.interruptableElements[i]);

            // Show/Hide "Hold B to Talk" message
            if (holdBToTalkMessage != null) holdBToTalkMessage.enabled = isTalkable;

            // Trigger standard talking animation
            if (_animator != null)
            {
                _animator.SetBool(_isTalkingHash, true);
                // Start coroutine to automatically stop talking animation after a duration
                StartCoroutine(RevertToIdleAnimation(9.0f)); // Use helper with duration
            }


            // --- TTS ---
            SpeakLine(lineText); // Use helper to handle TTS provider logic
            yield return new WaitUntil(() => _AIResponseToSpeech == null || _AIResponseToSpeech.readyToAnswer); // Wait for TTS to start playing


            // --- UI Buttons ---
            _skipLineButton.SetActive(true);
            Button skipButtonComponent = _skipLineButton.GetComponent<Button>();
            Image skipButtonImage = _skipLineButton.transform.GetChild(0).GetComponent<Image>(); // Assuming image is first child
            if (skipButtonComponent != null)
            {
                skipButtonComponent.interactable = !currentSection.disabkeSkipLineButton; // Disable if flag is set
                                                                                          // Update image color based on interactable state
                if (skipButtonImage != null)
                {
                    skipButtonImage.color = skipButtonComponent.interactable ? skipButtonComponent.colors.normalColor : skipButtonComponent.colors.disabledColor;
                }
            }

            // Deactivate exit button during line display? Or keep active?
            _exitButton.SetActive(false); // Deactivate standard exit button during lines/questions
            _restartConversationButton.SetActive(false); // Deactivate restart button


            // --- Pointing (Logic kept from new repo, but disabled for now) ---
            // string pointTargetName = currentSection.pointAt;
            // if (!string.IsNullOrEmpty(pointTargetName))
            // {
            //     if (_pointingScript != null) {
            //          bool directionChanged = _pointingScript.ChangeDirection(pointTargetName, _animator.transform);
            //          if (directionChanged && _animator != null)
            //          {
            //              _animator.SetBool(_isTalkingHash, false); // Stop talking anim if pointing
            //              _animator.SetBool(_isPointingHash, true);
            //          }
            //     } else { Debug.LogWarning("Pointing target specified but PointingScript is missing.", this); }
            // }

            // --- Wait for Player Input ---
            // Invoke the dialogue changed event (From new repo version)
            m_DialogueChanged?.Invoke(transform.name, dialogueTree.name, section, i);

            _skipLineTriggered = false; // Reset trigger
            while (!_skipLineTriggered)
            {
                // Allow exit/restart? For now, only allow skipping.
                yield return null; // Wait for SkipLine() to be called
            }
            // Stop the talking animation manually if skipped early? Or let RevertToIdle handle it.
            if (_animator != null) _animator.SetBool(_isTalkingHash, false); // Stop talking immediately on skip
            StopCoroutine("RevertToIdleAnimation"); // Stop the automatic revert timer

        } // End of dialogue lines loop

        // --- Post-Dialogue Actions ---
        isTalkable = false; // Cannot interrupt after lines are done, before question/end
        if (holdBToTalkMessage != null) holdBToTalkMessage.enabled = false;
        _skipLineButton.SetActive(false); // Hide skip button

        // Handle NPC movement after dialogue lines (From new repo version)
        string walkTurnDestination = currentSection.walkOrTurnTowardsAfterDialogue;
        if (!string.IsNullOrEmpty(walkTurnDestination))
        {
            WalkingNpc walker = GetComponent<WalkingNpc>(); // Check if WalkingNpc exists
            if (walker != null)
            {
                walker.WalkPath(walkTurnDestination);
            }
            else { Debug.LogWarning($"NPC {name} should walk/turn towards '{walkTurnDestination}' but WalkingNpc component is missing.", this); }
        }


        // --- Check if Dialogue Ends Here ---
        if (currentSection.endAfterDialogue)
        {
            dialogueEnded = true;
            timesEnded++;
            OnDialogueEnded?.Invoke(name); // Use the name passed to StartDialogue

            // Check AI-specific end behaviour
            if (_AIConversationController != null && currentSection.dialogueEnd == DialogueEnd.EndWithRestartButton)
            {
                StartDynamicQuery(dialogueTree); // Show restart button and generic message
            }
            else
            {
                ExitConversation(); // Default end: Show speak button or just close
            }
            yield break; // End the coroutine here
        }


        // --- Process Branch Point (Question & Answers) ---
        if (currentSection.branchPoint.answers == null || currentSection.branchPoint.answers.Length == 0)
        {
            Debug.LogWarning($"Dialogue section {section} in '{dialogueTree.name}' has no 'EndAfterDialogue' and no answers. Ending conversation.", this);
            ExitConversation();
            yield break;
        }

        // Display question
        string questionText = currentSection.branchPoint.question;
        _dialogueText.text = TruncateForDisplay(questionText);

        // Speak question
        SpeakLine(questionText);
        yield return new WaitUntil(() => _AIResponseToSpeech == null || _AIResponseToSpeech.readyToAnswer); // Wait for TTS to start playing

        // Invoke the dialogue changed event for the question (-1 indicates question) (From new repo version)
        m_DialogueChanged?.Invoke(transform.name, dialogueTree.name, section, -1);

        // Show answer buttons (Using NEW ButtonSpawner)
        ShowAnswers(currentSection.branchPoint);

        // Wait for player to select an answer
        _answerTriggered = false; // Reset trigger
        while (!_answerTriggered)
        {
            // Allow exit/restart? For now, only allow answering.
            yield return null; // Wait for AnswerQuestion() to be called
        }

        // --- Process Selected Answer ---
        Answer selectedAnswer = currentSection.branchPoint.answers[_answerIndex];

        // Handle NPC movement after answer (From new repo version)
        string walkTurnAfterAnswer = selectedAnswer.walkOrTurnTowardsAfterAnswer;
        if (!string.IsNullOrEmpty(walkTurnAfterAnswer))
        {
            WalkingNpc walker = GetComponent<WalkingNpc>();
            if (walker != null)
            {
                walker.WalkPath(walkTurnAfterAnswer);
            }
            else { Debug.LogWarning($"NPC {name} should walk/turn towards '{walkTurnAfterAnswer}' after answer but WalkingNpc component is missing.", this); }
        }

        // Check if conversation ends after this answer
        if (selectedAnswer.endAfterAnswer)
        {
            dialogueEnded = true;
            timesEnded++;
            OnDialogueEnded?.Invoke(name);

            // Check AI-specific end behaviour again? Or always default end here?
            // Let's assume default end (Speak button or close) after an answer choice leads to end.
            ExitConversation();
        }
        else
        {
            // Continue to the next dialogue section specified by the answer
            StartCoroutine(RunDialogue(dialogueTree, selectedAnswer.nextElement, 0)); // Start next section from element 0
        }
    }


    // Handles speaking a line using the appropriate TTS system
    private void SpeakLine(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        // Stop any previous speech first
        if (_AIResponseToSpeech != null) _AIResponseToSpeech.StopPlayback();
        else if (TTSSpeaker != null)
        {
            var witSpeaker = TTSSpeaker.GetComponentInChildren<TTSSpeaker>();
            if (witSpeaker != null && witSpeaker.IsSpeaking) witSpeaker.Stop();
        }


        if (_AIConversationController != null && _AIResponseToSpeech != null) // If AI NPC
        {
            // Add dialogue to AI context *before* speaking it
            AddDialogueToContext(text);

            if (useWitAI)
            {
                Debug.Log("DialogueBoxController: Speaking line via WitAI TTS.");
                StartCoroutine(_AIResponseToSpeech.WitAIDictate(text));
            }
            else
            {
                Debug.Log("DialogueBoxController: Speaking line via OpenAI TTS.");
                StartCoroutine(_AIResponseToSpeech.OpenAIDictate(text));
            }
        }
        else if (TTSSpeaker != null) // If standard NPC (or AI fallback?)
        {
            Debug.Log("DialogueBoxController: Speaking line via standard Wit TTSSpeaker.");
            var witSpeaker = TTSSpeaker.GetComponentInChildren<TTSSpeaker>();
            if (witSpeaker != null)
            {
                witSpeaker.Speak(text);
                // For standard Wit, set readyToAnswer immediately for flow control
                if (_AIResponseToSpeech != null)
                {
                    // Hacky: If AIResponseToSpeech exists but we're using standard TTS, manually set flag
                    // StartCoroutine(SetReadyAfterDelay(0.1f));
                }
            }
            else { Debug.LogError("DialogueBoxController: TTSSpeaker GameObject assigned, but TTSSpeaker component not found in children!", TTSSpeaker); }
        }
        else
        {
            Debug.LogError("DialogueBoxController: Cannot speak line - No AIResponseToSpeech and no TTSSpeaker available!", this);
        }
    }

    // Helper coroutine for the standard TTS case if waiting is needed
    // IEnumerator SetReadyAfterDelay(float delay) {
    //     yield return new WaitForSeconds(delay);
    //     if(_AIResponseToSpeech != null) _AIResponseToSpeech.SetReadyState(true); // Need method in AIResponseToSpeech
    // }


    // --- AI Specific Dialogue Flows ---

    // Called when AI dialogue ends with 'EndWithRestartButton' option
    public void StartDynamicQuery(DialogueTree dialogueTree)
    {
        Debug.Log("Dialogue ended. Showing restart button and generic message.", this);
        // Stop previous NPC speech if any is still playing
        if (_AIResponseToSpeech != null) _AIResponseToSpeech.StopPlayback();
        else if (TTSSpeaker != null)
        {
            var witSpeaker = TTSSpeaker.GetComponentInChildren<TTSSpeaker>();
            if (witSpeaker != null && witSpeaker.IsSpeaking) witSpeaker.Stop();
        }

        ResetBox(); // Clear answer buttons etc.
        _dialogueBox.SetActive(true); // Ensure box is visible
        if (_dialogueCanvas != null) _dialogueCanvas.SetActive(true);
        RestoreCanvasSize();

        // Show restart button instead of speak button
        if (_restartConversationButton != null) _restartConversationButton.SetActive(true);

        // Set and speak generic closing message
        string genericEndMessage = "Is there anything else I can help you with?"; // Or load from config/tree?
        _dialogueText.text = genericEndMessage;
        SpeakLine(genericEndMessage); // Use the standard speak helper

        // Enable interaction (Talkable = true) after the generic message is spoken?
        StartCoroutine(EnableTalkableAfterDelay(2.0f)); // Example: Allow talking after 2 seconds
    }

    IEnumerator EnableTalkableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isTalkable = true; // Allow player to speak again
        if (holdBToTalkMessage != null) holdBToTalkMessage.enabled = true;
        Debug.Log("DialogueBoxController: Re-enabled talkable state after dynamic query end.");
    }

    // Displays AI response text in the box
    public IEnumerator DisplayResponse(string response)
    {
        Debug.Log($"DisplayResponse called with response: '{response}'");

        // Stop thinking mode first
        _isThinking = false;

        // Stop coroutine if it exists
        if (_thinkingCoroutine != null)
        {
            StopCoroutine(_thinkingCoroutine);
            _thinkingCoroutine = null;
        }

        // Ensure the dialogue UI is active
        _dialogueBox.SetActive(true);
        if (_dialogueCanvas != null) _dialogueCanvas.SetActive(true);

        // Clear existing text first
        _dialogueText.text = "";

        // Wait a frame to ensure the text field is updated
        yield return null;

        try
        {
            // Log UI visibility
            Debug.Log($"_dialogueBox active: {_dialogueBox.activeSelf}, _dialogueCanvas active: {_dialogueCanvas.activeSelf}");

            // Log and set the response text
            Debug.Log($"Setting _dialogueText to: '{response}'");
            string truncatedText = TruncateForDisplay(response);
            Debug.Log($"Truncated text: '{truncatedText}'");
            _dialogueText.text = truncatedText;

            // Force UI refresh
            _dialogueBox.SetActive(false);
            _dialogueBox.SetActive(true);

            // Verify text after a delay
            StartCoroutine(VerifyTextAfterDelay());

            // Set talking animation if possible
            if (_animator != null && _animator.isActiveAndEnabled)
            {
                try { _animator.SetBool(_isTalkingHash, true); }
                catch { /* Ignore animation errors */ }
            }

            // Show restart button
            if (_restartConversationButton != null)
                _restartConversationButton.SetActive(true);

            // Enable talkable state
            isTalkable = true;
            if (holdBToTalkMessage != null)
                holdBToTalkMessage.enabled = true;

            // Revert to idle animation after delay (without storing reference)
            StartCoroutine(SafeRevertToIdleAnimation(9.0f));
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in DisplayResponse: {e.Message}");
        }

        yield return null;
    }

    private IEnumerator VerifyTextAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log($"_dialogueText after delay: '{_dialogueText.text}'");
    }


    private IEnumerator SafeRevertToIdleAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_animator != null && _animator.isActiveAndEnabled)
        {
            try
            {
                _animator.SetBool(_isTalkingHash, false);
                _animator.SetBool(_isListeningHash, false);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Error setting animator parameter in SafeRevertToIdleAnimation: {e.Message}");
            }
        }
    }

    // Displays "..." thinking indicator
    public IEnumerator DisplayThinking()
    {
        Debug.Log("DialogueBoxController: Starting thinking animation");
        _isThinking = true;
        int dots = 0;

        // While the thinking flag is set
        while (_isThinking)
        {
            // Only update text if we're still in thinking mode
            // This prevents overwriting a response text that might have been set
            if (_isThinking)
            {
                dots = (dots % 3) + 1; // Cycle between 1-3 dots
                string dotsText = new string('.', dots);
                _dialogueText.text = dotsText;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                // Exit if thinking mode was turned off
                yield break;
            }
        }
    }


    // Called by AIRequest or AIConversationController to start listening animation
    public void startThinking() // Renamed from deprecated version for clarity
    {
        Debug.Log("DialogueBoxController: Starting thinking mode");

        // Clear any existing thinking coroutine
        if (_thinkingCoroutine != null)
        {
            StopCoroutine(_thinkingCoroutine);
            _thinkingCoroutine = null;
        }

        // Try to set the animation parameter if possible
        try
        {
            if (_animator != null && _animator.isActiveAndEnabled)
            {
                _animator.SetBool(_isListeningHash, true);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to set animator parameter: {e.Message}");
        }

        // Set thinking flag and start the animation
        _isThinking = true;
        _thinkingCoroutine = StartCoroutine(DisplayThinking());
    }

    // Called by AIRequest to stop listening animation and thinking indicator
    public void stopThinking()
    {
        Debug.Log("DialogueBoxController: Stopping thinking mode");

        // Set flag first to ensure DisplayThinking exits
        _isThinking = false;

        // Try to set animation parameter if possible
        try
        {
            if (_animator != null && _animator.isActiveAndEnabled)
            {
                _animator.SetBool(_isListeningHash, false);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to set animator parameter: {e.Message}");
        }

        // Stop coroutine if it exists
        if (_thinkingCoroutine != null)
        {
            StopCoroutine(_thinkingCoroutine);
            _thinkingCoroutine = null;
        }

        // Clear the text to ensure it doesn't show dots
        _dialogueText.text = "";
    }


    // --- Button Actions ---

    public void SkipLine()
    {
        _skipLineTriggered = true;
    }

    public void AnswerQuestion(int answer)
    {
        if (dialogueIsActive && !_answerTriggered) // Prevent double clicks
        {
            _answerIndex = answer;
            _answerTriggered = true;
            _activatedCount++;
            // remove the buttons immediately (using NEW ButtonSpawner)
            if (buttonSpawner != null) buttonSpawner.removeAllButtons();
            else { Debug.LogError("ButtonSpawner reference is null in AnswerQuestion!", this); }

            // Stop TTS if it was the question being spoken
            StopSpeech();
        }
    }

    // Restarts the current dialogue tree from the beginning
    public void RestartConversation()
    {
        Debug.Log("Restarting conversation.", this);
        StopSpeech(); // Stop any ongoing speech
        if (dialogueTreeRestart != null)
        {
            StartDialogue(dialogueTreeRestart, 0, name); // Restart from section 0, element 0
        }
        else
        {
            Debug.LogWarning("Cannot restart conversation, dialogueTreeRestart is null.", this);
        }
    }

    // Called when player leaves trigger area or dialogue ends naturally
    public void ExitConversation()
    {
        Debug.Log("Exiting conversation.", this);
        StopSpeech(); // Stop any ongoing speech

        // Stop animations
        if (_animator != null)
        {
            _animator.SetBool(_isTalkingHash, false);
            _animator.SetBool(_isPointingHash, false);
            _animator.SetBool(_isListeningHash, false);
        }
        stopThinking(); // Ensure thinking indicator is stopped


        // Reset state
        dialogueIsActive = false;
        isTalkable = false;
        ResetBox(); // Clean up UI

        // Reset pointing direction if pointing was enabled
        // if (_pointingScript != null) _pointingScript.ResetDirection(_animator.transform);

        // Show speak button if configured in the DialogueTree
        if (dialogueTreeRestart != null && dialogueTreeRestart.speakButtonOnExit)
        {
            StartSpeakCanvas(dialogueTreeRestart);
        }
        else
        {
            // If no speak button, ensure the entire canvas is hidden? Or just the dialogue box part?
            HideDialogueBox(); // Hide the UI if not showing speak button
        }
    }

    public void ResetAfterAIInteraction()
    {
        // Keep dialogue active but allow normal progression
        dialogueIsActive = true;
        isTalkable = false; // Reset temporarily

        // If the dialogue tree has a next step, enable skip button
        _skipLineButton.SetActive(true);

        // Alternatively, if this is the end of a dialogue section, show answers or restart
        if (dialogueEnded)
        {
            if (_restartConversationButton != null) _restartConversationButton.SetActive(true);
        }
    }


    // --- UI Management ---

    // Prepares the UI for the initial "Speak" button state
    public void StartSpeakCanvas(DialogueTree dialogueTree)
    {
        ResetBox(); // Clear everything first
        _dialogueBox.SetActive(true); // Need the box active to hold the button
        if (_dialogueCanvas != null) _dialogueCanvas.SetActive(true);
        _dialogueText.text = ""; // No text for speak button state

        // Resize canvas/box to be smaller, just for the speak button
        if (_dialogueCanvas != null)
        {
            RectTransform canvasRect = _dialogueCanvas.GetComponent<RectTransform>();
            if (canvasRect != null)
            {
                canvasRect.sizeDelta = new Vector2(50, 30); // Small size for button
            }
        }

        // Use the NEW Button Spawner to create the speak button
        if (buttonSpawner != null)
        {
            buttonSpawner.spawnSpeakButton(dialogueTree); // Assumes new ButtonSpawner has this method
        }
        else { Debug.LogError("Cannot spawn Speak button, ButtonSpawner is missing!", this); }

    }

    // Resets the dialogue box UI to its default hidden state
    public void ResetBox()
    {
        StopAllCoroutines(); // Stop thinking, revertidle, etc.
        _thinkingCoroutine = null; // Clear coroutine ref

        if (_dialogueBox != null) _dialogueBox.SetActive(false);
        if (_skipLineButton != null) _skipLineButton.SetActive(false);
        if (_exitButton != null) _exitButton.SetActive(false); // Hide standard exit
        if (_restartConversationButton != null) _restartConversationButton.SetActive(false); // Hide restart button
        if (holdBToTalkMessage != null) holdBToTalkMessage.enabled = false; // Hide AI message

        // Use the NEW ButtonSpawner to remove buttons
        if (buttonSpawner != null) buttonSpawner.removeAllButtons();

        // Reset internal state flags
        _skipLineTriggered = false;
        _answerTriggered = false;
        isTalkable = false; // Reset talkable state

        // Restore original canvas size if needed (might interfere if called before RunDialogue)
        // RestoreCanvasSize(); // Call this at the START of RunDialogue instead
    }

    // Displays the answer buttons using the NEW ButtonSpawner
    void ShowAnswers(BranchPoint branchPoint)
    {
        if (buttonSpawner == null) { Debug.LogError("Cannot show answers, ButtonSpawner is missing!", this); return; }
        buttonSpawner.removeAllButtons(); // Clear any previous buttons (like speak button)
        buttonSpawner.spawnAnswerButtons(branchPoint.answers); // Spawn new answer buttons

        // Reset pointing animation if it was active
        if (_animator != null) _animator.SetBool(_isPointingHash, false);
        // if (_pointingScript != null ) _pointingScript.ResetDirection(_animator.transform);
    }


    // --- Helper Methods ---

    // Stops any ongoing TTS
    private void StopSpeech()
    {
        if (_AIResponseToSpeech != null)
        {
            _AIResponseToSpeech.StopPlayback();
        }
        else if (TTSSpeaker != null)
        {
            var witSpeaker = TTSSpeaker.GetComponentInChildren<TTSSpeaker>();
            if (witSpeaker != null && witSpeaker.IsSpeaking)
            {
                witSpeaker.Stop();
            }
        }
    }


    // Reverts talking/listening animation to idle after a delay
    private IEnumerator RevertToIdleAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_animator != null)
        {
            _animator.SetBool(_isTalkingHash, false);
            _animator.SetBool(_isListeningHash, false);
        }
    }

    // Gets the current answer count
    public int GetActivatedCount()
    {
        return _activatedCount;
    }

    // Truncates text for display if it's too long
    private string TruncateForDisplay(string text)
    {
        const int maxDisplayLength = 255; // Max characters for the dialogue box display
        if (text.Length > maxDisplayLength)
        {
            int lastSpace = text.LastIndexOf(' ', maxDisplayLength - 3);
            return (lastSpace > 0 ? text.Substring(0, lastSpace) : text.Substring(0, maxDisplayLength - 3)) + "...";
        }
        return text;
    }

    // Adds dialogue line to AI context if AI is enabled
    private void AddDialogueToContext(string dialogue)
    {
        if (_AIConversationController != null)
        {
            // Only add assistant messages (NPC speech) here. User messages added in AIRequest.
            _AIConversationController.AddMessage(new Message { role = "assistant", content = dialogue });
        }
    }

    // Restores the dialogue canvas size to its original dimensions
    private void RestoreCanvasSize()
    {
        if (_dialogueCanvas != null)
        {
            RectTransform canvasRect = _dialogueCanvas.GetComponent<RectTransform>();
            if (canvasRect != null && _oldDialogueCanvasSizeDelta != Vector2.zero)
            {
                canvasRect.sizeDelta = _oldDialogueCanvasSizeDelta;
            }
        }
    }


    // Public methods for NPCSpawner to set TTS mode
    public void useOpenAiTTS() { useWitAI = false; Debug.Log("DialogueBoxController: Set to use OpenAI TTS.", this); }
    public void useWitTTS() { useWitAI = true; Debug.Log("DialogueBoxController: Set to use WitAI TTS.", this); }

    // Public methods for ConversationController to control UI visibility based on proximity
    public void ShowDialogueBox()
    {
        if (_dialogueCanvas != null) _dialogueCanvas.SetActive(true);
        // Don't activate _dialogueBox itself here, let StartDialogue/StartSpeakCanvas handle it
        // if (_dialogueBox != null) _dialogueBox.SetActive(true);
        Debug.Log("Dialogue canvas shown due to proximity.", this);
    }
    public void HideDialogueBox()
    {
        // Only hide if a dialogue isn't actively running
        if (!dialogueIsActive)
        {
            if (_dialogueCanvas != null) _dialogueCanvas.SetActive(false);
            if (_dialogueBox != null) _dialogueBox.SetActive(false); // Hide the content box too
            Debug.Log("Dialogue canvas hidden due to proximity exit.", this);
        }
        else
        {
            Debug.Log("Player exited proximity but dialogue is active. Keeping canvas visible.", this);
        }
    }
}