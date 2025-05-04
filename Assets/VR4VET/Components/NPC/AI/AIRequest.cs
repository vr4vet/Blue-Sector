using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UploadDTO;

/// <summary>
/// Handles sending requests to the AI backend in runtime.
/// </summary>
public class AIRequest : MonoBehaviour
{
    // Configuration (Set by AIConversationController)
    public string Query;
    [HideInInspector] public int MaxTokens;
    [HideInInspector] public UploadDataDTO RequestPayload;

    // Dependencies (Fetched dynamically)
    private AIResponseToSpeech _aiResponseToSpeech;
    private DialogueBoxController _dialogueBoxController;
    private AIConversationController _aiConversationController;

    // Internal State
    private const string CHATBOT_API_URL = "http://localhost:8000/ask";
    private List<Message> _messagesToSend = new(); // Local copy for request
    private AudioSource _audioSource; // For fallback audio

    /// <summary>
    /// Initializes the AIRequest component and sends LLM request.
    /// </summary>
    void Start()
    {
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

        // Ensure DialogueBoxController Reference
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

        _messagesToSend = new List<Message>(_aiConversationController.messages);
        Message userMessage = new() { role = "user", content = Query };
        _messagesToSend.Add(userMessage);

        StartCoroutine(SendLLMRequest());
    }

    /// <summary>
    /// Sends a request chat-service containing the user's query and logged data from ActionManager.
    /// Data structure is defined in UploadDataDTO.cs, and needs to match the server-side code.
    /// Returns a json response matching the LLMResponse class.
    /// User message and LLM response are added to chat history with successful request and response.
    /// LLM may return a function call, which is then executed. (Currently only teleporting works)
    /// </summary>
    /// <returns></returns>
    IEnumerator SendLLMRequest()
    {
        if (string.IsNullOrWhiteSpace(Query))
        {
            Debug.LogError("AIRequest: Query is empty, aborting request.", this);
            HandleErrorOrFallback("My query was empty.");
            yield break;
        }

        Debug.Log($"AIRequest: Sending request to Chatbot. Query: '{Query}'");


        RequestPayload.chatLog = _messagesToSend;

        string jsonData = JsonUtility.ToJson(RequestPayload);
        Debug.Log($"AIRequest: Sending payload: {jsonData}");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(CHATBOT_API_URL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            Debug.Log($"AIRequest: Server response: {request.downloadHandler.text}");

            _aiConversationController.AddMessage(new Message { role = "user", content = Query });

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
                    LLMResponse response = JsonUtility.FromJson<LLMResponse>(request.downloadHandler.text);

                    Debug.Log($"LLMResponse JSON: {JsonUtility.ToJson(response, true)}");

                    if (response == null || response.choices == null || response.choices.Count == 0 || string.IsNullOrWhiteSpace(response.response))
                    {
                        Debug.LogError("AIRequest Error: Invalid or empty response from OpenAI.");
                        HandleErrorOrFallback("Received an empty response from the AI.");
                    }
                    else
                    {
                        string rawResponseText = response.response;
                        string sanitizedResponseText = SanitizeResponse(rawResponseText);

                        Message assistantMessage = new() { role = "assistant", content = sanitizedResponseText };
                        _aiConversationController.AddMessage(assistantMessage);

                        Debug.Log($"AI Response: {sanitizedResponseText}");

                        if (response.function_call != null)
                        {
                            Debug.Log($"AIRequest: Function call detected: {response.function_call.function_name}");
                            ExecuteFunction(response.function_call.function_name, response.function_call.function_parameters);
                        }
                        if (response.function_call.function_name != "teleport")
                        {
                            // Trigger TTS and UI Update
                            HandleSuccessfulResponse(sanitizedResponseText);
                        }
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

    /// <summary>
    /// Sanitizes the response text to ensure it is clean and ready for display.
    /// </summary>
    /// <param name="text"></param>
    /// <returns>Sanitized response text</returns>
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
            IEnumerator ttsCoroutine = _aiResponseToSpeech.WitAIDictate(responseText);

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
        string fallbackMessage = GetGenericErrorMessage(_aiConversationController?.GetTranscribe()?.CurrentLanguage ?? "en");

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

    /// <summary>
    /// If LLM response contains a function call, it is then executed if it matches the ones from the list.
    /// Should be fairly easy to add more functions in the future. But make sure to define it on the server-side as well.
    /// </summary>
    /// <param name="functionName"></param>
    /// <param name="parameters"></param>
    private void ExecuteFunction(string functionName, string[] parameters)
    {
        switch (functionName)
        {
            case "teleport":
                string location = parameters[0];
                Debug.Log($"Function case teleport to {location}");
                TeleportPlayer(location);
                break;

            case "showObject":
                string objectId = parameters[0];
                Debug.Log($"Showing object");
                /*HighlightObject(objectId);*/
                break;

            default:
                Debug.LogWarning($"Unknown function: {functionName}");
                break;
        }
    }

    /// <summary>
    /// Teleport the player to another scene using AISceneController.
    /// </summary>
    /// <param name="location"></param>
    private void TeleportPlayer(string location)
    {
        Debug.Log($"Teleporting player to {location}");
        AISceneController aiSceneController = GetComponent<AISceneController>();
        if (aiSceneController != null)
        {
            aiSceneController.ChangeScene(location);
        }
        else
        {
            Debug.LogError("AISceneController not found. Cannot teleport player.");
        }
    }
}