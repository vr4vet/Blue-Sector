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
    public List<SubtaskProgressDTO> subtaskProgress;
}

[Serializable]
public class ProgressDataCollection
{
    public List<ProgressDataDTO> items;
}

[Serializable]
public class UploadDataDTO
{
    public string user_name;
    public string user_mode;    // the "mode" defined in the user profiling
    public List<string> user_actions;   // this doesn't do anything yet
    public List<ProgressDataDTO> progress;     // list of json strings representing task progress
    public string question;     // question that user is asking
    public int NPC;     // ID of NPC that user is interacting with
}

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    private UploadDataDTO uploadData;
    private List<Task.Task> taskList;

    private void Awake()
    {
        // Ensure only one instance of ActionManager exists
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        Debug.Log("ActionManager initialized.");

        uploadData = new UploadDataDTO { user_name = "Ben",
                                         user_mode = "numberOneBrainrotter",
                                         user_actions = new List<string> { "vibeCoded", "askedChat", "lookmaxed" },
                                         question = "HELP ME! I AM TRAPPED INSIDE THIS ACURSED VR WORLD! I'M GOING CRAZY",
                                         NPC = 0 };

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
        uploadData.progress = progressHierarchy;
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
                        UpdateProgressData(progressData);

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

        StartCoroutine(SendUploadData(uploadData));

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