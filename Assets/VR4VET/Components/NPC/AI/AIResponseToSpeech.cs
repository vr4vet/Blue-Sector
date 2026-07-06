// Purpose: Handles Text-To-Speech using Wit.ai.
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