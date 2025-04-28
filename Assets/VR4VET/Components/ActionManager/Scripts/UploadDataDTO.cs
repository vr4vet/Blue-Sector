using System;
using System.Collections.Generic;
using ProgressDTO;

namespace UploadDTO
{
    /// <summary>
    /// Data structure for uploading user progress and interactions.
    /// </summary>
    [Serializable]
    public class UploadDataDTO
    {
        /// <summary>
        /// List of strings containing information about the user.
        /// </summary>
        public List<string> user_information;

        /// <summary>
        /// List of user actions such as grabbed/dropped objects.
        /// </summary>
        public List<string> user_actions;

        /// <summary>
        /// A list of progress data for tasks.
        /// </summary>
        public List<ProgressDataDTO> progress;

        /// <summary>
        /// The ID of the NPC that the user is interacting with.
        /// </summary>
        public int NPC;

        /// <summary>
        /// The chat history between user and current NPC.
        /// </summary>
        public List<Message> chatLog;

        /// <summary>
        /// The name of the current scene.
        /// </summary>
        public string scene_name;
    }
}
