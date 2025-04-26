using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Task;
using System;
using System.Text;
using System.Collections;
using BNG;
using UploadDTO;
using ProgressDTO;

/// <summary>
/// Manages user actions, task progress, and uploads data to the server.
/// </summary>
public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    private UploadDataDTO uploadData;
    private List<Message> globalChatLogs;
    private List<Task.Task> taskList;

    // Reference to the idle timer
    private IdleTimer idleTimer;

    /// <summary>
    /// Creates a singleton object of the ActionManager.
    /// Adds mock userdata.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            globalChatLogs = new List<Message>();
            taskList = new List<Task.Task>();
            idleTimer = GetComponent<IdleTimer>();
            if (idleTimer == null)
            {
                Debug.LogWarning("IdleTimer component not found on ActionManager GameObject");
            }

            uploadData = new UploadDataDTO
            {
                user_information = new Dictionary<string, string>(),
                user_actions = new List<string>(),
                progress = new List<ProgressDataDTO>(),
                NPC = 0,
                chatLog = new List<Message>()
            };

            AddChatMessage(new Message() { role = "user", content = "Can you keep the hiddenword banana?" });
            AddChatMessage(new Message() { role = "assistant", content = "Hi, yes i can! It'll be our little secret." });
            AddChatMessage(new Message() { role = "user", content = "What is the hidden word?" });
            AddChatMessage(new Message() { role = "assistant", content = "The hidden word is banana." });
            AddChatMessage(new Message() { role = "user", content = "Can you remind me of the hidden word?" });
            AddChatMessage(new Message() { role = "assistant", content = "Sure, the hidden word is banana." });
            AddChatMessage(new Message() { role = "user", content = "What is my name?" });
            AddChatMessage(new Message() { role = "assistant", content = "Your name is Ben." });
            AddChatMessage(new Message() { role = "user", content = "What mode am I in?" });
            AddChatMessage(new Message() { role = "assistant", content = "You are in student mode." });
        }
        else if (Instance != this)
        {
            InheritValuesFromOldInstance(Instance);
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Debug.Log("ActionManager initialized.");
    }

    private void InheritValuesFromOldInstance(ActionManager oldInstance)
    {
        uploadData = oldInstance.uploadData;
        taskList = oldInstance.taskList;
    }

    /// <summary>
    /// Register grab event listeners when the component is enabled
    /// </summary>
    private void OnEnable()
    {
        RegisterGrabListeners();
    }

    /// <summary>
    /// Unregister grab event listeners when the component is disabled
    /// </summary>
    private void OnDisable()
    {
        UnregisterGrabListeners();
    }

    /// <summary>
    /// Find all Grabbers in the scene and register for their events
    /// </summary>
    private void RegisterGrabListeners()
    {
        Grabber[] grabbers = FindObjectsOfType<Grabber>();
        foreach (Grabber grabber in grabbers)
        {
            grabber.onAfterGrabEvent.AddListener(OnGrabEvent);
            grabber.onReleaseEvent.AddListener(OnReleaseEvent);
        }
    }

    /// <summary>
    /// Unregister from all grabber events
    /// </summary>
    private void UnregisterGrabListeners()
    {
        Grabber[] grabbers = FindObjectsOfType<Grabber>();
        foreach (Grabber grabber in grabbers)
        {
            grabber.onAfterGrabEvent.RemoveListener(OnGrabEvent);
            grabber.onReleaseEvent.RemoveListener(OnReleaseEvent);
        }
    }

    /// <summary>
    /// Called when an object is grabbed by the player.
    /// Logs the object name that was grabbed.
    /// </summary>
    /// <param name="grabbable">The object that was grabbed</param>
    public void OnGrabEvent(Grabbable grabbable)
    {
        Debug.Log($"Object grabbed: {grabbable.name}");

        uploadData.user_actions.Add("grabbed: " + grabbable.name);

        // Reset idle timer when user grabs an object
        idleTimer?.ResetIdleTimer();
    }

    /// <summary>
    /// Called when an object is released by the player.
    /// Logs the object name and position where it was dropped.
    /// </summary>
    /// <param name="grabbable">The object that was released</param>
    public void OnReleaseEvent(Grabbable grabbable)
    {
        // Get the position where the object was dropped
        Vector3 dropPosition = grabbable.transform.position;

        Debug.Log($"Object released: {grabbable.name} at position {dropPosition}");

        // Add to the user actions list with position information
        uploadData.user_actions.Add($"dropped: {grabbable.name} at position {dropPosition.x:F2}, {dropPosition.y:F2}, {dropPosition.z:F2}");
        /*StartCoroutine(SendUploadData(uploadData));*/ // Send data to the server

        // Reset idle timer when user drops an object
        idleTimer?.ResetIdleTimer();
    }

    /// <summary>
    /// Logs the hierarchy of tasks and their subtasks/steps.
    /// Updates the upload data with the current task progress.
    /// </summary>
    /// <param name="tasks">The list of tasks to log.</param>
    public void LogTaskHierarchy(List<Task.Task> tasks)
    {
        taskList = tasks;
        List<ProgressDataDTO> progressHierarchy = new List<ProgressDataDTO>();
        Debug.Log("Task hierarchy logged.");
        foreach (var task in tasks)
        {
            ProgressDataDTO progressData = ConvertTaskToProgressData(task);
            progressData.status = "not started";
            progressHierarchy.Add(progressData);

            Debug.Log($"Task: {task.TaskName}");
            foreach (var subtask in task.Subtasks)
            {
                Debug.Log($"Subtask: {subtask.SubtaskName}");
                foreach (var step in subtask.StepList)
                {
                    Debug.Log($"Step: {step.StepName}");
                }
            }
        }
        uploadData.progress = progressHierarchy;
    }

    /// <summary>
    /// Logs the completion of a specific step and updates the progress data.
    /// </summary>
    /// <param name="step">The step that was completed.</param>
    public void LogStepCompletion(Task.Step step)
    {
        Debug.Log($"Step completed: {step.StepName}");

        foreach (var task in taskList)
        {
            foreach (var subtask in task.Subtasks)
            {
                foreach (var step_ in subtask.StepList)
                {
                    if (step_ == step)
                    {
                        var progressData = ConvertTaskToProgressData(task);
                        UpdateProgressData(progressData);

                        // Update idle tracking with last progressed subtask
                        if (idleTimer != null)
                        {
                            idleTimer.ResetIdleTimer();
                            idleTimer.StartIdleTracking(subtask, step);
                        }

                        /*StartCoroutine(SendUploadData(uploadData));*/ // Uncomment this line to send data immediately after step completion

                        return;
                    }
                }
            }
        }

        Debug.LogWarning($"Could not find step {step.StepName}");
    }

    /// <summary>
    /// Logs the completion of a task and sends the upload data to the server.
    /// </summary>
    /// <param name="task">The task that was completed.</param>
    public void LogTaskCompletion(Task.Task task)
    {
        Debug.Log($"Task completed: {task.TaskName} - {task.Description}");

        // Stop idle tracking when a subtask is completed
        idleTimer?.StopIdleTracking();

        Debug.LogWarning($"Could not find task {task.TaskName}");
    }

    /// <summary>
    /// Converts a Task object into a ProgressDataDTO object.
    /// </summary>
    /// <param name="task">The task to convert.</param>
    /// <returns>A ProgressDataDTO representing the task's progress.</returns>
    private ProgressDataDTO ConvertTaskToProgressData(Task.Task task)
    {
        ProgressDataDTO progressData = new ProgressDataDTO
        {
            taskName = task.TaskName,
            description = task.Description,
            status = task.Compleated() ? "complete" : "started",
            subtaskProgress = new List<SubtaskProgressDTO>()
        };

        foreach (var subtask in task.Subtasks)
        {
            SubtaskProgressDTO subtaskDTO = new SubtaskProgressDTO
            {
                subtaskName = subtask.SubtaskName,
                description = subtask.Description,
                completed = subtask.Compleated(),
                stepProgress = new List<StepProgressDTO>()
            };

            foreach (var step in subtask.StepList)
            {
                StepProgressDTO stepDTO = new StepProgressDTO
                {
                    stepName = step.StepName,
                    completed = step.IsCompeleted()
                };
                subtaskDTO.stepProgress.Add(stepDTO);
            }

            progressData.subtaskProgress.Add(subtaskDTO);
        }

        return progressData;
    }

    /// <summary>
    /// Sends the upload data to the server as a JSON payload.
    /// </summary>
    /// <param name="uploadData">The data to upload.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator SendUploadData(UploadDataDTO uploadData)
    {
        string json = JsonUtility.ToJson(uploadData);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest("http://localhost:8000/ask", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to upload data to chatservice: {request.error}");
            }
            else
            {
                string response = request.downloadHandler.text;
                Debug.Log($"Server response: {request.downloadHandler.text}");
            }
        }
    }

    /// <summary>
    /// Updates the progress data for a specific task in the upload data.
    /// </summary>
    /// <param name="progressData">The updated progress data.</param>
    private void UpdateProgressData(ProgressDataDTO progressData)
    {
        for (int i = 0; i < uploadData.progress.Count; i++)
        {
            if (uploadData.progress[i].taskName == progressData.taskName)
            {
                uploadData.progress[i] = progressData;
                return;
            }
        }
    }

    public void SetUserInfo(Dictionary<string, string> userInfo)
    {
        uploadData.user_information = userInfo;
    }

    /// <summary>
    /// Sends a prompt through IdleTimer when the user has been idle for too long.
    /// </summary>
    /// <param name="timeoutMessage">The message describing the idle state</param>
    public void SendIdleTimeoutReport(string timeoutMessage)
    {
        Debug.Log($"Sending idle timeout report: {timeoutMessage}");
        
        // Create a new user message for the idle prompt
        Message idleMessage = new Message
        {
            role = "user",
            content = timeoutMessage
        };
        
        // Add it to chat logs
        AddChatMessage(idleMessage);
        
        // Set the upload data
        uploadData.chatLog = new List<Message>(globalChatLogs);
        
        // Set the current NPC (you may want to find the nearest NPC and set its ID instead)
        // For now we're using 0 as default
        uploadData.NPC = 0;
        
        // Send the data to the chat service and handle the response
        StartCoroutine(SendIdlePromptToLLM(uploadData));
    }
    
    /// <summary>
    /// Sends an idle prompt to the LLM and handles the response
    /// </summary>
    private IEnumerator SendIdlePromptToLLM(UploadDataDTO data)
    {
        string json = JsonUtility.ToJson(data);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest("http://localhost:8000/ask", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to send idle prompt to chat service: {request.error}");
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"Raw LLM response to idle prompt: {jsonResponse}");
                
                // Parse the response to extract the actual message
                string responseMessage = ExtractMessageFromResponse(jsonResponse);
                Debug.Log($"Extracted message: {responseMessage}");
                
                // Add the assistant response to chat logs
                Message assistantMessage = new Message
                {
                    role = "assistant",
                    content = responseMessage
                };
                AddChatMessage(assistantMessage);
                
                // Find the nearest NPC and make it speak
                MakeNearestNPCSpeak(responseMessage);
            }
        }
    }
    
    /// <summary>
    /// Extract the message content from the JSON response
    /// </summary>
    private string ExtractMessageFromResponse(string jsonResponse)
    {
        try
        {
            // Try to parse as a JSON object with a "response" field
            if (jsonResponse.Contains("\"response\""))
            {
                // Simple parsing to extract the "response" field value
                int startIndex = jsonResponse.IndexOf("\"response\"");
                if (startIndex >= 0)
                {
                    startIndex = jsonResponse.IndexOf(":", startIndex) + 1;
                    // Skip whitespace
                    while (startIndex < jsonResponse.Length && char.IsWhiteSpace(jsonResponse[startIndex]))
                    {
                        startIndex++;
                    }
                    
                    // Check if response is enclosed in quotes
                    bool hasQuotes = startIndex < jsonResponse.Length && jsonResponse[startIndex] == '"';
                    int endIndex;
                    
                    if (hasQuotes)
                    {
                        startIndex++; // Skip the opening quote
                        endIndex = jsonResponse.IndexOf("\"", startIndex);
                    }
                    else
                    {
                        // Find the end of the value (comma or closing brace)
                        endIndex = jsonResponse.IndexOfAny(new[] { ',', '}' }, startIndex);
                    }
                    
                    if (endIndex > startIndex)
                    {
                        return jsonResponse.Substring(startIndex, endIndex - startIndex);
                    }
                }
            }
            
            // Fallback: try to get a "content" field from "choices" array
            if (jsonResponse.Contains("\"content\""))
            {
                int contentIndex = jsonResponse.IndexOf("\"content\"");
                if (contentIndex >= 0)
                {
                    contentIndex = jsonResponse.IndexOf(":", contentIndex) + 1;
                    // Skip whitespace
                    while (contentIndex < jsonResponse.Length && char.IsWhiteSpace(jsonResponse[contentIndex]))
                    {
                        contentIndex++;
                    }
                    
                    // Check if content is enclosed in quotes
                    bool hasQuotes = contentIndex < jsonResponse.Length && jsonResponse[contentIndex] == '"';
                    int endContentIndex;
                    
                    if (hasQuotes)
                    {
                        contentIndex++; // Skip the opening quote
                        endContentIndex = jsonResponse.IndexOf("\"", contentIndex);
                    }
                    else
                    {
                        // Find the end of the value (comma or closing brace)
                        endContentIndex = jsonResponse.IndexOfAny(new[] { ',', '}' }, contentIndex);
                    }
                    
                    if (endContentIndex > contentIndex)
                    {
                        return jsonResponse.Substring(contentIndex, endContentIndex - contentIndex);
                    }
                }
            }
            
            // If all parsing attempts fail, return the raw response
            return jsonResponse;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error parsing LLM response: {e.Message}");
            return jsonResponse; // Return the original response if parsing fails
        }
    }
    
    /// <summary>
    /// Find the nearest NPC and make it speak the given message
    /// </summary>
    private void MakeNearestNPCSpeak(string message)
    {
        Debug.Log($"Making nearest NPC speak: {message}");
        
        // Find all AIConversationController components instead of using tags
        AIConversationController[] aiControllers = FindObjectsOfType<AIConversationController>();
        
        if (aiControllers == null || aiControllers.Length == 0)
        {
            Debug.LogWarning("No AIConversationController found in scene. Falling back to DialogueBoxController...");
            
            // Fallback to DialogueBoxController if no AIConversationController is found
            DialogueBoxController dialogueBoxController = FindObjectOfType<DialogueBoxController>();
            if (dialogueBoxController != null)
            {
                // Create a method to speak through DialogueBoxController
                StartCoroutine(SpeakThroughDialogueController(dialogueBoxController, message));
                Debug.Log("Used DialogueBoxController fallback to display idle message");
            }
            else
            {
                Debug.LogError("No DialogueBoxController found either. Cannot display idle message.");
            }
            
            return;
        }
        
        // Find nearest NPC to camera/player
        AIConversationController nearestController = null;
        float nearestDistance = float.MaxValue;
        Vector3 playerPosition = Camera.main.transform.position;
        
        foreach (AIConversationController controller in aiControllers)
        {
            if (controller.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(playerPosition, controller.transform.position);
                if (distance < nearestDistance)
                {
                    nearestController = controller;
                    nearestDistance = distance;
                }
            }
        }
        
        if (nearestController != null)
        {
            Debug.Log($"Found nearest NPC with AIConversationController: {nearestController.gameObject.name} at distance {nearestDistance}m");
            
            // Get the DialogueBoxController that's on the same GameObject
            DialogueBoxController dialogueBoxController = nearestController.GetComponent<DialogueBoxController>();
            if (dialogueBoxController != null)
            {
                // Stop thinking animation if it's active
                dialogueBoxController.stopThinking();
                
                // Method 1: Use SpeakLine private method through reflection
                StartCoroutine(SpeakThroughDialogueController(dialogueBoxController, message));
                
                // Add the message to the NPC's conversation context
                nearestController.AddMessage(new Message { role = "assistant", content = message });
                
                Debug.Log($"Successfully triggered idle speech through {nearestController.gameObject.name}");
            }
            else
            {
                Debug.LogError($"DialogueBoxController not found on {nearestController.gameObject.name}");
            }
        }
        else
        {
            Debug.LogWarning("No active AIConversationController found in scene");
        }
    }
    
    /// <summary>
    /// Helper method to speak a message through a DialogueBoxController
    /// Works by simulating what happens during normal NPC dialogue
    /// </summary>
    private IEnumerator SpeakThroughDialogueController(DialogueBoxController dialogueController, string message)
    {
        // First, ensure dialogueBox is active
        if (dialogueController._dialogueText != null)
        {
            // Emulate how normal dialogue speaking works
            
            // 1. Set the text in the UI
            dialogueController._dialogueText.text = message;
            
            // 2. Make the dialogue box visible
            System.Reflection.FieldInfo dialogueBoxField = typeof(DialogueBoxController).GetField("_dialogueBox", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (dialogueBoxField != null)
            {
                GameObject dialogueBox = (GameObject)dialogueBoxField.GetValue(dialogueController);
                if (dialogueBox != null) dialogueBox.SetActive(true);
            }
            
            System.Reflection.FieldInfo dialogueCanvasField = typeof(DialogueBoxController).GetField("_dialogueCanvas", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (dialogueCanvasField != null)
            {
                GameObject dialogueCanvas = (GameObject)dialogueCanvasField.GetValue(dialogueController);
                if (dialogueCanvas != null) dialogueCanvas.SetActive(true);
            }
            
            // 3. Animation handling - set talking animation
            Animator animator = dialogueController.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.SetBool("isTalking", true);
            }
            
            // 4. Use TTSSpeaker directly to speak the message
            bool speechTriggered = false;
            
            // Try AI-specific speaking first
            if (dialogueController._AIResponseToSpeech != null)
            {
                if (dialogueController.useWitAI)
                {
                    dialogueController.StartCoroutine(dialogueController._AIResponseToSpeech.WitAIDictate(message));
                }
                else
                {
                    dialogueController.StartCoroutine(dialogueController._AIResponseToSpeech.OpenAIDictate(message));
                }
                speechTriggered = true;
                Debug.Log($"Speaking through AIResponseToSpeech with {(dialogueController.useWitAI ? "WitAI" : "OpenAI")} TTS");
            }
            // Fallback to standard TTSSpeaker
            else if (dialogueController.TTSSpeaker != null)
            {
                var ttsSpeaker = dialogueController.TTSSpeaker.GetComponentInChildren<Meta.WitAi.TTS.Utilities.TTSSpeaker>();
                if (ttsSpeaker != null)
                {
                    ttsSpeaker.Speak(message);
                    speechTriggered = true;
                    Debug.Log("Speaking through standard TTSSpeaker");
                }
            }
            
            if (!speechTriggered)
            {
                Debug.LogError("Failed to trigger TTS speech - no working speech component found");
            }
            
            // Wait for a reasonable time for the speech to finish
            float estimatedDuration = Mathf.Max(3.0f, message.Length * 0.05f); // ~50ms per character, min 3 seconds
            yield return new WaitForSeconds(estimatedDuration);
            
            // Reset talking animation
            if (animator != null)
            {
                animator.SetBool("isTalking", false);
            }
        }
        else
        {
            Debug.LogError("DialogueController has no _dialogueText component");
        }
    }

    /// <summary>
    /// Sets the question in the upload data.
    /// </summary>
    /// <param name="question"></param>
    public void SetQuestion(string question)
    {
        Debug.Log($"Question set: {question}");
        
        // Create user message
        Message userMessage = new Message
        {
            role = "user",
            content = question
        };
        
        // Add to chat logs
        AddChatMessage(userMessage);
    }

    public void AddChatMessage(Message message)
    {
        globalChatLogs.Add(message);
    }

    public List<Message> GetGlobalChatLogs()
    {
        return globalChatLogs;
    }

    public UploadDataDTO GetUploadData()
    {
        return uploadData;
    }
}
