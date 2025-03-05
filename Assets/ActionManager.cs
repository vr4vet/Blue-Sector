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
    
    // Find and update the subtask in the task list
    foreach (var task in taskList)
    {
        foreach (var subtask in task.Subtasks)
        {
            if (subtask.SubtaskName == subtaskName)
            {
                subtask.IsCompleted = true;
                subtask.Description = description;
                Debug.Log($"Updated subtask completion status for {subtaskName}");
                
                // Serialize the updated task list
                string allTasksJson = SerializeTaskListToJson();
                Debug.Log($"Serialized task list after subtask completion: {allTasksJson}");
                
                return;
            }
        }
    }
    
    Debug.LogWarning($"Could not find subtask {subtaskName} in task list");
}

public void LogStepCompletion(string stepName, int repetitionNumber)
{
    Debug.Log($"Step completed: {stepName} - Repetition: {repetitionNumber}");
    
    // Find and update the step in the task list
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
                    Debug.Log($"Updated step completion status for {stepName}");
                    
                    // Check if all steps in this subtask are completed
                    bool allStepsCompleted = true;
                    foreach (var s in subtask.StepList)
                    {
                        if (!s.IsCompleted)
                        {
                            allStepsCompleted = false;
                            break;
                        }
                    }
                    
                    if (allStepsCompleted)
                    {
                        Debug.Log($"All steps in subtask {subtask.SubtaskName} completed");
                    }
                    
                    // Serialize the updated task list
                    string allTasksJson = SerializeTaskListToJson();
                    Debug.Log($"Serialized task list after step completion: {allTasksJson}");
                    
                    return;
                }
            }
        }
    }
    
    Debug.LogWarning($"Could not find step {stepName} in task list");
}

public void LogTaskCompletion(string taskName, string description)
{
    Debug.Log($"Task completed: {taskName} - {description}");
    
    // Find and update the task in the task list
    foreach (var task in taskList)
    {
        if (task.TaskName == taskName)
        {
            task.IsCompleted = true;
            task.Description = description;
            Debug.Log($"Updated task completion status for {taskName}");
            
            // Serialize the updated task list
            string allTasksJson = SerializeTaskListToJson();
            Debug.Log($"Serialized task list after task completion: {allTasksJson}");
            
            return;
        }
    }
    
    Debug.LogWarning($"Could not find task {taskName} in task list");
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
}