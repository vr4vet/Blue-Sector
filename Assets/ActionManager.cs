using System.Collections.Generic;
using UnityEngine;
using Task;

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
    }

    public void LogStepCompletion(string stepName, int repetitionNumber)
    {
        Debug.Log($"Step completed: {stepName} - Repetition: {repetitionNumber}");
    }

    public void LogTaskCompletion(string taskName, string description)
    {
        Debug.Log($"Task completed: {taskName} - {description}");
    }
}