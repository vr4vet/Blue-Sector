using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem; // Assuming use of Input System for VR input
using UnityEngine.UI;        // Required for Image
using TMPro;               // Required for TextMeshProUGUI
using Debug = UnityEngine.Debug; // Explicitly use UnityEngine.Debug

// Ensure Whisper and ServerTranscription namespaces are available
#if WHISPER_UNITY_PACKAGE_AVAILABLE
using Whisper;
using Whisper.Utils;
using VR4VET.Transcription; // <<< ADJUST THIS NAMESPACE if your ServerTranscriptionManager is elsewhere
#else
// Define dummy types if Whisper package is missing
namespace Whisper {
    public class WhisperManager : MonoBehaviour { public string language; public bool translateToEnglish; public async Task<WhisperResult> GetTextAsync(float[] d, int f, int c) => await Task.FromResult<WhisperResult>(null); }
    public class MicrophoneRecord : MonoBehaviour { public string SelectedMicDevice; public bool vadStop; public event Action<AudioChunk> OnRecordStop; public bool IsRecording; public void StartRecord() {} public void StopRecord() {} }
    public class AudioChunk { public float[] Data; public int Frequency; public int Channels; public float Length; }
    public class WhisperResult { public string Result; public string Language; }
}
namespace Whisper.Utils { public static class AudioUtils { } }
namespace VR4VET.Transcription {
    public class ServerTranscriptionManager : MonoBehaviour {
        public class ServerTranscriptionInfo { public bool IsServerProcessed; public float ProcessingTimeSeconds; public string Processor; public override string ToString() => ""; }
        public event Action<string, string, ServerTranscriptionInfo> OnTranscriptionComplete;
        public event Action<int> OnProgress;
        public event Action<string> OnTranscriptionError;
        public void ProcessAudioChunk(AudioChunk ac) {}
        public void SetLanguage(string lang) {}
    }
}
#endif

// *** REMOVED Placeholder AIConversationController definition ***
// The script now assumes AIConversationController is defined in its own file.


/// <summary>
/// Handles voice transcription using local Whisper or a remote server,
/// updating subtitles and interacting with an AI Controller.
/// Based on the original Transcribe.cs structure, but adapted for async/server processing.
/// </summary>
[RequireComponent(typeof(WhisperManager), typeof(MicrophoneRecord))]
public class Transcribe : MonoBehaviour
{
    [Header("Core Dependencies")]
#if WHISPER_UNITY_PACKAGE_AVAILABLE
    [Tooltip("WhisperManager component for local transcription.")]
    public WhisperManager whisper;
    [Tooltip("MicrophoneRecord component for audio capture.")]
    public MicrophoneRecord microphoneRecord;
    [Tooltip("Optional: Assign ServerTranscriptionManager for server-side transcription.")]
    public ServerTranscriptionManager serverTranscription;
#endif
    [Tooltip("Assign the TextMeshProUGUI for displaying subtitles.")]
    public TextMeshProUGUI subtitleText;
    [Tooltip("Assign the Image for the subtitle background.")]
    public Image subtitleBackground;

    [Header("Configuration")]
    [Tooltip("Enable to use server-side transcription instead of local Whisper.")]
    public bool useServerTranscription = false;
    [Tooltip("List of supported language codes for cycling (e.g., 'en', 'no', 'de', 'nl').")]
    public string[] availableLanguages = { "en", "no", "de", "nl" };
    [Tooltip("Assign the Input Action Reference for the record button (e.g., Trigger Press).")]
    public InputActionReference recordAction; // <<< ASSIGN YOUR VR INPUT ACTION HERE
    [Tooltip("Delay in seconds before automatically hiding the final subtitle.")]
    public float subtitleFadeDelay = 5.0f;
    [Tooltip("Initial state of subtitles visibility.")]
    public bool subtitlesInitiallyEnabled = true;
    [Tooltip("Enable Voice Activity Detection to automatically stop recording on silence. Usually FALSE for hold-to-record.")]
    public bool enableVAD = false;

    // --- Private State ---
    private AIConversationController _currentAIController; // Uses the externally defined class
    private Coroutine _subtitleFadeCoroutine;
    private Coroutine _processingIndicatorCoroutine;
    private bool _isSubtitlesEnabled = true;
    private bool _isInitialized = false;
    private bool _isRecording = false; // Internal recording state
    private string _currentLanguage = "";

    // --- Properties ---
    public string currentLanguage => _currentLanguage;
    public bool IsRecording => _isRecording;

    // --- Initialization ---

    void Awake()
    {
        // Component checks in Awake
#if WHISPER_UNITY_PACKAGE_AVAILABLE
        if (whisper == null) whisper = GetComponent<WhisperManager>();
        if (microphoneRecord == null) microphoneRecord = GetComponent<MicrophoneRecord>();
        // Server transcription is optional
        if (serverTranscription == null) serverTranscription = GetComponent<ServerTranscriptionManager>();

        if (whisper == null || microphoneRecord == null)
        {
            Debug.LogError("Transcribe: Missing WhisperManager or MicrophoneRecord component!", this);
            enabled = false; // Disable script
            return;
        }

        // Check for essential UI
        if (subtitleText == null) Debug.LogError("Transcribe: subtitleText not assigned!", this);
        if (subtitleBackground == null) Debug.LogError("Transcribe: subtitleBackground not assigned!", this);

        // Subscribe to the microphone record stop event -> This replaces the WhisperStream events
        microphoneRecord.OnRecordStop += HandleRecordStop;

        // Subscribe to server events if server manager exists
        if (serverTranscription != null)
        {
            serverTranscription.OnTranscriptionComplete += HandleServerTranscriptionComplete;
            serverTranscription.OnTranscriptionError += HandleServerTranscriptionError;
        }
#else
         Debug.LogError("Transcribe: Whisper package missing. Transcription disabled.", this);
         enabled = false;
#endif
    }

    void Start()
    {
#if WHISPER_UNITY_PACKAGE_AVAILABLE
        // -- Language Setup --
        // TODO: Replace this with logic to get language from settings menu??
        string initialLanguage = whisper.language;
        if (string.IsNullOrEmpty(initialLanguage) || initialLanguage.Equals("auto", StringComparison.OrdinalIgnoreCase))
        {
            if (availableLanguages.Length > 0) _currentLanguage = availableLanguages[0];
            else { Debug.LogError("Transcribe: No available languages defined!", this); _currentLanguage = "en"; }
            whisper.language = _currentLanguage;
        }
        else
        {
            // Check if language is in our list
            bool found = Array.Exists(availableLanguages, lang => lang.Equals(initialLanguage, StringComparison.OrdinalIgnoreCase));
            if (!found) Debug.LogWarning($"Transcribe: Language '{initialLanguage}' set in WhisperManager is not in the availableLanguages list. Using it anyway.", this);
            _currentLanguage = initialLanguage; // Use the one from WhisperManager
        }
        if (serverTranscription != null) serverTranscription.SetLanguage(_currentLanguage);
        Debug.Log($"Transcribe: Initial language set to: {_currentLanguage}");

        // -- Microphone Setup --
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("Transcribe: No microphone found!", this);
            UpdateSubtitleDisplay("Error: No Microphone Found", true, 5f);
            enabled = false;
            return;
        }
        microphoneRecord.SelectedMicDevice = Microphone.devices[0]; // Use default device
        Debug.Log($"Transcribe: Using Microphone: {microphoneRecord.SelectedMicDevice}");

        // -- VAD Setup --
        microphoneRecord.vadStop = enableVAD;

        // -- Subtitle Setup --
        _isSubtitlesEnabled = subtitlesInitiallyEnabled;
        UpdateSubtitleDisplay("", false); // Start hidden

        _isInitialized = true;
        Debug.Log("Transcribe: Initialization complete.");
#else
         // Already logged error in Awake
         UpdateSubtitleDisplay("Transcription Unavailable", true, 5f);
#endif
    }

    void Update()
    {
        if (!_isInitialized) return;

        HandleInput(); // Handle VR and Keyboard input
    }

    void OnEnable()
    {
        if (recordAction != null) recordAction.action.Enable();
    }

    void OnDisable()
    {
        if (recordAction != null) recordAction.action.Disable();
        if (_isRecording) ForceStopRecording(); // Stop if disabled while recording
    }

    void OnDestroy()
    {
#if WHISPER_UNITY_PACKAGE_AVAILABLE
        // Unsubscribe from events
        if (microphoneRecord != null) microphoneRecord.OnRecordStop -= HandleRecordStop;
        if (serverTranscription != null)
        {
            serverTranscription.OnTranscriptionComplete -= HandleServerTranscriptionComplete;
            serverTranscription.OnTranscriptionError -= HandleServerTranscriptionError;
        }
#endif
        // Stop coroutines
        if (_subtitleFadeCoroutine != null) StopCoroutine(_subtitleFadeCoroutine);
        if (_processingIndicatorCoroutine != null) StopCoroutine(_processingIndicatorCoroutine);
    }


    // --- Input Handling ---

    private void HandleInput()
    {
        // --- VR Input (Primary) ---
        // <<< Replace checks with your actual Input System logic >>>
        if (recordAction != null && recordAction.action.WasPressedThisFrame())
        {
            // This assumes the context (like interacting with an NPC)
            // already knows which AIController to use and calls StartRecording.
            // If the button press itself should initiate recording, you need
            // to determine the AIController context here.
            Debug.LogWarning("Transcribe: VR record button pressed. Call StartRecording(AIConversationController) to begin.");
            // Example placeholder to allow testing button press directly:
            // AIConversationController controller = FindObjectOfType<AIConversationController>();
            // if (controller != null) StartRecording(controller);
            // else Debug.LogError("Transcribe: Record button pressed but no AIController found!");
        }
        else if (recordAction != null && recordAction.action.WasReleasedThisFrame())
        {
            if (_isRecording)
            {
                EndRecording(); // Call the public EndRecording method
            }
        }

        // --- Keyboard Input (Secondary/Debug) ---
        if (Input.GetKeyDown(KeyCode.L)) CycleLanguage();
        if (Input.GetKeyDown(KeyCode.U)) ToggleSubtitles();
    }


    // --- Public Control Methods ---

    /// <summary>
    /// Starts the recording process. Requires the AI controller that should receive the final transcript.
    /// </summary>
    /// <param name="aiController">The AI controller to send the result to.</param>
    public void StartRecording(AIConversationController aiController)
    {
        if (!_isInitialized) { Debug.LogError("Transcribe: Not initialized.", this); return; }
        if (_isRecording) { Debug.LogWarning("Transcribe: Already recording.", this); return; }
        if (aiController == null) { Debug.LogError("Transcribe: AIController is null.", this); return; }

        _currentAIController = aiController;
        _isRecording = true;
        microphoneRecord.vadStop = enableVAD; // Ensure VAD setting is current

        try
        {
#if WHISPER_UNITY_PACKAGE_AVAILABLE
            microphoneRecord.StartRecord();
            Debug.Log($"Transcribe: Starting recording for {_currentAIController.gameObject.name}");
            if (_processingIndicatorCoroutine != null) StopCoroutine(_processingIndicatorCoroutine);
            _processingIndicatorCoroutine = StartCoroutine(DisplayTranscriptionProcessing());
#endif
        }
        catch (Exception e)
        {
            Debug.LogError($"Transcribe: Error starting recording: {e.Message}", this);
            UpdateSubtitleDisplay("Error: Mic Start Failed", true, 3.0f);
            _isRecording = false;
            _currentAIController = null;
        }
    }

    /// <summary>
    /// Stops the current recording session and triggers processing.
    /// </summary>
    public void EndRecording()
    {
        if (!_isInitialized || !_isRecording) return;

        Debug.Log("Transcribe: Stopping recording.");
        // Stop the "Listening..." indicator immediately
        if (_processingIndicatorCoroutine != null)
        {
            StopCoroutine(_processingIndicatorCoroutine);
            _processingIndicatorCoroutine = null;
        }
        UpdateSubtitleDisplay("Processing...", true); // Show processing message

        try
        {
#if WHISPER_UNITY_PACKAGE_AVAILABLE
            microphoneRecord.StopRecord(); // This will trigger HandleRecordStop for processing
#else
            // If package missing, just reset state
             _isRecording = false;
             _currentAIController = null;
            UpdateSubtitleDisplay("", false); // Clear processing message
#endif
        }
        catch (Exception e)
        {
            Debug.LogError($"Transcribe: Error stopping microphone: {e.Message}", this);
            UpdateSubtitleDisplay("Error: Mic Stop Failed", true, 3.0f);
            _isRecording = false; // Reset state as processing won't happen
            _currentAIController = null;
        }
        // _isRecording = false; // Set in HandleRecordStop *after* getting the callback
    }

    /// <summary>
    /// Forces recording to stop immediately without processing.
    /// </summary>
    private void ForceStopRecording()
    {
        Debug.LogWarning("Transcribe: Forcing recording stop.");
        if (_processingIndicatorCoroutine != null) StopCoroutine(_processingIndicatorCoroutine);
        _processingIndicatorCoroutine = null;
        UpdateSubtitleDisplay("", false); // Clear subtitles

#if WHISPER_UNITY_PACKAGE_AVAILABLE
        try { if (microphoneRecord != null && microphoneRecord.IsRecording) microphoneRecord.StopRecord(); } catch { }
#endif
        _isRecording = false;
        _currentAIController = null;
    }


    /// <summary>
    /// Sets the transcription language.
    /// </summary>
    /// <param name="languageCode">The language code (e.g., "en", "no").</param>
    public void SetLanguage(string languageCode)
    {
        if (!_isInitialized) return;

        string validLanguage = languageCode;
        bool found = Array.Exists(availableLanguages, lang => lang.Equals(languageCode, StringComparison.OrdinalIgnoreCase));

        if (!found)
        {
            Debug.LogWarning($"Transcribe: Language '{languageCode}' not in availableLanguages list. Using anyway.", this);
        }
        else
        {
            // Use the correctly cased version from our list if found
            validLanguage = Array.Find(availableLanguages, lang => lang.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
        }

        if (_currentLanguage == validLanguage) return; // No change

        _currentLanguage = validLanguage;
        Debug.Log("Transcribe: Language changed to: " + _currentLanguage);
        UpdateSubtitleDisplay($"Language: {_currentLanguage.ToUpper()}", true, 2.0f);

#if WHISPER_UNITY_PACKAGE_AVAILABLE
        if (whisper != null) whisper.language = _currentLanguage;
        if (serverTranscription != null) serverTranscription.SetLanguage(_currentLanguage);
#endif
    }

    /// <summary>
    /// Cycles through the available languages.
    /// </summary>
    public void CycleLanguage()
    {
        if (availableLanguages.Length == 0) return;
        int index = Array.IndexOf(availableLanguages, _currentLanguage);
        if (index < 0) index = 0; // If current lang not in list, start from first
        index = (index + 1) % availableLanguages.Length;
        SetLanguage(availableLanguages[index]);
    }

    /// <summary>
    /// Toggles the visibility of subtitles.
    /// </summary>
    public void ToggleSubtitles()
    {
        _isSubtitlesEnabled = !_isSubtitlesEnabled;
        UpdateSubtitleDisplay($"Subtitles {(_isSubtitlesEnabled ? "ON" : "OFF")}", true, 2.0f);
        if (!_isSubtitlesEnabled)
        {
            // Immediately hide if toggled off
            if (subtitleText != null) subtitleText.enabled = false;
            if (subtitleBackground != null) subtitleBackground.enabled = false;
            if (_subtitleFadeCoroutine != null) { StopCoroutine(_subtitleFadeCoroutine); _subtitleFadeCoroutine = null; }
        }
    }


    // --- Transcription Event Handlers ---

#if WHISPER_UNITY_PACKAGE_AVAILABLE
    /// <summary>
    /// Called by MicrophoneRecord when recording finishes and audio data is ready.
    /// This is the main entry point for processing the audio.
    /// </summary>
    private async void HandleRecordStop(AudioChunk recordedAudio)
    {
        // Check if we were actively recording with an assigned AI controller
        if (_currentAIController == null)
        {
            Debug.LogWarning("Transcribe: OnRecordStop called, but no AI Controller was assigned. Ignoring audio chunk.");
            _isRecording = false; // Ensure state is reset if VAD stopped early etc.
            UpdateSubtitleDisplay("", false); // Clear "Processing..."
            return;
        }

        _isRecording = false; // Mark recording as logically finished NOW.

        if (recordedAudio.Data == null || recordedAudio.Data.Length == 0)
        {
            Debug.LogWarning("Transcribe: No audio data recorded.", this);
            UpdateSubtitleDisplay("No audio detected", true, 2.0f);
            // Call CreateRequest with empty string for AI to handle no input? Or just return?
            // Let's call CreateRequest with an empty string for consistency, AI can handle it.
            ProcessTranscriptionResult("", null, false, null, true); // Treat as non-error, but empty result
            return;
        }

        Debug.Log($"Transcribe: Audio Chunk received. Length: {recordedAudio.Length:F2}s. Processing...");

        // --- Choose Processing Path ---
        if (useServerTranscription && serverTranscription != null)
        {
            UpdateSubtitleDisplay("Sending to server...", true);
            serverTranscription.ProcessAudioChunk(recordedAudio); // Server manager will call back
        }
        else
        {
            if (whisper == null)
            { // Safety check
                ProcessTranscriptionResult(null, null, true, "WhisperManager not available for local processing.");
                return;
            }
            UpdateSubtitleDisplay("Processing locally...", true);
            try
            {
                var result = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
                ProcessTranscriptionResult(result?.Result, result?.Language); // Handle null result
            }
            catch (Exception e)
            {
                Debug.LogError($"Transcribe: Local transcription error: {e.Message}", this);
                ProcessTranscriptionResult(null, null, true, $"Local transcription error: {e.Message}");
            }
        }
    }

    /// <summary>
    /// Called by ServerTranscriptionManager when server processing is complete.
    /// </summary>
    private void HandleServerTranscriptionComplete(string text, string language, ServerTranscriptionManager.ServerTranscriptionInfo info)
    {
        Debug.Log($"Transcribe: Server result received. Language: {language ?? "Unknown"}. Info: {info}");
        ProcessTranscriptionResult(text, language, false, null); // Pass result to central processor
    }

    /// <summary>
    /// Called by ServerTranscriptionManager on error.
    /// </summary>
    private void HandleServerTranscriptionError(string errorMessage)
    {
        Debug.LogError($"Transcribe: Server transcription error: {errorMessage}", this);
        ProcessTranscriptionResult(null, null, true, errorMessage); // Pass error to central processor
    }

#endif

    // --- Result Processing ---

    /// <summary>
    /// Central method to handle the final transcription result (from local or server).
    /// </summary>
    private void ProcessTranscriptionResult(string rawText, string detectedLanguage, bool isError = false, string errorMessage = null, bool isEmptyAudio = false)
    {
        string finalResult = "";

        // Clear processing message immediately if showing
        if (subtitleText != null && subtitleText.text == "Processing...") UpdateSubtitleDisplay("", false);


        if (isError)
        {
            finalResult = $"Error: {errorMessage ?? "Transcription failed"}";
            UpdateSubtitleDisplay(finalResult, true, 5.0f); // Show error longer
        }
        else if (isEmptyAudio)
        {
            finalResult = ""; // No result to show for empty audio
            UpdateSubtitleDisplay("No audio detected", true, 2.0f); // Inform user
        }
        else
        {
            finalResult = SanitizeWhisperResult(rawText);
            Debug.Log($"Transcribe: Final Result: '{finalResult}' (Language: {detectedLanguage ?? "Unknown"})");

            if (_isSubtitlesEnabled)
            {
                UpdateSubtitleDisplay(finalResult, true, subtitleFadeDelay);
            }
            else
            {
                UpdateSubtitleDisplay("", false); // Ensure hidden
            }
        }

        // --- Send to AI Controller ---
        // Send even if error or empty, let AIController handle it
        if (_currentAIController != null)
        {
            // Let AIController decide how to handle errors/empty strings
            _currentAIController.CreateRequest(isError ? $"[Error: FAILED TO TRANSCRIBE]" : finalResult);

            // Clear the controller reference for the next interaction
            _currentAIController = null;
        }
        else
        {
            // This case should be less likely now with the check in HandleRecordStop
            Debug.LogWarning("Transcribe: Processing finished, but _currentAIController was already null.");
        }
    }

    private string SanitizeWhisperResult(string result)
    {
        if (string.IsNullOrWhiteSpace(result)) return "";
        return result.Replace("[ Inaudible ]", "").Replace("[BLANK_AUDIO]", "").Trim();
    }

    // --- UI Update Methods (from original Transcribe.cs) ---

    private void UpdateSubtitleDisplay(string text, bool showBackground, float fadeDelay = -1f)
    {
        bool shouldDisplay = _isSubtitlesEnabled || fadeDelay > 0;
        bool hasText = !string.IsNullOrEmpty(text);

        if (!shouldDisplay && hasText)
        {
            if (subtitleText != null && subtitleText.enabled) subtitleText.enabled = false;
            if (subtitleBackground != null && subtitleBackground.enabled) subtitleBackground.enabled = false;
            return;
        }

        if (subtitleText != null)
        {
            subtitleText.text = text;
            subtitleText.enabled = hasText;
        }
        if (subtitleBackground != null)
        {
            subtitleBackground.enabled = showBackground && hasText;
        }

        if (_subtitleFadeCoroutine != null)
        {
            StopCoroutine(_subtitleFadeCoroutine);
            _subtitleFadeCoroutine = null;
        }
        if (fadeDelay > 0 && hasText)
        {
            _subtitleFadeCoroutine = StartCoroutine(FadeOutSubtitle(fadeDelay));
        }
        else if (!hasText)
        {
            // Ensure hidden immediately if text is empty
            if (subtitleText != null) subtitleText.enabled = false;
            if (subtitleBackground != null) subtitleBackground.enabled = false;
        }
    }

    private IEnumerator FadeOutSubtitle(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_subtitleFadeCoroutine == null) yield break; // Coroutine was stopped/replaced

        // No need to check _isSubtitlesEnabled here, UpdateSubtitleDisplay handles it
        if (subtitleText != null) { subtitleText.text = ""; subtitleText.enabled = false; }
        if (subtitleBackground != null) subtitleBackground.enabled = false;

        _subtitleFadeCoroutine = null;
    }

    private IEnumerator DisplayTranscriptionProcessing()
    {
        // Shows "Listening..." while _isRecording is true
        string baseText = "Listening";
        int dotCount = 0;
        while (_isRecording)
        {
            dotCount = (dotCount + 1) % 4;
            // Use UpdateSubtitleDisplay to show temporally even if subtitles are off
            UpdateSubtitleDisplay(baseText + new string('.', dotCount), true, 0.5f);
            yield return new WaitForSeconds(0.4f);
        }
        _processingIndicatorCoroutine = null;
        // "Processing..." or final result will overwrite this in UpdateSubtitleDisplay calls
    }
}