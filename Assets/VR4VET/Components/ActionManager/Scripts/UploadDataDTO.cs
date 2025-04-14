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

    /// <summary>
    /// List of user actions such as grabbed/dropped objects.
    /// </summary>
    public List<string> user_actions;

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
    /// The chat history of the user and the chatbot.
    /// /summary>
    public List<String> chat_history;
}