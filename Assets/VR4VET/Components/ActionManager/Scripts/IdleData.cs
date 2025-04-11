using System;

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