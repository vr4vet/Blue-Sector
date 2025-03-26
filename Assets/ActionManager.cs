using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Task;
using System;
using System.Text;
using System.Collections;

[Serializable]
public class StepProgressDTO
{
    public string stepName;
    public int repetitionNumber;
    public bool completed;
}

[Serializable]
public class SubtaskProgressDTO
{
    public string subtaskName;
    public string description;
    public bool completed;
    public List<StepProgressDTO> stepProgress;
}

[Serializable]
public class ProgressDataDTO
{
    public string taskName;
    public string description;
    public string status; // "started" or "complete"
    public string userId = null; // Setting default to null
    public List<SubtaskProgressDTO> subtaskProgress;
}

[Serializable]
public class ProgressDataCollection
{
    public List<ProgressDataDTO> items;
}

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;
    private List<Task.Task> taskList;

    private void Awake()
    {
        // Ensure only one instance of ActionManager exists
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        Debug.Log("ActionManager initialized.");

        // Initialize the task list
        taskList = new List<Task.Task>();
    }

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
        StartCoroutine(SendTaskHierarchy(progressHierarchy));
    }

    public void LogSubtaskCompletion(Task.Subtask subtask)
    {
        Debug.Log($"Subtask completed: {subtask.SubtaskName} - {subtask.Description}");
    
        foreach (var task in taskList)
        {
            foreach (var subtask_ in task.Subtasks)
            {
                if (subtask_ == subtask)
                {
                    // Send progress for the PARENT TASK
                    var progressData = ConvertTaskToProgressData(task);
                    StartCoroutine(SendProgressData(progressData));
                
                    return;
                }
            }
        }
    
        Debug.LogWarning($"Could not find subtask {subtask.SubtaskName}");
    }

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
                        // Send progress for the PARENT TASK
                        var progressData = ConvertTaskToProgressData(task);
                        StartCoroutine(SendProgressData(progressData));
                    
                        return;
                    }
                }
            }
        }
    
        Debug.LogWarning($"Could not find step {step.StepName}");
    }

    public void LogTaskCompletion(Task.Task task)
    {
        Debug.Log($"Task completed: {task.TaskName} - {task.Description}");

            var progressData = ConvertTaskToProgressData(task);
            StartCoroutine(SendProgressData(progressData));

            Debug.LogWarning($"Could not find task {task.TaskName}");
    }

    // New method to convert a Task to ProgressDataDTO
    private ProgressDataDTO ConvertTaskToProgressData(Task.Task task)
    {
        ProgressDataDTO progressData = new ProgressDataDTO
        {
            taskName = task.TaskName,
            description = task.Description,
            status = task.Compleated() ? "complete" : "started",
            // userId is now null by default
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

    private IEnumerator SendTaskHierarchy(List<ProgressDataDTO> progressHierarchy)
    {
        ProgressDataCollection collection = new ProgressDataCollection { items = progressHierarchy };

        string json = JsonUtility.ToJson(collection);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest("http://localhost:8080/api/progress/initializeTasks", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to send task hierarchy: {request.error}");
            }
            else
            {
                Debug.Log($"Server response: {request.downloadHandler.text}");
            }
        }
    }


    private IEnumerator SendProgressData(ProgressDataDTO progressData)
    {
        string json = JsonUtility.ToJson(progressData);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest("http://localhost:8080/api/progress/updateTask", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to send progress: {request.error}");
            }
            else
            {
                Debug.Log($"Server response: {request.downloadHandler.text}");
            }
        }
    }
}