/* Developer: Erik Le Blanc
 * Ask your questions on github: https://github.com/erikleblanc
 */

using System;
using UnityEngine;
using Task;

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

    // Default threshold (in seconds) if no step-specific timeout is provided
    [SerializeField] private float defaultIdleThresholdInSeconds = 120f; // Default 2 minutes

    // How often to check if the user is still idle (in seconds)
    [SerializeField] private float idleCheckIntervalInSeconds = 20f;

    // Current idle threshold for the active step
    private float idleThresholdInSeconds;

    private float nextIdleCheckTime = 0f;

    /// <summary>
    /// Update is called once per frame and handles the idle timer tracking.
    /// </summary>
    private void Update()
    {
        if (isTrackingIdleTime && currentActiveTask != null && currentActiveStep != null)
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

        // Get the idle threshold from the step
        float stepTimeout = step.GetIdleTimeoutSeconds();

        // If timeout is 0, don't track idle time for this step
        if (stepTimeout <= 0)
        {
            isTrackingIdleTime = false;
            idleTimer = 0f;
            Debug.Log($"Idle tracking disabled for task: {task.TaskName}, step: {step.StepName}");
            return;
        }

        // Otherwise, start tracking with the specified timeout
        isTrackingIdleTime = true;
        idleTimer = 0f;
        idleThresholdInSeconds = stepTimeout;
        nextIdleCheckTime = Time.time + idleCheckIntervalInSeconds;

        Debug.Log($"Started idle tracking for task: {task.TaskName}, step: {step.StepName}, timeout: {idleThresholdInSeconds}s");
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
    /// Handles the idle threshold exceeded event from IdleTimer
    /// </summary>
    /// <param name="idleData">Data about the idle state</param>
    private void HandleIdleThresholdReached(IdleDataDTO idleData)
    {
        // Update the question to reflect the idle state
        /*uploadData.question = $"I've been working on '{idleData.currentTaskName}' for a while and might need some help. I'm stuck on the step '{idleData.lastActiveStep}'.";

        // Set the idle data
        uploadData.idleData = idleData;

        // Send the report
        StartCoroutine(SendUploadData(uploadData));

        // Clear idle data after sending
        uploadData.idleData = null;*/
    }

    /// <summary>
    /// Creates and sends a report about user idle status.
    /// </summary>
    private void SendIdleReport()
    {
        if (currentActiveTask == null || currentActiveStep == null)
            return;

        Debug.Log($"User has been idle for {idleTimer} seconds on task: {currentActiveTask.TaskName}, step: {currentActiveStep.StepName}");
        Debug.Log($"Idle threshold reached with timeout of {idleThresholdInSeconds} seconds");

        // Create idle data
        IdleDataDTO idleData = new IdleDataDTO
        {
            currentTaskName = currentActiveTask.TaskName,
            idleTimeSeconds = idleTimer,
            idleThresholdSeconds = idleThresholdInSeconds,
            lastActiveStep = currentActiveStep.StepName,
            taskStartTime = DateTime.Now.AddSeconds(-idleTimer).ToString("yyyy-MM-dd HH:mm:ss"),
            contextMessage = $"User might need assistance with task '{currentActiveTask.TaskName}', step '{currentActiveStep.StepName}'. They have been idle for {Math.Round(idleTimer / 60, 1)} minutes."
        };

        // Fire the event
        OnIdleThresholdReached?.Invoke(idleData);
    }
}
