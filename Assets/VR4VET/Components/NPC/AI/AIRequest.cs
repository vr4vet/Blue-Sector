// Purpose: Sends request to OpenAI API, handles response and fallback.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text; // For UTF8Encoding
// Note: Uses Message, OpenAIRequest, OpenAIResponse from OpenAIResponseSerializer.cs

public class AIRequest : MonoBehaviour
{
    // Configuration (Set by AIConversationController)
    public string query;
    public int maxTokens;

    // Dependencies (Fetched dynamically)
    private AIResponseToSpeech _aiResponseToSpeech;
    private DialogueBoxController _dialogueBoxController;
    private AIConversationController _aiConversationController;

    // Internal State
    private string _apiKey;
    private const string OpenAI_API_URL = "https://api.openai.com/v1/chat/completions";
    private List<Message> _messagesToSend = new(); // Local copy for request
    private AudioSource _audioSource; // For fallback audio

    void Start()
    {
        // Get API Key
        _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(_apiKey))
        {
            Debug.LogError("AIRequest: OpenAI API key not found. Make sure it's set in the .env file and EnvLoader is working.", this);
            Destroy(this); // Destroy self if key is missing
            return;
        }

        // Get Component References
        _aiConversationController = GetComponent<AIConversationController>();
        if (_aiConversationController == null)
        {
            Debug.LogError("AIRequest: AIConversationController not found on the same GameObject.", this);
            Destroy(this); return;
        }

        _aiResponseToSpeech = GetComponent<AIResponseToSpeech>();
        if (_aiResponseToSpeech == null)
        {
            Debug.LogError("AIRequest: AIResponseToSpeech component not found on the same GameObject.", this);
            Destroy(this); return;
        }

        // Add Fix 1: Ensure DialogueBoxController Reference
        _dialogueBoxController = GetComponent<DialogueBoxController>();
        if (_dialogueBoxController == null)
        {
            _dialogueBoxController = GetComponentInParent<DialogueBoxController>();
            if (_dialogueBoxController == null)
            {
                Debug.LogError("AIRequest: DialogueBoxController component not found in the parent hierarchy.", this);
                Destroy(this);
                return;
            }
        }

        // Ensure AudioSource exists for fallback
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogWarning("AIRequest: No AudioSource found. Adding one for fallback audio.", this);
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Prepare messages for the request
        _messagesToSend = new List<Message>(_aiConversationController.messages);
        Message userMessage = new() { role = "user", content = query };
        _messagesToSend.Add(userMessage);

        // Start the API call
        StartCoroutine(SendOpenAIRequest());
    }

    IEnumerator SendOpenAIRequest()
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            Debug.LogError("AIRequest: Query is empty, aborting request.", this);
            HandleErrorOrFallback("My query was empty.");
            yield break;
        }

        Debug.Log($"AIRequest: Sending request to OpenAI. Query: '{query}'");

        OpenAIRequest requestPayload = new()
        {
            model = "gpt-3.5-turbo",
            messages = _messagesToSend,
            max_tokens = maxTokens > 0 ? maxTokens : 150
        };

        string jsonData = JsonUtility.ToJson(requestPayload);

        Debug.Log($"AIRequest: Full request payload:\n{jsonData}");

        using (UnityWebRequest request = new(OpenAI_API_URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {_apiKey}");

            yield return request.SendWebRequest();

            _aiConversationController.AddMessage(new Message { role = "user", content = query });

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"AIRequest Error: {request.error}\nResponse Code: {request.responseCode}\nResponse Body: {request.downloadHandler?.text}");
                HandleErrorOrFallback(request.error, request.responseCode);
            }
            else
            {
                Debug.Log("AIRequest Success: Received response from OpenAI.");
                try
                {
                    OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(request.downloadHandler.text);

                    if (response == null || response.choices == null || response.choices.Count == 0 || string.IsNullOrWhiteSpace(response.choices[0].message.content))
                    {
                        Debug.LogError("AIRequest Error: Invalid or empty response from OpenAI.");
                        HandleErrorOrFallback("Received an empty response from the AI.");
                    }
                    else
                    {
                        string rawResponseText = response.choices[0].message.content;
                        string sanitizedResponseText = SanitizeResponse(rawResponseText);

                        Message assistantMessage = new() { role = "assistant", content = sanitizedResponseText };
                        _aiConversationController.AddMessage(assistantMessage);

                        Debug.Log($"AI Response: {sanitizedResponseText}");

                        // Trigger TTS and UI Update
                        HandleSuccessfulResponse(sanitizedResponseText);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"AIRequest Error: Failed to parse OpenAI response. Error: {e.Message}\nJSON: {request.downloadHandler.text}");
                    HandleErrorOrFallback("There was an issue processing the AI's response.");
                }
            }
        }
        yield return null;
    }

    string SanitizeResponse(string text)
    {
        return text
            .Replace("\n", " ")
            .Replace("\r", "")
            .Replace("\\\"", "'")
            .Trim();
    }

    void HandleSuccessfulResponse(string responseText)
    {
        if (_aiResponseToSpeech != null && _dialogueBoxController != null)
        {
            IEnumerator ttsCoroutine = _dialogueBoxController.useWitAI
                ? _aiResponseToSpeech.WitAIDictate(responseText)
                : _aiResponseToSpeech.OpenAIDictate(responseText);

            // Add Fix 4 & Fix 5: Synchronize Thinking Animation and Response Display
            StartCoroutine(ProcessResponseSequence(ttsCoroutine, responseText));
            Debug.Log($"HandleSuccessfulResponse: Passing response to DisplayResponse: '{responseText}'");

        }
        else
        {
            Debug.LogError("AIRequest: Cannot handle successful response - AIResponseToSpeech or DialogueBoxController is missing.", this);
        }
    }

    IEnumerator ProcessResponseSequence(IEnumerator ttsCoroutine, string responseText)
    {
        _dialogueBoxController.stopThinking();

        yield return StartCoroutine(ttsCoroutine);

        Debug.Log($"AIRequest: Displaying response: '{responseText}'");
        yield return StartCoroutine(_dialogueBoxController.DisplayResponse(responseText));
    }

    void HandleErrorOrFallback(string error, long responseCode = -1)
    {
        Debug.Log($"Handling Error/Fallback. Code: {responseCode}, Error: {error}");
        string fallbackMessage = GetGenericErrorMessage(_aiConversationController?.GetTranscribe()?.currentLanguage ?? "en");

        if (_dialogueBoxController != null)
        {
            _dialogueBoxController.stopThinking();
            StartCoroutine(_dialogueBoxController.DisplayResponse(fallbackMessage));
        }
    }

    string GetGenericErrorMessage(string langCode)
    {
        return langCode switch
        {
            "no" => " Beklager, noe gikk galt. Vennligst prøv igjen.",
            "de" => " Entschuldigung, etwas ist schief gelaufen. Bitte versuchen Sie es erneut.",
            "nl" => " Sorry, er is iets misgegaan. Probeer het opnieuw.",
            _ => " I'm sorry, something went wrong. Please try again.",
        };
    }
}