using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

// Ensure Whisper namespace is available
#if WHISPER_UNITY_PACKAGE_AVAILABLE
using Whisper;
using Whisper.Utils;
using WhisperResultType = Whisper.WhisperResult; // Type alias to avoid conflict
#else
// Define dummy types if Whisper package is missing
namespace Whisper
{
    public class WhisperManager : MonoBehaviour { }
    public class MicrophoneRecord : MonoBehaviour { }
    public class AudioChunk { }
    public class WhisperResult { public string Result; }
    public class WhisperStream
    {
        public event Action<string> OnResultUpdated;
        public event Action<WhisperResultType> OnSegmentUpdated;
        public event Action<WhisperResultType> OnSegmentFinished;
        public event Action<string> OnStreamFinished;
        public void StartStream() { }
        public void StopStream() { }
    }
}
namespace Whisper.Utils { public static class AudioUtils { } }
#endif

/*
    This script resides on the TranscriptionManager GameObject, which needs to be present in the scene for transcription
    to work. Subtitles also require a GameObject in the scene. It handles all transcription locally, and updates the
    subtitles text (even if they are active or not). Currently English, Norwegian, German and Dutch can be switched
    between using the 'L' key on the keyboard. It also runs the AIConversationController class 'CreateRequest' method
    once transcription has been completed. Subtitles can be toggled by pressing the 'U' key on the keyboard.
 */

public class Transcribe : MonoBehaviour
{
    // Dependencies (Assign in Inspector)
#if WHISPER_UNITY_PACKAGE_AVAILABLE
    public WhisperManager whisper;
    public MicrophoneRecord microphoneRecord;
#endif
    public TextMeshProUGUI subtitleText; // Assign the TMP_Text component for subtitles
    public Image subtitleBackground; // Assign the Image component for the subtitle background

    // Configuration
    private readonly string[] languages = { "en", "no", "de", "nl" }; // Supported languages
    private const int SUBTITLE_DURATION_SECONDS = 5; // Subtitle display time
    private int currentLanguageIndex = 0;
    public string currentLanguage { get; private set; } = "en"; // Default language

    // Internal State
#if WHISPER_UNITY_PACKAGE_AVAILABLE
    private WhisperStream _stream;
#endif
    private AIConversationController _currentAIController; // The AI NPC currently being spoken to
    private Coroutine _subtitleFadeCoroutine;
    private Coroutine _processingIndicatorCoroutine;
    private bool _isSubtitlesEnabled = true; // Control subtitle visibility
    private bool _isInitialized = false;
    private bool _isRecording = false;


    async void Start()
    {
#if WHISPER_UNITY_PACKAGE_AVAILABLE
        // Get components if not assigned
        if (whisper == null) whisper = GetComponent<WhisperManager>();
        if (microphoneRecord == null) microphoneRecord = GetComponent<MicrophoneRecord>();

        if (whisper == null || microphoneRecord == null)
        {
            Debug.LogError("Transcribe: WhisperManager or MicrophoneRecord component not found!", this);
            return;
        }

        // Check for subtitle UI assignment
        if (subtitleText == null) Debug.LogError("Transcribe: subtitleText (TextMeshProUGUI) is not assigned!", this);
        if (subtitleBackground == null) Debug.LogError("Transcribe: subtitleBackground (Image) is not assigned!", this);

        // Initial language setup
        currentLanguage = languages[currentLanguageIndex];
        whisper.language = currentLanguage; // Set initial language in WhisperManager

        // Initialize microphone devices check
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("Transcribe: No microphone devices found!", this);
            UpdateSubtitleDisplay("Error: No Microphone Found", true); // Show error permanently
            return; // Cannot proceed without a microphone
        }
        microphoneRecord.SelectedMicDevice = Microphone.devices[0]; // Select the first available mic

        // Create Whisper stream (async operation)
        Debug.Log("Transcribe: Creating Whisper stream...");
        _stream = await whisper.CreateStream(microphoneRecord); // Make sure 'await' is here
        Debug.Log("Transcribe: Whisper stream created.");


        // Subscribe to events
        if (_stream != null)
        {
            _stream.OnResultUpdated += OnResult; // Intermediate results
            //_stream.OnSegmentUpdated += OnSegmentUpdated; // Optional: More detailed updates
            //_stream.OnSegmentFinished += OnSegmentFinished; // Optional: Segment completion
            _stream.OnStreamFinished += OnStreamFinished; // Final result
        }
        else
        {
            Debug.LogError("Transcribe: Failed to create Whisper stream!", this);
            UpdateSubtitleDisplay("Error: Failed to initialize transcription", true);
            return;
        }

        // Don't subscribe to microphoneRecord events directly if WhisperStream handles it internally
        // microphoneRecord.OnRecordStop += OnRecordStop; // Probably not needed if using _stream.OnStreamFinished

        // Initial UI state
        UpdateSubtitleDisplay("", false); // Hide subtitles initially

        _isInitialized = true;
        Debug.Log("Transcribe: Initialization complete.");

#else
        Debug.LogError("Transcribe: Whisper package seems missing. Transcription disabled.", this);
        UpdateSubtitleDisplay("Transcription Unavailable", true);
#endif
    }

    void Update()
    {
        if (!_isInitialized) return;

        // Language switching (Keyboard L)
        if (Input.GetKeyDown(KeyCode.L))
        {
            CycleLanguage();
        }

        // Subtitle toggle (Keyboard U)
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleSubtitles();
        }
    }

    public void CycleLanguage()
    {
        currentLanguageIndex = (currentLanguageIndex + 1) % languages.Length;
        currentLanguage = languages[currentLanguageIndex];
        Debug.Log("Transcribe: Language changed to: " + currentLanguage);
        UpdateSubtitleDisplay($"Language: {currentLanguage.ToUpper()}", true, 2.0f); // Show temporary confirmation

#if WHISPER_UNITY_PACKAGE_AVAILABLE
        if (whisper != null)
        {
            whisper.language = currentLanguage; // Update WhisperManager
             Debug.Log("Transcribe: Whisper language updated.");
        }
#endif
    }

    public void ToggleSubtitles()
    {
        _isSubtitlesEnabled = !_isSubtitlesEnabled;
        Debug.Log("Transcribe: Subtitles " + (_isSubtitlesEnabled ? "Enabled" : "Disabled"));
        if (!_isSubtitlesEnabled)
        {
            UpdateSubtitleDisplay("", false); // Hide immediately if disabling
        }
        UpdateSubtitleDisplay($"Subtitles {(_isSubtitlesEnabled ? "ON" : "OFF")}", true, 2.0f); // Show temporary confirmation
    }

    // Called by AIConversationController to start the recording process
    public void StartRecording(AIConversationController aiController)
    {
        if (!_isInitialized) { Debug.LogError("Transcribe: Not initialized, cannot start recording.", this); return; }
        if (_isRecording) { Debug.LogWarning("Transcribe: Already recording.", this); return; }
        if (aiController == null) { Debug.LogError("Transcribe: AIController reference is null.", this); return; }

#if WHISPER_UNITY_PACKAGE_AVAILABLE
        _currentAIController = aiController; // Store reference to the NPC we're talking to
         Debug.Log($"Transcribe: Starting recording for {aiController.gameObject.name}");
        _isRecording = true;

        // Start microphone and Whisper stream
        try {
            microphoneRecord.StartRecord(); // Start hardware recording
            _stream.StartStream();          // Start processing stream
            if (_processingIndicatorCoroutine != null) StopCoroutine(_processingIndicatorCoroutine);
            _processingIndicatorCoroutine = StartCoroutine(DisplayTranscriptionProcessing()); // Show "..."
        } catch (Exception e) {
             Debug.LogError($"Transcribe: Error starting recording: {e.Message}", this);
             UpdateSubtitleDisplay("Error: Mic/Transcription Start Failed", true);
             _isRecording = false;
        }
#endif
    }

    // Called by AIConversationController to stop the recording process
    public void EndRecording()
    {
        if (!_isInitialized || !_isRecording) return;

#if WHISPER_UNITY_PACKAGE_AVAILABLE
    Debug.Log("Transcribe: Stopping recording.");
    _isRecording = false;

    try {
        microphoneRecord.StopRecord(); // Stop hardware recording
        // Uncomment this line:
        _stream.StopStream(); // Explicitly stop the stream
        
        // Processing indicator will be stopped in OnStreamFinished
    } catch (Exception e) {
        Debug.LogError($"Transcribe: Error stopping recording: {e.Message}", this);
    }
#endif
    }

    public bool IsRecording()
    {
        return _isRecording;
    }


#if WHISPER_UNITY_PACKAGE_AVAILABLE
    // --- Whisper Event Handlers ---

    private void OnResult(string result)
    {
        // Called with intermediate transcription results
         // Optional: Update subtitle text with intermediate results for live feedback
         // if (_isSubtitlesEnabled && _isRecording) {
         //    UpdateSubtitleDisplay(result, true); // Show intermediate results without fadeout yet
         // }
        // Debug.Log($"Whisper Intermediate Result: {result}");
    }

    // Optional detailed segment handlers
// private void OnSegmentUpdated(WhisperResultType segment) { Debug.Log($"Segment updated: {segment.Result}"); }
// private void OnSegmentFinished(WhisperResultType segment) { Debug.Log($"Segment finished: {segment.Result}"); }

    private void OnStreamFinished(string finalResult)
    {
        // Called when Whisper finishes processing the recorded audio
        _isRecording = false; // Ensure recording state is updated
         Debug.Log($"Transcribe: Stream finished. Final Result: '{finalResult}'");

        // Stop the "..." processing indicator
        if (_processingIndicatorCoroutine != null)
        {
            StopCoroutine(_processingIndicatorCoroutine);
            _processingIndicatorCoroutine = null;
        }

        // Sanitize the final result
        string processedResult = SanitizeWhisperResult(finalResult);

        // Update subtitles with the final result (if enabled)
        if (_isSubtitlesEnabled)
        {
             // Show the final text, start the fadeout timer
            UpdateSubtitleDisplay(processedResult, true, SUBTITLE_DURATION_SECONDS);
        } else {
             // Ensure subtitles are hidden if disabled
             UpdateSubtitleDisplay("", false);
        }

        // Send the processed result to the AI controller *that initiated the recording*
        if (_currentAIController != null)
        {
            _currentAIController.CreateRequest(processedResult);
        }
        else
        {
            Debug.LogError("Transcribe: _currentAIController is null when stream finished. Cannot send result.", this);
        }
         // Clear the reference after processing is done
         // _currentAIController = null; // Consider if needed, might cause issues if EndRecording called before this
    }

     private string SanitizeWhisperResult(string result) {
         if (string.IsNullOrWhiteSpace(result)) return "";

         // Remove common Whisper artifacts
         result = result.Replace("[ Inaudible ]", "").Replace("[BLANK_AUDIO]", "");
         // Add any other specific replacements needed
         // result = result.Replace("...", ""); // Example

         return result.Trim();
     }

#endif // WHISPER_UNITY_PACKAGE_AVAILABLE

    // --- UI Update Methods ---

    private void UpdateSubtitleDisplay(string text, bool showBackground, float fadeDelay = -1f)
    {
        if (subtitleText != null)
        {
            subtitleText.text = text;
            subtitleText.enabled = !string.IsNullOrEmpty(text); // Only enable text if not empty
        }

        if (subtitleBackground != null)
        {
            subtitleBackground.enabled = showBackground && !string.IsNullOrEmpty(text); // Show background only if text exists
        }

        // Handle fadeout timer
        if (_subtitleFadeCoroutine != null)
        {
            StopCoroutine(_subtitleFadeCoroutine); // Stop previous fade timer
            _subtitleFadeCoroutine = null;
        }
        if (fadeDelay > 0 && showBackground && !string.IsNullOrEmpty(text)) // Only start fade if showing something
        {
            _subtitleFadeCoroutine = StartCoroutine(FadeOutSubtitle(fadeDelay));
        }
    }

    private IEnumerator FadeOutSubtitle(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateSubtitleDisplay("", false); // Hide text and background
        _subtitleFadeCoroutine = null;
    }

    private IEnumerator DisplayTranscriptionProcessing()
    {
        // Show "..." indicator while recording/processing
        string baseText = "Listening";
        int dotCount = 0;
        while (true)
        {
            dotCount = (dotCount + 1) % 4;
            UpdateSubtitleDisplay(baseText + new string('.', dotCount), true); // Show with background, no fade
            yield return new WaitForSeconds(0.4f);
        }
    }


    void OnDestroy()
    {
#if WHISPER_UNITY_PACKAGE_AVAILABLE
        // Unsubscribe from events to prevent errors
        if (_stream != null)
        {
             _stream.OnResultUpdated -= OnResult;
             _stream.OnStreamFinished -= OnStreamFinished;
             // Unsubscribe from other segment events if used
        }
        // if (microphoneRecord != null) {
        //     microphoneRecord.OnRecordStop -= OnRecordStop;
        // }
#endif
    }
}