using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Task;
using System;
using System.Text;
using System.Collections;

/// <summary>
/// Represents the progress of a single step in a subtask.
/// </summary>
[Serializable]
public class StepProgressDTO
{
    /// <summary>
    /// The name of the step.
    /// </summary>
    public string stepName;

    /// <summary>
    /// The repetition number of the step.
    /// </summary>
    public int repetitionNumber;

    /// <summary>
    /// Indicates whether the step is completed.
    /// </summary>
    public bool completed;
}

/// <summary>
/// Represents the progress of a subtask, including its steps.
/// </summary>
[Serializable]
public class SubtaskProgressDTO
{
    /// <summary>
    /// The name of the subtask.
    /// </summary>
    public string subtaskName;

    /// <summary>
    /// The description of the subtask.
/// </summary>
    public string description;

    /// <summary>
    /// Indicates whether the subtask is completed.
    /// </summary>
    public bool completed;

    /// <summary>
    /// A list of progress data for the steps in the subtask.
    /// </summary>
    public List<StepProgressDTO> stepProgress;
}

/// <summary>
/// Represents the progress of a task, including its subtasks.
/// </summary>
[Serializable]
public class ProgressDataDTO
{
    /// <summary>
    /// The name of the task.
    /// </summary>
    public string taskName;

    /// <summary>
    /// The description of the task.
    /// </summary>
    public string description;

    /// <summary>
    /// The status of the task, either "started" or "complete".
    /// </summary>
    public string status;

    /// <summary>
    /// A list of progress data for the subtasks in the task.
    /// </summary>
    public List<SubtaskProgressDTO> subtaskProgress;
}

/// <summary>
/// A collection of progress data for multiple tasks.
/// </summary>
[Serializable]
public class ProgressDataCollection
{
    /// <summary>
    /// A list of progress data for tasks.
    /// </summary>
    public List<ProgressDataDTO> items;
}

/// <summary>
/// Data structure for uploading user progress and interactions.
/// </summary>
[Serializable]
public class UploadDataDTO
{
    /// <summary>
    /// The name of the user.
    /// </summary>
    public string user_name;

    /// <summary>
    /// The mode defined in the user profiling.
    /// </summary>
    public string user_mode;

    /// <summary>
    /// A list of user actions (currently unused).
    /// </summary>
    public List<string> user_actions;

    /// <summary>
    /// A list of progress data for tasks.
    /// </summary>
    public List<ProgressDataDTO> progress;

    /// <summary>
    /// The question that the user is asking.
    /// </summary>
    public string question;

    /// <summary>
    /// The ID of the NPC that the user is interacting with.
    /// </summary>
    public int NPC;
}

/// <summary>
/// Manages user actions, task progress, and uploads data to the server.
/// </summary>
public class ActionManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the ActionManager.
    /// </summary>
    public static ActionManager Instance;

    private UploadDataDTO uploadData;
    private List<Task.Task> taskList;

    /// <summary>
    /// Creates a singleton object of the ActionManager.
    /// Adds mock userdata.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        Debug.Log("ActionManager initialized.");

        uploadData = new UploadDataDTO
        {
            user_name = "Ben",
            user_mode = "numberOneBrainrotter",
            user_actions = new List<string> { "vibeCoded", "askedChat", "lookmaxed" },
            question = "HELP ME! I AM TRAPPED INSIDE THIS ACURSED VR WORLD! I'M GOING CRAZY",
            NPC = 0
        };

        taskList = new List<Task.Task>();
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
            progressData.status = "pending";
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

        StartCoroutine(SendUploadData(uploadData));

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
}