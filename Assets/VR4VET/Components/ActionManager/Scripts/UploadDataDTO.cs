using System;
using System.Collections.Generic;
using ProgressDTO.ProgressDataDTO;

namespace UploadDTO
{

    /// <summary>
    /// Data structure for uploading user progress and interactions.
    /// </summary>
    [Serializable]
    public class UploadDataDTO
    {
        /// <summary>
        /// Dictionary containing information about the user.
        /// </summary>
        public Dictionary<String, String> user_information;

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
}
