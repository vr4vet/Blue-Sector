using System;
using System.Collections.Generic;
using ProgressDTO;

namespace UploadDTO
{
    /// <summary>
    /// Data structure for uploading logged data to chat-service through AIRequest.
    /// Changes made to this class will need to be reflected in the server-side code.
    /// </summary>

    /// All of this needs to be reformatted to match RAGdoll body

    [Serializable]
    public class UploadDataDTO
    {
        public string agent_id = "6a3d0d73e3d056dad668064c"; // The agent name of which to communicate
        public string active_agent_role_id; // the agent role to use for current query
        public string access_key = "SSuEKoMZdBBHrGQY60xQw2T7udTla3PmrcG-vd_5HR8="; //RAGdoll agent access key
        public List<Message> chat_log; // chat log so far to give the LLM context
        public List<string> user_information;
        public List<string> user_actions;

        // more RAGdoll fields to be added, just required ones for now
    }
}