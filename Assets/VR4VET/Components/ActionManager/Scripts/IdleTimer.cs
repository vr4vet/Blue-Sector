/* Developer: Erik Le Blanc
 * Ask your questions on github: https://github.com/erikleblanc
 */
using System;
using UnityEngine;
using Task;

/// <summary>
/// Tracks user idle time and reports when the user has been idle for too long.
/// Should be connected to send requests through NPCs when the user is idle.
/// Some parts might be deprecated or not used.
/// </summary
public class IdleTimer : MonoBehaviour
{
    private float _idleTimer = 0f;
    private bool _isTrackingIdleTime = false;
    private Task.Subtask _lastProgressedSubtask;
    private Task.Step _lastCompletedStep;
    private string _timeoutPrompt;

    // Default threshold (in seconds)
    [SerializeField] private float _defaultIdleThresholdInSeconds; // Default 2 minutes

    // How often to check if the user is still idle (in seconds)
    [SerializeField] private float _idleCheckIntervalInSeconds;

    // Current idle threshold for the active subtask
    private float _idleThresholdInSeconds;

    private float _nextIdleCheckTime = 0f;

    private bool _thresholdReached = false;

    /// <summary>
    /// Update is called once per frame and handles the idle timer tracking.
    /// </summary>
    private void Update()
    {
        if (_isTrackingIdleTime && _lastProgressedSubtask != null && _lastCompletedStep != null)
        {
            // Increment idle timer
            _idleTimer += Time.deltaTime;

            // First time threshold is reached
            if (!_thresholdReached && _idleTimer >= _idleThresholdInSeconds)
            {
                // Send idle report immediately when threshold is first reached
                SendIdleReport();
                _thresholdReached = true;
                _nextIdleCheckTime = Time.time + _idleCheckIntervalInSeconds;
            }
            // Subsequent interval checks after threshold was already reached
            else if (_thresholdReached && Time.time >= _nextIdleCheckTime)
            {
                // Send additional idle reports at the specified intervals
                SendIdleReport();
                _nextIdleCheckTime = Time.time + _idleCheckIntervalInSeconds;
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
        _lastProgressedSubtask = subtask;
        _lastCompletedStep = step;

        float stepTimeout = _defaultIdleThresholdInSeconds;

        if (subtask.Compleated())
        {
            stepTimeout = 0f;
        }

        // If timeout is 0, disable tracking until new subtask has started progress
        if (stepTimeout <= 0)
        {
            _isTrackingIdleTime = false;
            _idleTimer = 0f;
            Debug.Log($"Idle tracking disabled for subtask: {subtask.SubtaskName}, step: {step.StepName}");
            return;
        }

        // Otherwise, start tracking with the specified timeout
        _isTrackingIdleTime = true;
        _idleTimer = 0f;
        _thresholdReached = false;
        _idleThresholdInSeconds = stepTimeout;
        _nextIdleCheckTime = Time.time + _idleCheckIntervalInSeconds;

        Debug.Log($"Started idle tracking for subtask: {subtask.SubtaskName}, step: {step.StepName}, timeout: {_idleThresholdInSeconds}s");
    }

    /// <summary>
    /// Stops tracking idle time.
    /// </summary>
    public void StopIdleTracking()
    {
        _isTrackingIdleTime = false;
        _idleTimer = 0f;

        Debug.Log("Stopped idle tracking");
    }

    /// <summary>
    /// Resets the idle timer without stopping tracking.
    /// Call this when the user performs any meaningful action.
    /// </summary>
    public void ResetIdleTimer()
    {
        if (_isTrackingIdleTime)
        {
            _idleTimer = 0f;
            _thresholdReached = false;
            _nextIdleCheckTime = Time.time + _idleCheckIntervalInSeconds;
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
        _timeoutPrompt = $"User might need assistance with subtask '{_lastProgressedSubtask.SubtaskName}'. They recently completed step '{_lastCompletedStep.StepName}'. They have been idle for {Math.Round(_idleTimer / 60, 1)} minutes.";
        
        if (ActionManager.Instance != null)
        {
            /*ActionManager.Instance.SendIdleTimeoutReport(timeoutPrompt);*/ // Uncomment this line if you want to send timeout prompt on idletimeout
        }
    }

    /// <summary>
    /// Creates and sends a report about user idle status.
    /// </summary>
    private void SendIdleReport()
    {
        if (_lastProgressedSubtask == null || _lastCompletedStep == null)
            return;

        Debug.Log($"Idle threshold reached with timeout of {_idleThresholdInSeconds} seconds");

        // Create idle data
        IdleData idleData = new IdleData
        {
            currentSubtaskName = _lastProgressedSubtask.SubtaskName,
            idleTimeSeconds = _idleTimer,
            idleThresholdSeconds = _idleThresholdInSeconds,
            lastActiveStep = _lastCompletedStep.StepName,
            subtaskStartTime = DateTime.Now.AddSeconds(-_idleTimer).ToString("yyyy-MM-dd HH:mm:ss")
        };

        // Fire the event
        HandleIdleThresholdReached(idleData);
    }
}
