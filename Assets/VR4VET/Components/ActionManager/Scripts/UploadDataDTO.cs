using System;
using System.Collections.Generic;
using ProgressDTO;

namespace UploadDTO
{
    /// <summary>
    /// Data structure for uploading logged data to chat-service through AIRequest.
    /// Changes made to this class will need to be reflected in the server-side code.
    /// </summary>
    [Serializable]
    public class UploadDataDTO
    {
        /// <summary>
        /// The name of the scene where the user is currently located.
        /// </summary>
        public string scene_name;

        /// <summary>
        /// Information about the user logged through Questionaire in reception.
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
        /// This currently serves no purpose but is included for future use.
        /// </summary>
        public int NPC;

        /// <summary>
        /// The chat history between user and NPC.
        /// /summary>
        public List<Message> chatLog;
    }
}
