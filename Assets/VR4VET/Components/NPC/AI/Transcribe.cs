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
#if WHISPER_UNITY_PACKAGE_AVAILABLE
    public WhisperManager whisper;
    public MicrophoneRecord microphoneRecord;
#endif
    public TextMeshProUGUI subtitleText;
    public Image subtitleBackground;

    // A list of supported language codes, if you allow manual cycling
    private readonly string[] languages = { "en", "no", "de", "nl" };

    // Instead of defaulting to "en", let the manager handle it or start empty
    public string currentLanguage { get; private set; } = "";

#if WHISPER_UNITY_PACKAGE_AVAILABLE
    private WhisperStream _stream;
#endif
    private AIConversationController _currentAIController;
    private Coroutine _subtitleFadeCoroutine;
    private Coroutine _processingIndicatorCoroutine;
    private bool _isSubtitlesEnabled = true;
    private bool _isInitialized = false;
    private bool _isRecording = false;

    async void Start()
    {
#if WHISPER_UNITY_PACKAGE_AVAILABLE
        if (whisper == null) whisper = GetComponent<WhisperManager>();
        if (microphoneRecord == null) microphoneRecord = GetComponent<MicrophoneRecord>();

        if (whisper == null || microphoneRecord == null)
        {
            Debug.LogError("Transcribe: Missing WhisperManager or MicrophoneRecord.", this);
            return;
        }
        if (subtitleText == null) Debug.LogError("Transcribe: subtitleText not assigned!", this);
        if (subtitleBackground == null) Debug.LogError("Transcribe: subtitleBackground not assigned!", this);

        // Now we only set the language if not already specified by the manager
        // or if an empty string is detected
        if (string.IsNullOrEmpty(whisper.language) || whisper.language.Equals("auto", StringComparison.OrdinalIgnoreCase))
        {
            // Fall back to local "languages" array if not set
            currentLanguage = languages[0];
            whisper.language = currentLanguage;
        }
        else
        {
            // Use manager's existing language
            currentLanguage = whisper.language;
        }

        // Continue with microphone checks, etc.
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("Transcribe: No microphone found!", this);
            UpdateSubtitleDisplay("Error: No Microphone Found", true);
            return;
        }
        microphoneRecord.SelectedMicDevice = Microphone.devices[0];

        _stream = await whisper.CreateStream(microphoneRecord);
        if (_stream == null)
        {
            Debug.LogError("Transcribe: Failed to create Whisper stream!", this);
            UpdateSubtitleDisplay("Error: Failed to initialize", true);
            return;
        }

        // Subscribe to events
        _stream.OnResultUpdated += OnResult;
        _stream.OnStreamFinished += OnStreamFinished;

        UpdateSubtitleDisplay("", false);

        _isInitialized = true;
        Debug.Log("Transcribe: Initialization complete.");
#else
        Debug.LogError("Transcribe: Whisper package missing. Transcription disabled.", this);
        UpdateSubtitleDisplay("Transcription Unavailable", true);
#endif
    }

    void Update()
    {
        if (!_isInitialized) return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            CycleLanguage();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleSubtitles();
        }
    }

    public void CycleLanguage()
    {
        // Example: cycle local languages array
        int index = Array.IndexOf(languages, currentLanguage);
        index = (index + 1) % languages.Length;
        currentLanguage = languages[index];
        Debug.Log("Transcribe: Language changed to: " + currentLanguage);
        UpdateSubtitleDisplay($"Language: {currentLanguage.ToUpper()}", true, 2.0f);

#if WHISPER_UNITY_PACKAGE_AVAILABLE
        if (whisper != null)
        {
            whisper.language = currentLanguage;
        }
#endif
    }

    public void ToggleSubtitles()
    {
        _isSubtitlesEnabled = !_isSubtitlesEnabled;
        UpdateSubtitleDisplay($"Subtitles {(_isSubtitlesEnabled ? "ON" : "OFF")}", true, 2.0f);
        if (!_isSubtitlesEnabled)
        {
            UpdateSubtitleDisplay("", false);
        }
    }

    public void StartRecording(AIConversationController aiController)
    {
        if (!_isInitialized) { Debug.LogError("Transcribe: Not initialized.", this); return; }
        if (_isRecording) { Debug.LogWarning("Transcribe: Already recording.", this); return; }
        if (aiController == null) { Debug.LogError("Transcribe: AIController is null.", this); return; }

#if WHISPER_UNITY_PACKAGE_AVAILABLE
        _currentAIController = aiController;
        Debug.Log($"Transcribe: Starting recording for {aiController.gameObject.name}");
        _isRecording = true;

        try
        {
            microphoneRecord.StartRecord();
            _stream.StartStream();
            if (_processingIndicatorCoroutine != null) StopCoroutine(_processingIndicatorCoroutine);
            _processingIndicatorCoroutine = StartCoroutine(DisplayTranscriptionProcessing());
        }
        catch (Exception e)
        {
            Debug.LogError($"Transcribe: Error starting recording: {e.Message}", this);
            UpdateSubtitleDisplay("Error: Mic/Transcription Start Failed", true);
            _isRecording = false;
        }
#endif
    }

    public void EndRecording()
    {
        if (!_isInitialized || !_isRecording) return;

#if WHISPER_UNITY_PACKAGE_AVAILABLE
        Debug.Log("Transcribe: Stopping recording.");
        _isRecording = false;

        try
        {
            microphoneRecord.StopRecord();
            _stream.StopStream();
        }
        catch (Exception e)
        {
            Debug.LogError($"Transcribe: Error stopping recording: {e.Message}", this);
        }
#endif
    }

    public bool IsRecording() { return _isRecording; }

#if WHISPER_UNITY_PACKAGE_AVAILABLE
    private void OnResult(string result) { }
    private void OnStreamFinished(string finalResult)
    {
        _isRecording = false;
        Debug.Log($"Transcribe: Stream finished. Final result: '{finalResult}'");

        if (_processingIndicatorCoroutine != null)
        {
            StopCoroutine(_processingIndicatorCoroutine);
            _processingIndicatorCoroutine = null;
        }

        string processed = SanitizeWhisperResult(finalResult);

        if (_isSubtitlesEnabled)
        {
            UpdateSubtitleDisplay(processed, true, 5f);
        }
        else
        {
            UpdateSubtitleDisplay("", false);
        }

        if (_currentAIController != null)
        {
            _currentAIController.CreateRequest(processed);
        }
        else
        {
            Debug.LogError("Transcribe: _currentAIController is null.", this);
        }
    }

    private string SanitizeWhisperResult(string result)
    {
        if (string.IsNullOrWhiteSpace(result)) return "";
        return result.Replace("[ Inaudible ]", "").Replace("[BLANK_AUDIO]", "").Trim();
    }
#endif

    private void UpdateSubtitleDisplay(string text, bool showBackground, float fadeDelay = -1f)
    {
        if (subtitleText != null)
        {
            subtitleText.text = text;
            subtitleText.enabled = !string.IsNullOrEmpty(text);
        }
        if (subtitleBackground != null)
        {
            subtitleBackground.enabled = showBackground && !string.IsNullOrEmpty(text);
        }

        if (_subtitleFadeCoroutine != null)
        {
            StopCoroutine(_subtitleFadeCoroutine);
            _subtitleFadeCoroutine = null;
        }
        if (fadeDelay > 0 && showBackground && !string.IsNullOrEmpty(text))
        {
            _subtitleFadeCoroutine = StartCoroutine(FadeOutSubtitle(fadeDelay));
        }
    }

    private IEnumerator FadeOutSubtitle(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateSubtitleDisplay("", false);
        _subtitleFadeCoroutine = null;
    }

    private IEnumerator DisplayTranscriptionProcessing()
    {
        // Show “...” while waiting
        string baseText = "Listening";
        int dotCount = 0;
        while (true)
        {
            dotCount = (dotCount + 1) % 4;
            UpdateSubtitleDisplay(baseText + new string('.', dotCount), true);
            yield return new WaitForSeconds(0.4f);
        }
    }

    void OnDestroy()
    {
#if WHISPER_UNITY_PACKAGE_AVAILABLE
        if (_stream != null)
        {
            _stream.OnResultUpdated -= OnResult;
            _stream.OnStreamFinished -= OnStreamFinished;
        }
#endif
    }
}
