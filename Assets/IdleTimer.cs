using System;
using UnityEngine;
using Task;

/// <summary>
/// Data structure for tracking user idle time on tasks.
/// </summary>
[Serializable]
public class IdleDataDTO
{
    /// <summary>
    /// Name of the task where the user has been idle.
    /// </summary>
    public string currentTaskName;

    /// <summary>
    /// The amount of time the user has been idle in seconds.
    /// </summary>
    public float idleTimeSeconds;

    /// <summary>
    /// The threshold after which the system considers the user may need help.
    /// </summary>
    public float idleThresholdSeconds;

    /// <summary>
    /// Last active step the user was working on before becoming idle.
    /// </summary>
    public string lastActiveStep;

    /// <summary>
    /// Timestamp when the user started the task.
    /// </summary>
    public string taskStartTime;

    /// <summary>
    /// Optional message for the backend LLM providing context for the idle state.
    /// </summary>
    public string contextMessage;
}

/// <summary>
/// Tracks user idle time and reports when the user has been idle for too long.
/// </summary>
public class IdleTimer : MonoBehaviour
{
    // Delegate for idle events
    public delegate void IdleEventHandler(IdleDataDTO idleData);

    // Event fired when the user has been idle for longer than the threshold
    public event IdleEventHandler OnIdleThresholdReached;

    private float idleTimer = 0f;
    private bool isTrackingIdleTime = false;
    private Task.Task currentActiveTask = null;
    private Task.Step currentActiveStep = null;

    // Default threshold (in seconds) for when to send an idle alert
    [SerializeField] private float idleThresholdInSeconds = 300f; // 5 minutes by default

    // How often to check if the user is still idle (in seconds)
    [SerializeField] private float idleCheckIntervalInSeconds = 60f; // 1 minute by default

    private float nextIdleCheckTime = 0f;

    /// <summary>
    /// Update is called once per frame and handles the idle timer tracking.
    /// </summary>
    private void Update()
    {
        if (isTrackingIdleTime && currentActiveTask != null)
        {
            // Increment idle timer
            idleTimer += Time.deltaTime;

            // Check if it's time for the next idle check
            if (Time.time >= nextIdleCheckTime)
            {
                // Check if we have exceeded the idle threshold
                if (idleTimer >= idleThresholdInSeconds)
                {
                    // Send idle report
                    SendIdleReport();

                    // Reset next check time (increase interval to prevent spamming)
                    nextIdleCheckTime = Time.time + idleCheckIntervalInSeconds;
                }
                else
                {
                    // Still under threshold, set next check time
                    nextIdleCheckTime = Time.time + idleCheckIntervalInSeconds;
                }
            }
        }
    }

    /// <summary>
    /// Starts tracking idle time for the specified task and step.
    /// </summary>
    /// <param name="task">The task to track idle time for.</param>
    /// <param name="step">The current step the user is working on.</param>
    public void StartIdleTracking(Task.Task task, Task.Step step)
    {
        currentActiveTask = task;
        currentActiveStep = step;
        isTrackingIdleTime = true;
        idleTimer = 0f;
        nextIdleCheckTime = Time.time + idleCheckIntervalInSeconds;

        Debug.Log($"Started idle tracking for task: {task.TaskName}, step: {step.StepName}");
    }

    /// <summary>
    /// Stops tracking idle time.
    /// </summary>
    public void StopIdleTracking()
    {
        isTrackingIdleTime = false;
        idleTimer = 0f;

        Debug.Log("Stopped idle tracking");
    }

    /// <summary>
    /// Resets the idle timer without stopping tracking.
    /// Call this when the user performs any meaningful action.
    /// </summary>
    public void ResetIdleTimer()
    {
        if (isTrackingIdleTime)
        {
            idleTimer = 0f;
            nextIdleCheckTime = Time.time + idleCheckIntervalInSeconds;
            Debug.Log("Idle timer reset due to user activity");
        }
    }

    /// <summary>
    /// Creates and sends a report about user idle status.
    /// </summary>
    private void SendIdleReport()
    {
        if (currentActiveTask == null)
            return;

        Debug.Log($"User has been idle for {idleTimer} seconds on task: {currentActiveTask.TaskName}");

        // Create idle data
        IdleDataDTO idleData = new IdleDataDTO
        {
            currentTaskName = currentActiveTask.TaskName,
            idleTimeSeconds = idleTimer,
            idleThresholdSeconds = idleThresholdInSeconds,
            lastActiveStep = currentActiveStep != null ? currentActiveStep.StepName : "Unknown",
            taskStartTime = DateTime.Now.AddSeconds(-idleTimer).ToString("yyyy-MM-dd HH:mm:ss"),
            contextMessage = $"User might need assistance with task '{currentActiveTask.TaskName}'. They have been idle for {Math.Round(idleTimer / 60, 1)} minutes."
        };

        // Fire the event
        OnIdleThresholdReached?.Invoke(idleData);
    }
}
