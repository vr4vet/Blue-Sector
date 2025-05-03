// Purpose: Custom Inspector for the NPC Scriptable Object to handle TTS provider selection.
// Note: Unchanged from the deprecated version. Requires UnityEditor namespace.
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NPC))]
public class NPCInspector : Editor
{
    public override void OnInspectorGUI()
    {
        NPC npc = (NPC)target;

        // Draw the default inspector fields
        DrawDefaultInspector();

        // --- AI Configuration Section ---
        if (npc.IsAiNpc)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("AI Configuration", EditorStyles.boldLabel);

            // Ensure WitVoiceId always matches VoicePresetId
            npc.WitVoiceId = npc.VoicePresetId;

            // Context Prompt
            EditorGUILayout.LabelField("Context Prompt (Instructions for AI)");
            npc.ContextPrompt = EditorGUILayout.TextArea(npc.ContextPrompt, GUILayout.Height(60));

            // Max Tokens
            npc.MaxTokens = EditorGUILayout.IntField("Max Response Tokens", npc.MaxTokens);
        }

        // Apply changes if GUI was modified
        if (GUI.changed)
        {
            EditorUtility.SetDirty(npc);
        }
    }
}
#endif