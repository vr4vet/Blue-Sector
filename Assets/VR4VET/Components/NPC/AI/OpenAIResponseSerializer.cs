using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;


/// <summary>
/// Message structure for chat messages to and from LLM.
/// Is needed for LLM to understand the context of the conversation.
/// role is either user, assistant or system.
/// content is the actual message.
/// </summary>
[Serializable]
public class Message
{
    public string role;
    public string content;
}

/// <summary>
/// Not enitrely sure why this is called Choice, but represents a response from the LLM.
/// </summary>
[Serializable]
public class Choice
{
    public int index;
    public Message message;     // Only message is used currently
    public string finish_reason;
}


/// <summary>
/// Not used currently, but can be used to track token usage.
/// </summary>
[Serializable]
public class Usage
{
    public int prompt_tokens;
    public int completion_tokens;
    public int total_tokens;
}

/// <summary>
/// The response json structure returned from LLM backend call.
/// The majority of these fields are currently not used, but are included for future use.
/// response is used as the main response text, and added to chat history.
/// </summary>
[Serializable]
public class  LLMResponse
{
    public string id;
    public long created;
    public string model;
    public string provider;
    public List<Choice> choices;
    public Usage usage;
    public string system_fingerprint;

    public FunctionCall function_call;
    public string response => choices?.FirstOrDefault()?.message.content;

    public List<string> context_used;
    public Dictionary<string, object> metadata;
}

/// <summary>
/// LLM may return a function call, in which function will be called upon recieving response.
/// Currently, only teleporting is supported.
/// </summary>
[Serializable]
public class FunctionCall
{
    public string function_name;
    public string[] function_parameters;
}