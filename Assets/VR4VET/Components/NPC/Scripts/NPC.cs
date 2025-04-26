using System;
using UnityEngine;

[CreateAssetMenu(menuName = "NPCScriptableObjects/NPC")]
public class NPC : ScriptableObject
{
    // --- Existing Fields ---
    public String NameOfNPC;
    public GameObject NpcPrefab;
    public GameObject CharacterModel;
    public Avatar CharacterAvatar;
    public int VoicePresetId; // Used for standard Wit preset selection
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

    // --- AI Fields ---
    [Header("AI Settings")]
    public bool isAiNpc = false; // Flag to enable AI components/behavior

    // Fields below are hidden in the default inspector, shown by NPCInspector.cs if isAiNpc is true
    [HideInInspector] public int WitVoiceId = 0; // Always synchronized with VoicePresetId

    [HideInInspector]
    [TextArea(3, 10)]
    public string contextPrompt = "You are a helpful assistant."; // Default context

    [HideInInspector]
    public int maxTokens = 150; // Default max response tokens
}