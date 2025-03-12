using System.Collections.Generic;
using UnityEngine;
using Task;
using System;
using System.Text;

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
    public string status; // "start" or "complete"
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
        Debug.Log("Task hierarchy logged.");
        foreach (var task in tasks)
        {
            Debug.Log($"Task: {task.TaskName}");
            foreach (var subtask in task.Subtasks)
            {
                Debug.Log($"  Subtask: {subtask.SubtaskName}");
                foreach (var step in subtask.StepList)
                {
                    Debug.Log($"    Step: {step.StepName}");
                }
            }
        }
    }

public void LogSubtaskCompletion(string subtaskName, string description)
{
    Debug.Log($"Subtask completed: {subtaskName} - {description}");
    
    foreach (var task in taskList)
    {
        foreach (var subtask in task.Subtasks)
        {
            if (subtask.SubtaskName == subtaskName)
            {
                subtask.IsCompleted = true;
                subtask.Description = description;

                // Send progress for the PARENT TASK
                var progressData = ConvertTaskToProgressData(task);
                progressData.status = "start"; // Task is still in progress
                StartCoroutine(SendProgressData(progressData));
                
                return;
            }
        }
    }
    
    Debug.LogWarning($"Could not find subtask {subtaskName}");
}

public void LogStepCompletion(string stepName, int repetitionNumber)
{
    Debug.Log($"Step completed: {stepName} - Repetition: {repetitionNumber}");
    
    foreach (var task in taskList)
    {
        foreach (var subtask in task.Subtasks)
        {
            foreach (var step in subtask.StepList)
            {
                if (step.StepName == stepName)
                {
                    step.IsCompleted = true;
                    step.RepetitionNumber = repetitionNumber;

                    // Send progress for the PARENT TASK
                    var progressData = ConvertTaskToProgressData(task);
                    progressData.status = "start"; // Task is still in progress
                    StartCoroutine(SendProgressData(progressData));
                    
                    return;
                }
            }
        }
    }
    
    Debug.LogWarning($"Could not find step {stepName}");
}

public void LogTaskCompletion(string taskName, string description)
{
    Debug.Log($"Task completed: {taskName} - {description}");
    
    foreach (var task in taskList)
    {
        if (task.TaskName == taskName)
        {
            task.IsCompleted = true;
            task.Description = description;

            // Send final completion status
            var progressData = ConvertTaskToProgressData(task);
            progressData.status = "complete"; // Explicit completion
            StartCoroutine(SendProgressData(progressData));
            
            return;
        }
    }
    
    Debug.LogWarning($"Could not find task {taskName}");
}
public void LogTaskStart(string taskName)
{
    foreach (var task in taskList)
    {
        if (task.TaskName == taskName)
        {
            // Send initial "start" status
            var progressData = ConvertTaskToProgressData(task);
            progressData.status = "start";
            StartCoroutine(SendProgressData(progressData));
            
            return;
        }
    }
    
    Debug.LogWarning($"Could not find task {taskName}");
}

    // New method to convert a Task to ProgressDataDTO
    private ProgressDataDTO ConvertTaskToProgressData(Task.Task task)
    {
        ProgressDataDTO progressData = new ProgressDataDTO
        {
            taskName = task.TaskName,
            status = task.IsCompleted ? "complete" : "start",
            // userId is now null by default
            subtaskProgress = new List<SubtaskProgressDTO>()
        };

        foreach (var subtask in task.Subtasks)
        {
            SubtaskProgressDTO subtaskDTO = new SubtaskProgressDTO
            {
                subtaskName = subtask.SubtaskName,
                description = subtask.Description,
                completed = subtask.IsCompleted,
                stepProgress = new List<StepProgressDTO>()
            };

            foreach (var step in subtask.StepList)
            {
                StepProgressDTO stepDTO = new StepProgressDTO
                {
                    stepName = step.StepName,
                    repetitionNumber = step.RepetitionNumber,
                    completed = step.IsCompleted
                };
                subtaskDTO.stepProgress.Add(stepDTO);
            }

            progressData.subtaskProgress.Add(subtaskDTO);
        }

        return progressData;
    }

    // New method to serialize taskList to JSON
    public string SerializeTaskListToJson()
    {
        if (taskList == null || taskList.Count == 0)
        {
            Debug.LogWarning("Task list is empty, nothing to serialize");
            return "[]";
        }

        List<ProgressDataDTO> progressDataList = new List<ProgressDataDTO>();
        
        foreach (var task in taskList)
        {
            ProgressDataDTO progressData = ConvertTaskToProgressData(task);
            progressDataList.Add(progressData);
        }

        // We need a wrapper class since JsonUtility can't serialize lists directly
        ProgressDataCollection collection = new ProgressDataCollection
        {
            items = progressDataList
        };

        return JsonUtility.ToJson(collection);
    }
    private IEnumerator SendProgressData(ProgressDataDTO progressData)
    {
        string json = JsonUtility.ToJson(progressData);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest("http://localhost:8000/api/progress", "POST"))
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