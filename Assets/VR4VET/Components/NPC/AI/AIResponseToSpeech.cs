// Purpose: Handles Text-To-Speech using OpenAI or Wit.ai.
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Meta.WitAi.TTS.Utilities; // Required for TTSSpeaker
using System.Text; // For Encoding

public class AIResponseToSpeech : MonoBehaviour
{
    // Configuration
    public string OpenAiVoiceId = "alloy"; // Set via NPCSpawner

    // Dependencies (Should be on the same GameObject or assigned)
    private AudioSource _audioSource;
    private TTSSpeaker _ttsSpeaker; // Reference to the Wit TTSSpeaker component

    // Internal State
    private string _apiKey;
    private const string OpenAI_TTS_API_URL = "https://api.openai.com/v1/audio/speech";
    public bool readyToAnswer { get; private set; } = false; // Flag for AIRequest
    private Coroutine _playAudioCoroutine;

    void Start()
    {
        // Get API Key (Needed only for OpenAI TTS)
        _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        // Don't log error here if empty, WitAI might still be used. Error logged in OpenAIDictate if needed.

        // Ensure AudioSource exists (Needed for OpenAI TTS playback)
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("AIResponseToSpeech: No AudioSource found. Adding one for OpenAI TTS playback.", this);
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Find the TTSSpeaker component (Needed for WitAI TTS)
        // It should be attached dynamically by NPCSpawner via AttachTTSComponents
        StartCoroutine(FindTTSSpeakerWithDelay()); // Wait a frame in case it's added slightly after this Start
    }

    public void SetReadyState(bool isReady)
    {
        readyToAnswer = isReady;
        Debug.Log($"AIResponseToSpeech: Ready state manually set to {isReady}");
    }

    IEnumerator FindTTSSpeakerWithDelay()
    {
        yield return null; // Wait one frame
        _ttsSpeaker = GetComponentInChildren<TTSSpeaker>(); // Search in children as it's attached to a child object
        if (_ttsSpeaker == null)
        {
            Debug.LogWarning("AIResponseToSpeech: Wit.ai TTSSpeaker component not found in children. WitAI TTS will not work.", this);
        }
        else
        {
            Debug.Log("AIResponseToSpeech: Wit.ai TTSSpeaker found.", this);
        }
    }


    // --- OpenAI TTS Method ---
    public IEnumerator OpenAIDictate(string responseText)
    {
        readyToAnswer = false; // Reset flag

        if (string.IsNullOrEmpty(_apiKey))
        {
            Debug.LogError("AIResponseToSpeech (OpenAI): API key not found. Cannot generate speech.", this);
            readyToAnswer = true; // Signal completion (failure)
            yield break; // Exit coroutine
        }
        if (_audioSource == null)
        {
            Debug.LogError("AIResponseToSpeech (OpenAI): AudioSource is missing. Cannot play speech.", this);
            readyToAnswer = true;
            yield break;
        }

        // Stop any previous playback
        StopPlayback();

        Debug.Log($"AIResponseToSpeech (OpenAI): Requesting speech for: '{responseText.Substring(0, Math.Min(responseText.Length, 50))}...'");

        // Sanitize text slightly for JSON compatibility if needed, though input field handles most
        string sanitizedText = responseText.Replace("\"", "'"); // Basic sanitization

        // Construct JSON payload
        // Ensure OpenAiVoiceId has a default value if not set properly
        string voiceId = string.IsNullOrEmpty(OpenAiVoiceId) ? "alloy" : OpenAiVoiceId;
        string jsonData = $"{{\"model\": \"tts-1\", \"input\": \"{sanitizedText}\", \"voice\": \"{voiceId}\"}}"; // Using tts-1 standard model

        using (UnityWebRequest request = new UnityWebRequest(OpenAI_TTS_API_URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer(); // Get audio data
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {_apiKey}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"AIResponseToSpeech (OpenAI) Error: {request.error}\nResponse Code: {request.responseCode}\nResponse Body: {request.downloadHandler?.text}");
                readyToAnswer = true; // Signal completion (failure)
            }
            else
            {
                Debug.Log("AIResponseToSpeech (OpenAI): Received audio data.");
                byte[] audioData = request.downloadHandler.data;

                // Option 1: Play directly from memory (requires audio format knowledge - assume MP3)
                // This avoids saving to disk but might be less robust if format changes.
                // We need a way to load AudioClip from byte array. Unity doesn't have a built-in MP3 decoder runtime AFAIK.
                // Let's stick to saving and loading for now as it's safer.

                // Option 2: Save locally and play (as in original script)
                string filePath = Path.Combine(Application.persistentDataPath, "openai_speech.mp3");
                try
                {
                    File.WriteAllBytes(filePath, audioData);
                    Debug.Log($"AIResponseToSpeech (OpenAI): Audio saved to {filePath}");
                    _playAudioCoroutine = StartCoroutine(PlayAudioFromFile(filePath));
                    // readyToAnswer will be set true inside PlayAudioFromFile after loading
                }
                catch (Exception e)
                {
                    Debug.LogError($"AIResponseToSpeech (OpenAI): Failed to write audio file. Error: {e.Message}");
                    readyToAnswer = true; // Signal completion (failure)
                }
            }
        }
    }

    private IEnumerator PlayAudioFromFile(string filePath)
    {
        Debug.Log($"AIResponseToSpeech (OpenAI): Loading audio from {filePath}");
        // Use file:// prefix for local files
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG)) // Assume MP3
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"AIResponseToSpeech (OpenAI): Error loading audio file: {request.error}");
                readyToAnswer = true; // Signal completion (failure)
            }
            else
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                if (audioClip == null)
                {
                    Debug.LogError("AIResponseToSpeech (OpenAI): Failed to decode audio clip.", this);
                    readyToAnswer = true; // Signal completion (failure)
                }
                else
                {
                    Debug.Log("AIResponseToSpeech (OpenAI): Audio loaded, playing...");
                    _audioSource.clip = audioClip;
                    _audioSource.Play();
                    // We consider it "ready" once playback starts. AIRequest will wait for this.
                    readyToAnswer = true;
                    // Optional: Wait for playback to finish?
                    // yield return new WaitWhile(() => _audioSource.isPlaying);
                    // Debug.Log("OpenAI TTS Playback Finished.");
                    // // Clean up file?
                    // File.Delete(filePath);
                }
            }
        }
        _playAudioCoroutine = null; // Clear coroutine reference
    }

    // --- Wit.ai TTS Method ---
    public IEnumerator WitAIDictate(string responseText)
    {
        Debug.Log($"AIResponseToSpeech: Starting WitAI TTS for response: '{responseText}'");

        readyToAnswer = false; // Reset flag

        if (_ttsSpeaker == null)
        {
            Debug.LogError("AIResponseToSpeech (WitAI): TTSSpeaker component is missing or not assigned. Cannot speak.", this);
            readyToAnswer = true; // Signal completion (failure)
            yield break; // Exit
        }

        // Stop any previous playback from Wit or OpenAI
        StopPlayback();

        Debug.Log($"AIResponseToSpeech (WitAI): Speaking: '{responseText.Substring(0, Math.Min(responseText.Length, 50))}...'");

        // Use the Wit TTSSpeaker component
        _ttsSpeaker.Speak(responseText);

        // Wit TTS usually starts playing very quickly. We can set readyToAnswer almost immediately.
        // However, let's wait a tiny bit to ensure the Speak command is processed.
        yield return new WaitForSeconds(0.1f); // Small delay
        readyToAnswer = true;
        Debug.Log("AIResponseToSpeech (WitAI): Speak command issued.");

        // We don't need to wait for completion here, AIRequest handles the flow.
    }

    // --- Playback Control ---
    public void StopPlayback()
    {
        Debug.Log("AIResponseToSpeech: Stopping playback.");
        // Stop Wit TTS
        if (_ttsSpeaker != null && _ttsSpeaker.IsSpeaking)
        {
            _ttsSpeaker.Stop();
        }
        // Stop OpenAI TTS playback via AudioSource
        if (_audioSource != null && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        // Stop the file loading coroutine if it's running
        if (_playAudioCoroutine != null)
        {
            StopCoroutine(_playAudioCoroutine);
            _playAudioCoroutine = null;
            // Since we interrupted loading/playback, ensure ready flag is set to avoid deadlocks
            readyToAnswer = true;
        }
    }

    // Call this from OnDestroy or when the NPC is deactivated to clean up
    void OnDestroy()
    {
        StopPlayback();
        // Clean up saved audio file if it exists
        string filePath = Path.Combine(Application.persistentDataPath, "openai_speech.mp3");
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to delete temporary TTS file: {e.Message}");
            }
        }
    }
}