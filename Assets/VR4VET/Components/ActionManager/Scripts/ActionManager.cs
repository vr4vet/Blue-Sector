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
    
    // Reference to the Chatbot prefab
    [SerializeField]
    [Tooltip("The Chatbot prefab that will be spawned when the player is idle and no NPCs are nearby")]
    private GameObject chatbotPrefab;
    
    // Maximum distance to check for nearby NPCs
    [SerializeField]
    [Tooltip("Maximum distance in meters to check for nearby NPCs")]
    private float maxNpcDetectionDistance = 4f;
    
    // Reference to the currently spawned chatbot instance
    private GameObject currentChatbotInstance;

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
    /// If no NPC is within range, spawn the Chatbot prefab
    /// </summary>
    private void MakeNearestNPCSpeak(string message)
    {
        Debug.Log($"Making nearest NPC speak: {message}");
        
        // Get the player's position
        Vector3 playerPosition = Camera.main.transform.position;
        
        // Find all AIConversationController components
        AIConversationController[] aiControllers = FindObjectsOfType<AIConversationController>();
        
        // Find nearest NPC to camera/player
        AIConversationController nearestController = null;
        float nearestDistance = float.MaxValue;
        
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
        
        // Check if we found a nearby NPC within the specified range
        if (nearestController != null && nearestDistance <= maxNpcDetectionDistance)
        {
            Debug.Log($"Found nearby NPC: {nearestController.gameObject.name} at distance {nearestDistance}m");
            
            // Use the existing NPC to speak the message
            DialogueBoxController dialogueBoxController = nearestController.GetComponent<DialogueBoxController>();
            if (dialogueBoxController != null)
            {
                dialogueBoxController.stopThinking();
                StartCoroutine(SpeakThroughDialogueController(dialogueBoxController, message));
                nearestController.AddMessage(new Message { role = "assistant", content = message });
                Debug.Log($"Using existing NPC at {nearestDistance}m distance to deliver idle message");
            }
            else
            {
                Debug.LogError($"DialogueBoxController not found on {nearestController.gameObject.name}");
                SpawnChatbotAndSpeak(message, playerPosition);
            }
        }
        else
        {
            // No NPC within range - spawn Chatbot
            if (nearestController != null)
            {
                Debug.Log($"Nearest NPC is too far away ({nearestDistance}m > {maxNpcDetectionDistance}m). Spawning Chatbot instead.");
            }
            else
            {
                Debug.Log("No active NPCs found in scene. Spawning Chatbot instead.");
            }
            
            SpawnChatbotAndSpeak(message, playerPosition);
        }
    }
    
    /// <summary>
    /// Spawns the Chatbot prefab at an appropriate position near the player and makes it speak
    /// </summary>
    /// <param name="message">The message for the Chatbot to speak</param>
    /// <param name="playerPosition">The position of the player</param>
    private void SpawnChatbotAndSpeak(string message, Vector3 playerPosition)
    {
        if (chatbotPrefab == null)
        {
            Debug.LogError("Chatbot prefab is not assigned in the ActionManager!");
            return;
        }

        // If we already have a Chatbot instance, use it
        if (currentChatbotInstance != null && currentChatbotInstance.activeInHierarchy)
        {
            Debug.Log("Using existing Chatbot instance");
            
            // Position the existing Chatbot
            PositionChatbotNearPlayer(currentChatbotInstance, playerPosition);
        }
        else
        {
            // Clean up any previous instance
            if (currentChatbotInstance != null)
            {
                Destroy(currentChatbotInstance);
            }

            // Spawn a new Chatbot prefab in front of the player
            currentChatbotInstance = Instantiate(chatbotPrefab);
            Debug.Log("Spawned new Chatbot instance");
            
            // Position the Chatbot in front of the player
            PositionChatbotNearPlayer(currentChatbotInstance, playerPosition);
        }

        // Make the Chatbot speak
        DialogueBoxController chatbotDialog = currentChatbotInstance.GetComponent<DialogueBoxController>();
        if (chatbotDialog != null)
        {
            StartCoroutine(SpeakThroughDialogueController(chatbotDialog, message));
            
            // If it has an AIConversationController, add the message to its conversation context
            AIConversationController aiController = currentChatbotInstance.GetComponent<AIConversationController>();
            if (aiController != null)
            {
                aiController.AddMessage(new Message { role = "assistant", content = message });
            }
        }
        else
        {
            Debug.LogError("Chatbot prefab does not have a DialogueBoxController component!");
        }
    }
    
    /// <summary>
    /// Positions the Chatbot in front of the player at an appropriate distance and height
    /// Ensures the chatbot is visible regardless of terrain constraints
    /// </summary>
    private void PositionChatbotNearPlayer(GameObject chatbot, Vector3 playerPosition)
    {
        if (chatbot == null || Camera.main == null) return;
        
        // Get reference to player camera rig to position relative to it
        GameObject playerRig = GameObject.FindWithTag("Player");
        if (playerRig == null)
        {
            // Fallback to main camera if player rig not found
            playerRig = Camera.main.gameObject;
        }
        
        // Get player's head position and forward direction
        Vector3 playerEyePosition = Camera.main.transform.position;
        Vector3 playerForward = Camera.main.transform.forward;
        playerForward.y = 0; // Flatten to horizontal plane
        playerForward.Normalize();
        
        // Position the Chatbot 1.5 meters in front of the player and slightly to the side
        float chatbotDistance = 1.5f;
        float sideOffset = 0.3f; // Slight offset to the side so it's not directly in the way
        
        Vector3 spawnPosition = playerEyePosition + (playerForward * chatbotDistance);
        spawnPosition += Camera.main.transform.right * sideOffset; // Offset to the right side
        
        // Make sure the Chatbot is at a proper height for visibility
        float eyeHeight = playerEyePosition.y;
        float chatbotHeight = eyeHeight - 0.3f; // Place slightly below eye level
        spawnPosition.y = chatbotHeight;
        
        Debug.Log($"Attempting to position Chatbot at {spawnPosition}");
        
        // Check if this position would be valid (not inside terrain)
        if (Physics.CheckSphere(spawnPosition, 0.2f, LayerMask.GetMask("Terrain", "Default")))
        {
            // Position is inside terrain/collider - try alternative positioning
            Debug.Log("Initial position inside terrain/collider, trying alternative position");
            
            // Try positions at different offsets until we find a clear spot
            float[] distanceOptions = { 1.2f, 1.8f, 2.5f, 3.0f };
            float[] heightOptions = { -0.1f, -0.5f, 0f, 0.5f };
            bool foundValidPosition = false;
            
            foreach (float distance in distanceOptions)
            {
                foreach (float heightOffset in heightOptions)
                {
                    Vector3 testPosition = playerEyePosition + (playerForward * distance);
                    testPosition += Camera.main.transform.right * sideOffset;
                    testPosition.y = eyeHeight + heightOffset;
                    
                    if (!Physics.CheckSphere(testPosition, 0.2f, LayerMask.GetMask("Terrain", "Default")))
                    {
                        spawnPosition = testPosition;
                        foundValidPosition = true;
                        Debug.Log($"Found valid position at distance {distance}m, height offset {heightOffset}m");
                        break;
                    }
                }
                if (foundValidPosition) break;
            }
            
            // If we still can't find a good position, force teleport right in front of player
            if (!foundValidPosition)
            {
                Debug.LogWarning("Could not find valid position - forcing teleport directly in front of player");
                spawnPosition = playerEyePosition + (playerForward * 1.0f);
                spawnPosition.y = playerEyePosition.y;
            }
        }
        
        // Check if the chatbot has a CharacterController and disable it temporarily to prevent position conflicts
        CharacterController characterController = chatbot.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
        }
        
        // Force navmeshagent to teleport if present
        UnityEngine.AI.NavMeshAgent navAgent = chatbot.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.enabled = false;
            chatbot.transform.position = spawnPosition;
            StartCoroutine(ReenableNavAgent(navAgent, 0.2f));
        }
        else
        {
            // Direct position setting
            chatbot.transform.position = spawnPosition;
        }
        
        // Reset physics state and re-enable character controller after positioning
        Rigidbody rb = chatbot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = spawnPosition;
        }
        
        if (characterController != null)
        {
            StartCoroutine(ReenableCharacterController(characterController, spawnPosition, 0.1f));
        }
        
        // Make Chatbot look at player's horizontal position (don't tilt up/down)
        Vector3 lookTarget = new Vector3(playerEyePosition.x, chatbot.transform.position.y, playerEyePosition.z);
        chatbot.transform.LookAt(lookTarget);
        
        Debug.Log($"Successfully positioned Chatbot at {chatbot.transform.position}, facing player");
    }
    
    /// <summary>
    /// Re-enables a NavMeshAgent after a short delay
    /// </summary>
    private IEnumerator ReenableNavAgent(UnityEngine.AI.NavMeshAgent agent, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (agent != null)
        {
            // Save the current position before re-enabling the agent
            Vector3 currentPosition = agent.transform.position;
            
            // Temporarily make the agent not auto-update its position
            bool originalUpdatePosition = agent.updatePosition;
            agent.updatePosition = false;
            
            // Re-enable the agent
            agent.enabled = true;
            
            // For flying fish, we need to handle differently - check if we're on a NavMesh
            if (!agent.isOnNavMesh)
            {
                // Option 1: Keep the agent disabled if we're not on NavMesh
                // This works for flying fish that don't need navigation
                Debug.LogWarning("NavMeshAgent enabled but not on NavMesh. Disabling NavMeshAgent for flying fish.");
                agent.enabled = false;
                
                // Check if this is a fish that should fly
                SimpleFishController fishController = agent.GetComponent<SimpleFishController>();
                if (fishController != null)
                {
                    // This is a fish - make sure it can fly and follows player
                    fishController.SetFlyingEnabled(true);
                    fishController.SetFollowPlayerEnabled(true);
                    Debug.Log("Fish controller detected - enabled flying mode");
                }
            }
            else
            {
                // We're on a valid NavMesh - restore the position explicitly
                agent.Warp(currentPosition);
                Debug.Log("Re-enabled NavMeshAgent after positioning");
            }
            
            // Restore the agent's settings
            agent.updatePosition = originalUpdatePosition;
        }
    }
    
    /// <summary>
    /// Re-enables a CharacterController after positioning
    /// </summary>
    private IEnumerator ReenableCharacterController(CharacterController controller, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (controller != null)
        {
            controller.enabled = true;
            Debug.Log("Re-enabled CharacterController after positioning");
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
            // First check for fish controller (for fish NPCs)
            SimpleFishController fishController = dialogueController.GetComponentInChildren<SimpleFishController>();
            if (fishController != null)
            {
                fishController.SetTalking(true);
                Debug.Log("Set fish talking animation via SimpleFishController");
            }
            // Next try FishAnimatorController which safely handles missing parameters
            else
            {
                FishAnimatorController fishAnimController = dialogueController.GetComponentInChildren<FishAnimatorController>();
                if (fishAnimController != null)
                {
                    fishAnimController.OnTalkAnimationStarted();
                    Debug.Log("Set fish talking animation via FishAnimatorController");
                }
                // Last resort - try regular Animator with parameter safety check
                else
                {
                    Animator animator = dialogueController.GetComponentInChildren<Animator>();
                    if (animator != null)
                    {
                        try
                        {
                            // Check if parameter exists before setting it
                            if (HasAnimatorParameter(animator, "isTalking"))
                            {
                                animator.SetBool("isTalking", true);
                                Debug.Log("Set isTalking animation parameter");
                            }
                            else if (HasAnimatorParameter(animator, "Talk"))
                            {
                                animator.SetTrigger("Talk");
                                Debug.Log("Set Talk animation trigger");
                            }
                            else
                            {
                                Debug.LogWarning("Could not find animation parameter for talking (isTalking or Talk)");
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogWarning($"Could not set Animator parameter: {ex.Message}");
                        }
                    }
                }
            }
            
            // 4. Use TTSSpeaker directly to speak the message
            bool speechTriggered = false;
            
            // Try AI-specific speaking first
            if (dialogueController._AIResponseToSpeech != null)
            {
                try
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
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error using AIResponseToSpeech: {ex.Message}");
                }
            }
            // Fallback to standard TTSSpeaker
            if (!speechTriggered && dialogueController.TTSSpeaker != null)
            {
                var ttsSpeaker = dialogueController.TTSSpeaker.GetComponent<Meta.WitAi.TTS.Utilities.TTSSpeaker>();
                if (ttsSpeaker == null)
                {
                    ttsSpeaker = dialogueController.TTSSpeaker.GetComponentInChildren<Meta.WitAi.TTS.Utilities.TTSSpeaker>();
                }
                
                if (ttsSpeaker != null)
                {
                    try
                    {
                        ttsSpeaker.Speak(message);
                        speechTriggered = true;
                        Debug.Log("Speaking through standard TTSSpeaker");
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"Error using TTSSpeaker: {ex.Message}");
                    }
                }
            }
            
            // Deeper search for TTSSpeaker on the GameObject and its parents
            if (!speechTriggered)
            {
                // First try on this game object
                var ttsSpeaker = dialogueController.gameObject.GetComponentInChildren<Meta.WitAi.TTS.Utilities.TTSSpeaker>();
                if (ttsSpeaker != null)
                {
                    try
                    {
                        ttsSpeaker.Speak(message);
                        speechTriggered = true;
                        Debug.Log("Speaking through found TTSSpeaker component");
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"Error using found TTSSpeaker: {ex.Message}");
                    }
                }
                
                // Try parent objects if needed
                if (!speechTriggered)
                {
                    ttsSpeaker = dialogueController.gameObject.GetComponentInParent<Meta.WitAi.TTS.Utilities.TTSSpeaker>();
                    if (ttsSpeaker != null)
                    {
                        try
                        {
                            ttsSpeaker.Speak(message);
                            speechTriggered = true;
                            Debug.Log("Speaking through parent TTSSpeaker component");
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError($"Error using parent TTSSpeaker: {ex.Message}");
                        }
                    }
                }
                
                // Last resort - create a temporary audio source as fallback
                if (!speechTriggered)
                {
                    Debug.LogError("Failed to trigger TTS speech - no working speech component found. Using fallback audio.");
                    AudioSource tempAudio = dialogueController.gameObject.AddComponent<AudioSource>();
                    tempAudio.PlayOneShot(AudioClip.Create("beep", 4410, 1, 44100, false));
                    Destroy(tempAudio, 2f); // Remove after playing
                }
            }
            
            // Wait for a reasonable time for the speech to finish
            float estimatedDuration = Mathf.Max(3.0f, message.Length * 0.05f); // ~50ms per character, min 3 seconds
            yield return new WaitForSeconds(estimatedDuration);
            
            // Reset talking animation using the same hierarchy of controllers we used to start it
            if (fishController != null)
            {
                fishController.SetTalking(false);
            }
            else
            {
                FishAnimatorController fishAnimController = dialogueController.GetComponentInChildren<FishAnimatorController>();
                if (fishAnimController != null)
                {
                    fishAnimController.OnTalkAnimationEnded();
                }
                else
                {
                    Animator animator = dialogueController.GetComponentInChildren<Animator>();
                    if (animator != null)
                    {
                        try
                        {
                            if (HasAnimatorParameter(animator, "isTalking"))
                            {
                                animator.SetBool("isTalking", false);
                            }
                        }
                        catch (System.Exception) { }
                    }
                }
            }

            // Notify the ChatbotController if present
            ChatbotController chatbotController = dialogueController.GetComponent<ChatbotController>();
            if (chatbotController != null)
            {
                chatbotController.OnSpeechFinished();
            }
        }
        else
        {
            Debug.LogError("DialogueController has no _dialogueText component");
        }
    }
    
    /// <summary>
    /// Helper method to check if an animator has a parameter
    /// </summary>
    private bool HasAnimatorParameter(Animator animator, string paramName)
    {
        if (animator == null) return false;
        
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
}
