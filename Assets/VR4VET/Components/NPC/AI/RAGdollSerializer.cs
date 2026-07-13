using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RAGRoot
{
    public RAGdollResponse response;
}

    [Serializable]
    public class RAGdollResponse
    {
        public string id;
        public long created;
        public string model;
        public string agent_id;
        public string active_role;
        public string[] accessible_documents;
        public string[] context_used;
        public MetaDataClass metadata;
        public function_call function_call;
        public string function_calls;
        public string response;
    }
[Serializable]
public class MetaDataClass
{
    public int response_length;
    public string agent_name;
    public int num_context_retrieved;
    public int num_progress_items;
}

[Serializable]
public class function_call
{
    public string function_name;
    public CallParameters[] function_parameters;
}

[Serializable]
public partial class CallParameters
{
    
}