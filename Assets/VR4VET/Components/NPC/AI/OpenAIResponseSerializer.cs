// Purpose: Data structures for OpenAI API JSON serialization.
// Note: Unchanged from the deprecated version.
using System;
using System.Collections.Generic;

// Field definitions for all of our serializable structures, for JSON requests and responses
[Serializable]
public class Message
{
    public string role; // "system", "user", or "assistant"
    public string content;
    // 'refusal' field seems specific, maybe keep commented unless needed
    // public object refusal;
}

[Serializable]
public class Choice
{
    public int index;
    public Message message;
    // 'logprobs' and 'finish_reason' might be useful for advanced analysis
    // public object logprobs;
    public string finish_reason; // e.g., "stop", "length"
}

// 'CompletionTokenDetails' seems overly specific, maybe remove if not used
// [Serializable]
// public class CompletionTokenDetails
// {
// 	public int reasoning_tokens;
// }

[Serializable]
public class Usage
{
    public int prompt_tokens;
    public int completion_tokens;
    public int total_tokens;
    // public CompletionTokenDetails completion_token_details; // Removed if not needed
}

[Serializable]
public class OpenAIResponse
{
    public string id;
    // public string object_; // 'object' is a keyword, renamed if needed or ignore
    public long created; // Unix timestamp
    public string model;
    public List<Choice> choices;
    public Usage usage;
    public string system_fingerprint;
}

[Serializable]
public class OpenAIRequest
{
    public string model;
    public List<Message> messages;
    public int max_tokens;
    // Optional parameters can be added here (e.g., temperature, top_p)
    // public float? temperature; // Use nullable for optional parameters
}