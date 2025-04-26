using System;
using UnityEngine;

// Define TTSProvider enum outside the class if used by multiple scripts
public enum TTSProvider
{
    Wit,
    OpenAI
}

[CreateAssetMenu(menuName = "NPCScriptableObjects/NPC")]
public class NPC : ScriptableObject
{
    // --- Existing Fields ---
    public String NameOfNPC;
    public GameObject NpcPrefab;
    public GameObject CharacterModel;
    public Avatar CharacterAvatar;
    public int VoicePresetId; // Keep this for standard Wit preset selection if needed outside AI
    public Vector3 SpawnPosition;
    public Vector3 SpawnRotation;
    public bool ShouldFollow;
    public bool WithoutDialogue;
    public DialogueTree[] DialogueTreesSO;
    public TextAsset[] DialogueTreeJSON;
    public RuntimeAnimatorController runtimeAnimatorController;
    [Range(0, 1)]
    public float SpatialBlend;
    [Range(1, 100)]
    public float MinDistance;
    public bool GlobalChatMemory;

    // --- New AI Fields ---
    [Header("AI Settings")]
    public bool isAiNpc = false; // Flag to enable AI components/behaviour

    // Fields below are hidden in default inspector, shown by NPCInspector.cs if isAiNpc is true
    [HideInInspector] public TTSProvider selectedTTSProvider = TTSProvider.Wit; // Default TTS provider for AI
    [HideInInspector] public int WitVoiceId = 0; // Default Wit Voice ID for AI (Matches VoicePresetId convention?)
    [HideInInspector] public string OpenAiVoiceId = "alloy"; // Default OpenAI Voice ID

    [HideInInspector] // Shown by custom inspector
    [TextArea(3, 10)]
    public string contextPrompt = "You are a helpful assistant."; // Default context

    [HideInInspector] // Shown by custom inspector
    public int maxTokens = 150; // Default max response tokens
}