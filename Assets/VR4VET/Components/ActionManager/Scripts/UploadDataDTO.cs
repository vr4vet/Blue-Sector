using System;
using System.Collections.Generic;

/// <summary>
/// Data structure for uploading user progress and interactions.
/// </summary>
[Serializable]
public class UploadDataDTO
{
    public string user_name;

    /// <summary>
    /// The mode defined in the user profiling.
    /// </summary>
    public string user_mode;
    public List<string> user_actions;   // currently not used

    /// <summary>
    /// A list of progress data for tasks.
    /// </summary>
    public List<ProgressDataDTO> progress;

    /// <summary>
    /// The question that the user is asking.
    /// </summary>
    public string question;

    /// <summary>
    /// The ID of the NPC that the user is interacting with.
    /// </summary>
    public int NPC;

    /// <summary>
    /// Information about user idle time, if applicable.
    /// </summary>
    public IdleDataDTO idleData;
}