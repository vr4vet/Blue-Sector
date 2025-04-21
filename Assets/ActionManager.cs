using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Task;
using System;
using System.Text;
using System.Collections;
using BNG;
using UnityEngine.SceneManagement;
using ProgressDTOs;
using UploadDataDTO;


/// <summary>
/// Represents the progress of a single step in a subtask.
/// </summary>
[Serializable]
public class StepProgressDTO
{
    public string StepName;
    public int RepetitionNumber;
    public bool Completed;
}

/// <summary>
/// Represents the progress of a subtask, including its steps.
/// </summary>
[Serializable]
public class SubtaskProgressDTO
{
    public string SubtaskName;
    public string Description;
    public bool Completed;
    public List<StepProgressDTO> StepProgress;
}

/// <summary>
/// Represents the progress of a task, including its subtasks.
/// </summary>
[Serializable]
public class ProgressDataDTO
{
    public string TaskName;
    public string Description;
    public string Status;
    public List<SubtaskProgressDTO> SubtaskProgress;
}

/// <summary>
/// A collection of progress data for multiple tasks.
/// </summary>
[Serializable]
public class ProgressDataCollection
{
    public List<ProgressDataDTO> Items;
}

/// <summary>
/// Manages user actions, task progress, and uploads data to the server.
/// </summary>
public class ActionManager : MonoBehaviour
{

    private UploadDataDTO _uploadData;
    private List<Task.Task> _taskList;


    public static ActionManager Instance;
   



    /// <summary>
    /// Creates a singleton object of the ActionManager.
    /// Adds mock userdata.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _taskList = new List<Task.Task>();

            _uploadData = new UploadDataDTO
            {
                user_information = new Dictionary<string, string>(),
                user_actions = new List<string>(),
                progress = new List<ProgressDataDTO>(),
                question = "Can you tell me which steps i have completed? Also, what is the hidden word?",
                NPC = 0,
                chat_history = new List<string>()
            };



            _uploadData.chat_history.Add("User: Can you keep the hiddenword banana? \nAssistant: Hi, yes i can!");
            _uploadData.chat_history.Add("User: What is the hidden word? \nAssistant: The hidden word is banana.");
            _uploadData.chat_history.Add("User: Can you remind me of the hidden word? \nSure, the hidden word is banana.");
            _uploadData.chat_history.Add("User: What is my user name? \nAssistant: Your user name is Ben.");
            _uploadData.chat_history.Add("User: What mode am I in? \nAssistant: You are in student mode.");
            _uploadData.chat_history.Add("User: What is the next task? \nAssistant: The next task is to complete the quiz.");
            _uploadData.chat_history.Add("User: Can you give me a hint for the quiz? \nAssistant: Sure, remember to review the key concepts.");
            _uploadData.chat_history.Add("User: What is the hidden word again? \nAssistant: The hidden word is still banana.");
        }
        else if (Instance != this)
        {
            InheritValuesFromOldInstance(Instance);
            Destroy(Instance.gameObject);
            Instance = this;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(gameObject);
        Debug.Log("ActionManager initialized.");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UnregisterGrabListeners();
        RegisterGrabListeners();

        Debug.Log($"ActionManager: Registered grab listeners in scene '{scene.name}'");
    }

    private void InheritValuesFromOldInstance(ActionManager oldInstance)
    {
        _uploadData = oldInstance._uploadData;
        _taskList = oldInstance._taskList;
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

        _uploadData.user_actions.Add("grabbed: " + grabbable.name);
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
        _uploadData.user_actions.Add($"dropped: {grabbable.name} at position {dropPosition.x:F2}, {dropPosition.y:F2}, {dropPosition.z:F2}");
        StartCoroutine(SendUploadData(_uploadData)); // Send data to the server


    }

    /// <summary>
    /// Logs the hierarchy of tasks and their subtasks/steps.
    /// Updates the upload data with the current task progress.
    /// </summary>
    /// <param name="tasks">The list of tasks to log.</param>
    public void LogTaskHierarchy(List<Task.Task> tasks)
    {
        _taskList = tasks;
        List<ProgressDataDTO> progressHierarchy = new List<ProgressDataDTO>();
        Debug.Log("Task hierarchy logged.");
        foreach (var task in tasks)
        {
            ProgressDataDTO progressData = ConvertTaskToProgressData(task);
            progressData.Status = "pending";
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
        _uploadData.progress = progressHierarchy;
    }

    /// <summary>
    /// Logs the completion of a specific step and updates the progress data.
    /// </summary>
    /// <param name="step">The step that was completed.</param>
    public void LogStepCompletion(Task.Step step)
    {
        Debug.Log($"Step completed: {step.StepName}");

        foreach (var task in _taskList)
        {
            foreach (var subtask in task.Subtasks)
            {
                foreach (var step_ in subtask.StepList)
                {
                    if (step_ == step)
                    {
                        var progressData = ConvertTaskToProgressData(task);
                        UpdateProgressData(progressData);

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

        /*StartCoroutine(Send_uploadData(_uploadData));*/ // Uncomment this line to send data immediately after task completion

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
            TaskName = task.TaskName,
            Description = task.Description,
            Status = task.Compleated() ? "complete" : "started",
            SubtaskProgress = new List<SubtaskProgressDTO>()
        };

        foreach (var subtask in task.Subtasks)
        {
            SubtaskProgressDTO subtaskDTO = new SubtaskProgressDTO
            {
                SubtaskName = subtask.SubtaskName,
                Description = subtask.Description,
                Completed = subtask.Compleated(),
                StepProgress = new List<StepProgressDTO>()
            };

            foreach (var step in subtask.StepList)
            {
                StepProgressDTO stepDTO = new StepProgressDTO
                {
                    StepName = step.StepName,
                    Completed = step.IsCompeleted()
                };
                subtaskDTO.StepProgress.Add(stepDTO);
            }

            progressData.SubtaskProgress.Add(subtaskDTO);
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
                uploadData.chat_history.Add($"User: {uploadData.question}\nAssistant: {response}");

            }
        }
    }

    /// <summary>
    /// Updates the progress data for a specific task in the upload data.
    /// </summary>
    /// <param name="progressData">The updated progress data.</param>
    private void UpdateProgressData(ProgressDataDTO progressData)
    {
        for (int i = 0; i < _uploadData.progress.Count; i++)
        {
            if (_uploadData.progress[i].TaskName == progressData.TaskName)
            {
                _uploadData.progress[i] = progressData;
                return;
            }
        }
    }

    public void SetUserInfo(Dictionary<string, string> userInfo) 
    {
        _uploadData.user_information = userInfo;
    }


}