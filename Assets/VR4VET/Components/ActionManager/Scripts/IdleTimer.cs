/* Developer: Erik Le Blanc
 * Ask your questions on github: https://github.com/erikleblanc
 */

using System;
using UnityEngine;
using Task;

/// <summary>
/// Tracks user idle time and reports when the user has been idle for too long.
/// </summary
public class IdleTimer : MonoBehaviour
{
    private float idleTimer = 0f;
    private bool isTrackingIdleTime = false;
    private Task.Subtask lastProgressedSubtask;
    private Task.Step lastCompletedStep;
    private string timeoutPrompt;

    // Default threshold (in seconds)
    [SerializeField] private float defaultIdleThresholdInSeconds; // Default 2 minutes

    // How often to check if the user is still idle (in seconds)
    [SerializeField] private float idleCheckIntervalInSeconds;

    // Current idle threshold for the active subtask
    private float idleThresholdInSeconds;

    private float nextIdleCheckTime = 0f;

    private bool thresholdReached = false;

    /// <summary>
    /// Update is called once per frame and handles the idle timer tracking.
    /// </summary>
    private void Update()
    {
        if (isTrackingIdleTime && lastProgressedSubtask != null && lastCompletedStep != null)
        {
            // Increment idle timer
            idleTimer += Time.deltaTime;

            // First time threshold is reached
            if (!thresholdReached && idleTimer >= idleThresholdInSeconds)
            {
                // Send idle report immediately when threshold is first reached
                SendIdleReport();
                thresholdReached = true;
                nextIdleCheckTime = Time.time + idleCheckIntervalInSeconds;
            }
            // Subsequent interval checks after threshold was already reached
            else if (thresholdReached && Time.time >= nextIdleCheckTime)
            {
                // Send additional idle reports at the specified intervals
                SendIdleReport();
                nextIdleCheckTime = Time.time + idleCheckIntervalInSeconds;
            }
        }
    }

    /// <summary>
    /// Starts tracking idle time for a subtask after step completion.
    /// </summary>
    /// <param name="subtask">The subtask to track idle time for.</param>
    /// <param name="step">The last step the user completed.</param>
    public void StartIdleTracking(Task.Subtask subtask, Task.Step step)
    {
        lastProgressedSubtask = subtask;
        lastCompletedStep = step;

        float stepTimeout = defaultIdleThresholdInSeconds;

        if (subtask.Compleated())
        {
            stepTimeout = 0f;
        }


        // If timeout is 0, disable tracking until new subtask has started progress
        if (stepTimeout <= 0)
        {
            isTrackingIdleTime = false;
            idleTimer = 0f;
            Debug.Log($"Idle tracking disabled for subtask: {subtask.SubtaskName}, step: {step.StepName}");
            return;
        }

        // Otherwise, start tracking with the specified timeout
        isTrackingIdleTime = true;
        idleTimer = 0f;
        thresholdReached = false;
        idleThresholdInSeconds = stepTimeout;
        nextIdleCheckTime = Time.time + idleCheckIntervalInSeconds;

        Debug.Log($"Started idle tracking for subtask: {subtask.SubtaskName}, step: {step.StepName}, timeout: {idleThresholdInSeconds}s");
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
            thresholdReached = false;
            nextIdleCheckTime = Time.time + idleCheckIntervalInSeconds;
            Debug.Log("Idle timer reset due to user activity");
        }
    }


    /// <summary>
    /// Handles the idle threshold exceeded event from IdleTimer
    /// </summary>
    /// <param name="idleData">Data about the idle state</param>
    private void HandleIdleThresholdReached(IdleData idleData)
    {
        Debug.Log($"Idle threshold reached: {idleData.idleTimeSeconds} seconds on subtask: {idleData.currentSubtaskName}, step: {idleData.lastActiveStep}");
        // Update the question to reflect the idle state
        timeoutPrompt = $"User might need assistance with subtask '{lastProgressedSubtask.SubtaskName}'. They recently completed step '{lastCompletedStep.StepName}'. They have been idle for {Math.Round(idleTimer / 60, 1)} minutes.";
        
        if (ActionManager.Instance != null)
        {
            ActionManager.Instance.SendIdleTimeoutReport(timeoutPrompt); // Uncomment this line if you want to send timeout prompt on idletimeout
        }
    }

    /// <summary>
    /// Creates and sends a report about user idle status.
    /// </summary>
    private void SendIdleReport()
    {
        if (lastProgressedSubtask == null || lastCompletedStep == null)
            return;

        Debug.Log($"Idle threshold reached with timeout of {idleThresholdInSeconds} seconds");

        // Create idle data
        IdleData idleData = new IdleData
        {
            currentSubtaskName = lastProgressedSubtask.SubtaskName,
            idleTimeSeconds = idleTimer,
            idleThresholdSeconds = idleThresholdInSeconds,
            lastActiveStep = lastCompletedStep.StepName,
            subtaskStartTime = DateTime.Now.AddSeconds(-idleTimer).ToString("yyyy-MM-dd HH:mm:ss")
        };

        // Fire the event
        HandleIdleThresholdReached(idleData);
    }
}
